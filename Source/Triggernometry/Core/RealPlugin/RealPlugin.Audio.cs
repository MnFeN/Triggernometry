using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Speech.Synthesis;
using Triggernometry.Core.Actions;
using Triggernometry.Localization;
using WMPLib;

namespace Triggernometry.Core
{

    public partial class RealPlugin
    {
        internal Dictionary<string, DateTime> TtsRepetitions = new Dictionary<string, DateTime>();
        internal Dictionary<string, DateTime> SoundRepetitions = new Dictionary<string, DateTime>();
        private DateTime oldestTtsRepetition = DateTime.MaxValue;
        private DateTime oldestSoundRepetition = DateTime.MaxValue;
        internal WindowsMediaPlayer wmp;
        internal SpeechSynthesizer tts;
        internal bool WMPUnavailable;

        private void InitAudio(ref string exwhere)
        {
            exwhere = I18n.Translate("internal/Plugin/iniwmp", "trying to initialize Windows Media Player");
            WMPUnavailable = false;
            try
            {
                wmp = new WindowsMediaPlayer();
            }
            catch (Exception ex)
            {
                WMPUnavailable = true;
                FilteredAddToLog(DebugLevelEnum.Error, I18n.Translate("internal/Plugin/iniwmperror", "Error while {0} ({1}), going to fallback to ACT hooks for sound playback", exwhere, ex.Message));
            }
            exwhere = I18n.Translate("internal/Plugin/initts", "trying to initialize TTS");
            tts = new SpeechSynthesizer();
            ui.wmp = wmp;
            ui.tts = tts;
        }

        private void DeInitAudio()
        {
            wmp?.close();
            wmp = null;
            tts?.Dispose();
            tts = null;
        }

        internal void TtsPlaybackAct(Context ctx, ActionPlaySpeech a)
        {
            string text = TtsPlaybackGetTextFromAction(ctx, a);
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }
            lock (TtsRepetitions)
            {
                if (RegisterRepetition(TtsRepetitions, Config.TtsRepCooldown, text, ref oldestTtsRepetition) == false)
                {
                    return;
                }
            }
            TtsPlaybackHook(text);
        }

        internal void SoundPlaybackAct(Context ctx, ActionPlaySound a, string filename)
        {
            lock (SoundRepetitions)
            {
                if (RegisterRepetition(SoundRepetitions, Config.SoundRepCooldown, filename, ref oldestSoundRepetition) == false)
                {
                    return;
                }
            }
            double vol = ctx.EvaluateNumericExpression(a.ExpressionLogger, ctx, a.Volume);
            vol *= ctx.Plugin.Config.SfxVolumeAdjustment / 100.0;
            if (vol < 0.0)
            {
                vol = 0.0;
            }
            if (vol > 100.0)
            {
                vol = 100.0;
            }
            SoundPlaybackHook(filename, (int)Math.Floor(vol));
        }

        internal string TtsPlaybackGetTextFromAction(Context ctx, ActionPlaySpeech a)
        {
            string text = ctx.EvaluateStringExpression(a.ExpressionLogger, ctx, a.Message);
            if (ctx.Plugin != null)
            {
                text = ctx.Plugin.Config.PerformSubstitution(text, Configuration.Substitution.SubstitutionScopeEnum.TextToSpeech);
            }
            return text;
        }

        internal void TtsPlaybackSelf(Context ctx, ActionPlaySpeech a)
        {
            string text = TtsPlaybackGetTextFromAction(ctx, a);
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }
            lock (TtsRepetitions)
            {
                if (RegisterRepetition(TtsRepetitions, Config.TtsRepCooldown, text, ref oldestTtsRepetition) == false)
                {
                    return;
                }
            }
            SpeechSynthesizer mytts = new SpeechSynthesizer();
            double vol = ctx.EvaluateNumericExpression(a.ExpressionLogger, ctx, a.Volume);
            vol *= ctx.Plugin.Config.TtsVolumeAdjustment / 100.0;
            if (vol < 0.0)
            {
                vol = 0.0;
            }
            if (vol > 100.0)
            {
                vol = 100.0;
            }
            double rate = ctx.EvaluateNumericExpression(a.ExpressionLogger, ctx, a.Rate);
            if (rate < -10.0)
            {
                rate = -10.0;
            }
            if (rate > 10.0)
            {
                rate = 10.0;
            }
            mytts.Volume = (int)Math.Ceiling(vol);
            mytts.Rate = (int)Math.Ceiling(rate);
            mytts.SpeakAsync(text);
        }

        internal void SoundPlaybackSelf(Context ctx, ActionPlaySound a, string filename)
        {
            lock (SoundRepetitions)
            {
                if (RegisterRepetition(SoundRepetitions, Config.SoundRepCooldown, filename, ref oldestSoundRepetition) == false)
                {
                    return;
                }
            }
            WindowsMediaPlayer mywmp;
            Action stopTracking = null;
            bool useSharedPlayer = a.UseSharedPlayer;
            if (!useSharedPlayer)
            {
                mywmp = new WindowsMediaPlayer();
                stopTracking = a.TrackPlayer(mywmp);
            }
            else
            {
                mywmp = wmp;
            }
            try
            {
                double vol = ctx.EvaluateNumericExpression(a.ExpressionLogger, ctx, a.Volume);
                vol *= ctx.Plugin.Config.SfxVolumeAdjustment / 100.0;
                if (vol < 0.0)
                {
                    vol = 0.0;
                }
                if (vol > 100.0)
                {
                    vol = 100.0;
                }
                // 新播放器没有旧媒体，只对共享实例执行重置。
                if (useSharedPlayer) mywmp.URL = "";
                mywmp.settings.volume = (int)Math.Ceiling(vol);
                mywmp.URL = filename;
            }
            catch
            {
                stopTracking?.Invoke();
                throw;
            }
        }

        internal void TtsPlaybackExternal(Context ctx, ActionPlaySpeech a)
        {
            string proc = (Config.TtsExternalApp ?? "").Trim();
            if (proc.Length == 0)
            {
                return;
            }
            string text = TtsPlaybackGetTextFromAction(ctx, a);
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }
            lock (TtsRepetitions)
            {
                if (RegisterRepetition(TtsRepetitions, Config.TtsRepCooldown, text, ref oldestTtsRepetition) == false)
                {
                    return;
                }
            }
            string args = Config.TtsExternalAppArgs ?? "";
            args = args.Replace("$source", text);
            Process.Start(proc, args)?.Dispose();
        }

        internal void SoundPlaybackExternal(Context ctx, ActionPlaySound a, string filename)
        {
            lock (SoundRepetitions)
            {
                if (RegisterRepetition(SoundRepetitions, Config.SoundRepCooldown, filename, ref oldestSoundRepetition) == false)
                {
                    return;
                }
            }
            string proc = (Config.SoundExternalApp ?? "").Trim();
            if (proc.Length == 0)
            {
                return;
            }
            string args = Config.SoundExternalAppArgs ?? "";
            args = args.Replace("$source", filename);
            Process.Start(proc, args)?.Dispose();
        }

        /// <summary>在存储区锁内登记去重时间，仅可能存在过期项时扫描清理。</summary>
        internal bool RegisterRepetition(Dictionary<string, DateTime> repstore, int cooldown, string item, ref DateTime oldestEntry)
        {
            if (cooldown == 0)
            {
                return true;
            }
            DateTime now = DateTime.Now;
            if (repstore.TryGetValue(item, out DateTime last) == true)
            {
                if (last.AddMilliseconds(cooldown) > now)
                {
                    return false;
                }
            }
            repstore[item] = now;
            if (now < oldestEntry) oldestEntry = now;
            DateTime expiresBefore = now.AddMilliseconds(-(double)cooldown);
            if (oldestEntry >= expiresBefore) return true;

            // 起点可能已被覆盖；保留这个较早的下界只会多扫描一次，不会漏删过期项。
            oldestEntry = DateTime.MaxValue;
            List<string> expired = null;
            foreach (var pair in repstore)
            {
                if (pair.Value < expiresBefore)
                    (expired ?? (expired = new List<string>())).Add(pair.Key);
                else if (pair.Value < oldestEntry)
                    oldestEntry = pair.Value;
            }
            if (expired != null)
                foreach (string key in expired) repstore.Remove(key);
            return true;
        }

        internal void TtsPlaybackSmart(Context ctx, ActionPlaySpeech a)
        {
            switch (a.AudioTarget)
            {
                case Configuration.AudioRoutingMethodEnum.None:
                    if (Config.TtsMethod == Configuration.AudioRoutingMethodEnum.ExternalApplication)
                    {
                        TtsPlaybackExternal(ctx, a);
                    }
                    else if (Config.TtsMethod == Configuration.AudioRoutingMethodEnum.ACT)
                    {
                        TtsPlaybackAct(ctx, a);
                    }
                    else if (Config.TtsMethod == Configuration.AudioRoutingMethodEnum.Triggernometry)
                    {
                        TtsPlaybackSelf(ctx, a);
                    }
                    break;
                case Configuration.AudioRoutingMethodEnum.ACT:
                    TtsPlaybackAct(ctx, a);
                    break;
                case Configuration.AudioRoutingMethodEnum.Triggernometry:
                    TtsPlaybackSelf(ctx, a);
                    break;
            }
        }

        internal void SoundPlaybackSmart(Context ctx, ActionPlaySound a)
        {
            string filename = ctx.EvaluateStringExpression(a.ExpressionLogger, ctx, a.Filename);
            Uri u = new Uri(filename);
            if (u.IsFile == false)
            {
                string fn = Path.Combine(ConfigPath, "TriggernometryRemoteSounds");
                if (Directory.Exists(fn) == false)
                {
                    Directory.CreateDirectory(fn);
                }
                string ext = Path.GetExtension(u.LocalPath);
                fn = Path.Combine(fn, GenerateHash(u.AbsoluteUri) + Path.GetExtension(u.LocalPath));
                bool fromcache = false;
                if (File.Exists(fn) == true)
                {
                    FileInfo fi = new FileInfo(fn);
                    DateTime dt = DateTime.Now.AddMinutes(0 - Config.CacheSoundExpiry);
                    if (fi.LastWriteTime > dt)
                    {
                        filename = fn;
                        fromcache = true;
                    }
                }
                if (fromcache == false)
                {
                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers["User-Agent"] = "Triggernometry Sound Retriever";
                        byte[] data = wc.DownloadData(u.AbsoluteUri);
                        File.WriteAllBytes(fn, data);
                        filename = fn;
                    }
                }
            }
            switch (a.AudioTarget)
            {
                case Configuration.AudioRoutingMethodEnum.None:
                    if (Config.SoundMethod == Configuration.AudioRoutingMethodEnum.ExternalApplication)
                    {
                        SoundPlaybackExternal(ctx, a, filename);
                    }
                    else if (WMPUnavailable == true || Config.SoundMethod == Configuration.AudioRoutingMethodEnum.ACT)
                    {
                        SoundPlaybackAct(ctx, a, filename);
                    }
                    else if (Config.SoundMethod == Configuration.AudioRoutingMethodEnum.Triggernometry)
                    {
                        SoundPlaybackSelf(ctx, a, filename);
                    }
                    break;
                case Configuration.AudioRoutingMethodEnum.ACT:
                    SoundPlaybackAct(ctx, a, filename);
                    break;
                case Configuration.AudioRoutingMethodEnum.Triggernometry:
                    SoundPlaybackSelf(ctx, a, filename);
                    break;
            }
        }

    }
}
