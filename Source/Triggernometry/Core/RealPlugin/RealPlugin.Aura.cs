using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Triggernometry.Core.Actions;
using Triggernometry.UI.CustomControls;
using Triggernometry.UI.Forms;
using Triggernometry.Localization;
using Triggernometry.Utilities;

namespace Triggernometry.Core
{

    public partial class RealPlugin
    {
        internal Scarborough.Scarborough sc = null;
        private bool _usingScarborough = false;

        internal Dictionary<string, AuraContainerForm> imageauras = new Dictionary<string, AuraContainerForm>();
        internal Dictionary<string, AuraContainerForm> textauras = new Dictionary<string, AuraContainerForm>();

        internal Thread AuraUpdateThread = null;

        private void InitAura()
        {
            AuraUpdateThread = new Thread(new ThreadStart(AuraUpdateThreadProc));
            AuraUpdateThread.Name = "AuraUpdateThread";
            AuraUpdateThread.Start();

            if (cfg.UseScarborough == true)
            {
                sc = new Scarborough.Scarborough();
                sc.plug = this;
                _usingScarborough = true;
            }
        }

        private void DeInitAura()
        {
            if (AuraUpdateThread != null)
            {
                if (AuraUpdateThread.Join(5000) == false)
                {
                    AuraUpdateThread.Abort();
                }
                AuraUpdateThread = null;
            }
            if (sc != null)
            {
                sc.Shutdown();
                sc.Dispose();
                sc = null;
            }
        }

        private bool _HideAllAuras = false;
        internal bool HideAllAuras
        {
            get
            {
                return _HideAllAuras;
            }
            set
            {
                if (value != _HideAllAuras)
                {
                    _HideAllAuras = value;
                    if (_HideAllAuras == true)
                    {
                        ProcessAuraControl(true);
                    }
                    else
                    {
                        ProcessAuraControl(false);
                    }
                }
            }
        }

        private void ProcessAuraControl(bool hideAuras)
        {
            if (hideAuras == true)
            {
                if (sc != null)
                {
                    sc.HideAllItems();
                }
                else
                {
                    foreach (KeyValuePair<string, AuraContainerForm> kp in textauras)
                    {
                        kp.Value.Hide();
                    }
                    foreach (KeyValuePair<string, AuraContainerForm> kp in imageauras)
                    {
                        kp.Value.Hide();
                    }
                }
            }
            else
            {
                if (sc != null)
                {
                    sc.ShowAllItems();
                }
                else
                {
                    foreach (KeyValuePair<string, AuraContainerForm> kp in textauras)
                    {
                        kp.Value.Show();
                    }
                    foreach (KeyValuePair<string, AuraContainerForm> kp in imageauras)
                    {
                        kp.Value.Show();
                    }
                }
            }
        }

        private void AuraUpdateThreadProc()
        {
            WaitHandle[] wh = new WaitHandle[1];
            wh[0] = ExitEvent;
            int numTicks = 0, procticks = 0;
            DateTime prevTick, tickTime;
            if (mainform.IsHandleCreated == false)
            {
                do
                {
                    Thread.Sleep(100);
                } while (mainform.IsHandleCreated == false);
            }
            prevTick = DateTime.Now;
            double msSince, lag = 0.0;
            while (true)
            {
                if (ExitEvent.WaitOne(20) == true)
                {
                    return;
                }
                if (procticks >= 10)
                {
                    VerifyProcessWindow();
                    procticks = 0;
                }
                tickTime = DateTime.Now;
                msSince = (tickTime - prevTick).TotalMilliseconds + lag;
                numTicks = (int)Math.Floor(msSince / 20.0);
                //System.Diagnostics.Debug.WriteLine(String.Format("{0}.{1} -> {2}.{3} = {4}+{5} = {6}x", prevTick, prevTick.Millisecond, tickTime, tickTime.Millisecond, msSince - lag, lag, numTicks));
                lag = msSince - numTicks * 20;
                prevTick = tickTime;
                UpdateAuras(numTicks);
                procticks++;
            }
        }

        private void VerifyProcessWindow()
        {
            HideAllAuras = !string.IsNullOrWhiteSpace(cfg.WindowToMonitor)
                && !WindowsUtils.IsInFocus(cfg.WindowToMonitor);
        }

        internal void UpdateAuras(int numTicks)
        {
            lock (imageauras)
            {
                List<string> toRem = new List<string>();
                foreach (KeyValuePair<string, AuraContainerForm> kp in imageauras)
                {
                    if (kp.Value.UpdateAura(numTicks) == false)
                    {
                        toRem.Add(kp.Key);
                    }
                }
                foreach (string rem in toRem)
                {
                    imageauras.Remove(rem);
                }
            }
            lock (textauras)
            {
                List<string> toRem = new List<string>();
                foreach (KeyValuePair<string, AuraContainerForm> kp in textauras)
                {
                    if (kp.Value.UpdateAura(numTicks) == false)
                    {
                        toRem.Add(kp.Key);
                    }
                }
                foreach (string rem in toRem)
                {
                    textauras.Remove(rem);
                }
            }
        }

        internal void ImageAuraManagement(Context ctx, ActionOverlayImage a)
        {
            if (_usingScarborough == true)
            {
                SbImageAuraManagement(ctx, a);
            }
            else
            {
                LegacyImageAuraManagement(ctx, a);
            }
        }

        internal void TextAuraManagement(Context ctx, ActionOverlayText a)
        {
            if (_usingScarborough == true)
            {
                SbTextAuraManagement(ctx, a);
            }
            else
            {
                LegacyTextAuraManagement(ctx, a);
            }
        }

        internal void SbImageAuraManagement(Context ctx, ActionOverlayImage a)
        {
            switch (a.Operation)
            {
                case ActionOverlayImage.OperationEnum.Activate:
                    {
                        string ax = ctx.EvaluateStringExpression(a.ActionContextLogger, ctx, a.Name);
                        FilteredAddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Plugin/actimageaura", "Activating image aura '{0}'", ax));
                        try
                        {
                            Scarborough.ScarboroughImage si = new Scarborough.ScarboroughImage(sc);
                            si.ImageExpression = a.Filename;
                            si.InitXExpression = a.XIniExpression;
                            si.InitYExpression = a.YIniExpression;
                            si.InitWExpression = a.WIniExpression;
                            si.InitHExpression = a.HIniExpression;
                            si.InitOExpression = a.OIniExpression;
                            si.UpdateXExpression = a.XTickExpression;
                            si.UpdateYExpression = a.YTickExpression;
                            si.UpdateWExpression = a.WTickExpression;
                            si.UpdateHExpression = a.HTickExpression;
                            si.UpdateOExpression = a.OTickExpression;
                            si.TTLExpression = a.TTLTickExpression;
                            si.Display = a.SizeMode;
                            si.ctx = ctx;
                            sc.Activate(ax, si);
                        }
                        catch (Exception ex)
                        {
                            FilteredAddToLog(DebugLevelEnum.Error, I18n.Translate("internal/Plugin/exactimageaura", "Exception '{0}' when activating image aura '{1}'", ex.Message, ax));
                        }
                    }
                    break;
                case ActionOverlayImage.OperationEnum.Deactivate:
                    {
                        string ax = ctx.EvaluateStringExpression(a.ActionContextLogger, ctx, a.Name);
                        FilteredAddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Plugin/deactimageaura", "Deactivating image aura '{0}'", ax));
                        sc.Deactivate(ax, Scarborough.Scarborough.ItemAction.ItemTypeEnum.Image);
                    }
                    break;
                case ActionOverlayImage.OperationEnum.DeactivateAll:
                    {
                        FilteredAddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Plugin/deactallimageaura", "Deactivating all image auras"));
                        sc.DeactivateAllImages();
                    }
                    break;
                case ActionOverlayImage.OperationEnum.DeactivateRegex:
                    {
                        string ax = ctx.EvaluateStringExpression(a.ActionContextLogger, ctx, a.Name);
                        FilteredAddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Plugin/deactimageaurarex", "Deactivating image auras matching '{0}'", ax));
                        sc.DeactivateRegex(ax, Scarborough.Scarborough.ItemAction.ItemTypeEnum.Image);
                    }
                    break;
                case ActionOverlayImage.OperationEnum.DeactivateTrigger:
                    {
                        FilteredAddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Plugin/deactimageauratrig", "Deactivating image auras from trigger '{0}' ({1})", ctx.Trigger.LogName, ctx.Trigger.Id));
                        sc.DeactivateTrigger(ctx.Trigger, Scarborough.Scarborough.ItemAction.ItemTypeEnum.Image);
                    }
                    break;
            }
        }

        internal void LegacyImageAuraManagement(Context ctx, ActionOverlayImage a)
        {
            if (mainform.InvokeRequired == true)
            {
                string ax = ctx.EvaluateStringExpression(a.ActionContextLogger, ctx, a.Name);
                FilteredAddToLog(DebugLevelEnum.Verbose, I18n.Translate("internal/Plugin/invokeimageaura", "Invoke required for image aura '{0}'", ax));
                mainform.Invoke(new Action<Context, ActionOverlayImage>(ImageAuraManagement), ctx, a);
                return;
            }
            switch (a.Operation)
            {
                case ActionOverlayImage.OperationEnum.Activate:
                    {
                        string ax = ctx.EvaluateStringExpression(a.ActionContextLogger, ctx, a.Name);
                        FilteredAddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Plugin/actimageaura", "Activating image aura '{0}'", ax));
                        AuraContainerForm acf = null;
                        bool newAura = false;
                        try
                        {
                            lock (ctx.Plugin.imageauras)
                            {
                                if (ctx.Plugin.imageauras.ContainsKey(ax) == true)
                                {
                                    acf = ctx.Plugin.imageauras[ax];
                                }
                                else
                                {
                                    acf = new AuraContainerForm(AuraContainerForm.AuraTypeEnum.Image);
                                    acf.plug = this;
                                    acf.AuraName = ax;
                                    newAura = true;
                                }
                                acf.AuraPrepare();
                                acf.ctx = ctx;
                                acf.ImageExpression = a.Filename;
                                acf.Display = a.SizeMode;
                                acf.Left = acf.EvaluateNumericExpression(ctx, a.XIniExpression);
                                acf.Top = acf.EvaluateNumericExpression(ctx, a.YIniExpression);
                                int i = acf.EvaluateNumericExpression(ctx, a.WIniExpression);
                                if (i < 0)
                                {
                                    i = 0;
                                }
                                acf.Width = i;
                                if (i < 0)
                                {
                                    i = 0;
                                }
                                i = acf.EvaluateNumericExpression(ctx, a.HIniExpression);
                                if (i < 0)
                                {
                                    i = 0;
                                }
                                acf.Height = i;
                                i = acf.EvaluateNumericExpression(ctx, a.OIniExpression);
                                if (i < 0)
                                {
                                    i = 0;
                                }
                                if (i > 100)
                                {
                                    i = 100;
                                }
                                acf.PresentableOpacity = i;
                                acf.XExpression = a.XTickExpression;
                                acf.YExpression = a.YTickExpression;
                                acf.WExpression = a.WTickExpression;
                                acf.HExpression = a.HTickExpression;
                                acf.OExpression = a.OTickExpression;
                                acf.TTLExpression = a.TTLTickExpression;
                                acf.AuraActivate();
                                if (newAura == true)
                                {
                                    ctx.Plugin.imageauras[ax] = acf;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            FilteredAddToLog(DebugLevelEnum.Error, I18n.Translate("internal/Plugin/exactimageaura", "Exception '{0}' when activating image aura '{1}'", ex.Message, ax));
                            if (acf != null)
                            {
                                if (newAura == true)
                                {
                                    acf.Dispose();
                                }
                                else
                                {
                                    acf.AuraDeactivate();
                                }
                            }
                        }
                    }
                    break;
                case ActionOverlayImage.OperationEnum.Deactivate:
                    {
                        string ax = ctx.EvaluateStringExpression(a.ActionContextLogger, ctx, a.Name);
                        FilteredAddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Plugin/deactimageaura", "Deactivating image aura '{0}'", ax));
                        lock (ctx.Plugin.imageauras)
                        {
                            if (ctx.Plugin.imageauras.ContainsKey(ax) == true)
                            {
                                AuraContainerForm acf = ctx.Plugin.imageauras[ax];
                                acf.AuraDeactivate();
                            }
                        }
                    }
                    break;
                case ActionOverlayImage.OperationEnum.DeactivateAll:
                    {
                        FilteredAddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Plugin/deactallimageaura", "Deactivating all image auras"));
                        while (true)
                        {
                            lock (ctx.Plugin.imageauras)
                            {
                                if (ctx.Plugin.imageauras.Count > 0)
                                {
                                    AuraContainerForm acf = ctx.Plugin.imageauras.ElementAt(0).Value;
                                    acf.AuraDeactivate();
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                    break;
                case ActionOverlayImage.OperationEnum.DeactivateRegex:
                    {
                        string ax = ctx.EvaluateStringExpression(a.ActionContextLogger, ctx, a.Name);
                        Regex rex = new Regex(ax);
                        FilteredAddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Plugin/deactimageaurarex", "Deactivating image auras matching '{0}'", ax));
                        List<string> toRem = new List<string>();
                        lock (ctx.Plugin.imageauras)
                        {
                            toRem.AddRange(from sx in ctx.Plugin.imageauras where rex.IsMatch(sx.Key) select sx.Key);
                            foreach (string rem in toRem)
                            {
                                AuraContainerForm acf = ctx.Plugin.imageauras[rem];
                                acf.AuraDeactivate();
                            }
                        }
                    }
                    break;
                case ActionOverlayImage.OperationEnum.DeactivateTrigger:
                    {
                        FilteredAddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Plugin/deactimageauratrig", "Deactivating image auras from trigger '{0}' ({1})", ctx.Trigger.LogName, ctx.Trigger.Id));
                        List<string> toRem = new List<string>();
                        lock (ctx.Plugin.imageauras)
                        {
                            toRem.AddRange(from sx in ctx.Plugin.imageauras where sx.Value.ctx.Trigger == ctx.Trigger select sx.Key);
                            foreach (string rem in toRem)
                            {
                                AuraContainerForm acf = ctx.Plugin.imageauras[rem];
                                acf.AuraDeactivate();
                            }
                        }
                    }
                    break;
            }
        }

        internal void SbTextAuraManagement(Context ctx, ActionOverlayText a)
        {
            if (mainform.InvokeRequired == true)
            {
                string ax = ctx.EvaluateStringExpression(a.ActionContextLogger, ctx, a.Name);
                FilteredAddToLog(DebugLevelEnum.Verbose, I18n.Translate("internal/Plugin/invoketextaura", "Invoke required for text aura '{0}'", ax));
                mainform.Invoke(new Action<Context, ActionOverlayText>(TextAuraManagement), ctx, a);
                return;
            }
            switch (a.Operation)
            {
                case ActionOverlayText.OperationEnum.Activate:
                    {
                        string ax = ctx.EvaluateStringExpression(a.ActionContextLogger, ctx, a.Name);
                        FilteredAddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Plugin/acttextaura", "Activating text aura '{0}'", ax));
                        try
                        {
                            Scarborough.ScarboroughText si = new Scarborough.ScarboroughText(sc);
                            si.InitXExpression = a.XIniExpression;
                            si.InitYExpression = a.YIniExpression;
                            si.InitWExpression = a.WIniExpression;
                            si.InitHExpression = a.HIniExpression;
                            si.InitOExpression = a.OIniExpression;
                            si.UpdateXExpression = a.XTickExpression;
                            si.UpdateYExpression = a.YTickExpression;
                            si.UpdateWExpression = a.WTickExpression;
                            si.UpdateHExpression = a.HTickExpression;
                            si.UpdateOExpression = a.OTickExpression;
                            si.TTLExpression = a.TTLTickExpression;
                            FontStyle fs = FontStyle.Regular;
                            if ((a.Effect & ActionOverlayText.EffectEnum.Bold) != 0)
                            {
                                fs |= FontStyle.Bold;
                            }
                            if ((a.Effect & ActionOverlayText.EffectEnum.Italic) != 0)
                            {
                                fs |= FontStyle.Italic;
                            }
                            if ((a.Effect & ActionOverlayText.EffectEnum.Underline) != 0)
                            {
                                fs |= FontStyle.Underline;
                            }
                            if ((a.Effect & ActionOverlayText.EffectEnum.Strikeout) != 0)
                            {
                                fs |= FontStyle.Strikeout;
                            }
                            si.TextExpression = a.Text;
                            si.TextAlignment = (ActionOld.TextAuraAlignmentEnum)a.Alignment;
                            si.TextColor = ExpressionTextBox.ParseColor(
                                    ctx.EvaluateStringExpression(a.ActionContextLogger, ctx, a.ForeColor),
                                    Color.Black);
                            si.OutlineColor = ExpressionTextBox.ParseColor(
                                    ctx.EvaluateStringExpression(a.ActionContextLogger, ctx, a.OutlineColor),
                                    Color.Empty);
                            si.UseOutline = si.OutlineColor != Color.Empty;
                            si.FontName = a.Font;
                            si.FontSize = a.FontSize;
                            si.FontStyle = fs;
                            si.ctx = ctx;
                            si.BackgroundColor = ExpressionTextBox.ParseColor(
                                    ctx.EvaluateStringExpression(a.ActionContextLogger, ctx, a.BackColor),
                                    Color.Transparent);
                            sc.Activate(ax, si);
                        }
                        catch (Exception ex)
                        {
                            FilteredAddToLog(DebugLevelEnum.Error, I18n.Translate("internal/Plugin/exacttextaura", "Exception '{0}' when activating text aura '{1}'", ex.Message, ax));
                        }
                    }
                    break;
                case ActionOverlayText.OperationEnum.Deactivate:
                    {
                        string ax = ctx.EvaluateStringExpression(a.ActionContextLogger, ctx, a.Name);
                        FilteredAddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Plugin/deacttextaura", "Deactivating text aura '{0}'", ax));
                        sc.Deactivate(ax, Scarborough.Scarborough.ItemAction.ItemTypeEnum.Text);
                    }
                    break;
                case ActionOverlayText.OperationEnum.DeactivateAll:
                    {
                        FilteredAddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Plugin/deactalltextaura", "Deactivating all text auras"));
                        sc.DeactivateAllText();
                    }
                    break;
                case ActionOverlayText.OperationEnum.DeactivateRegex:
                    {
                        string ax = ctx.EvaluateStringExpression(a.ActionContextLogger, ctx, a.Name);
                        FilteredAddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Plugin/deacttextaurarex", "Deactivating text auras matching '{0}'", ax));
                        sc.DeactivateRegex(ax, Scarborough.Scarborough.ItemAction.ItemTypeEnum.Text);
                    }
                    break;
                case ActionOverlayText.OperationEnum.DeactivateTrigger:
                    {
                        FilteredAddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Plugin/deacttextauratrig", "Deactivating text auras from trigger '{0}' ({1})", ctx.Trigger.LogName, ctx.Trigger.Id));
                        sc.DeactivateTrigger(ctx.Trigger, Scarborough.Scarborough.ItemAction.ItemTypeEnum.Text);
                    }
                    break;
            }
        }

        internal void LegacyTextAuraManagement(Context ctx, ActionOverlayText a)
        {
            if (mainform.InvokeRequired == true)
            {
                string ax = ctx.EvaluateStringExpression(a.ActionContextLogger, ctx, a.Name);
                FilteredAddToLog(DebugLevelEnum.Verbose, I18n.Translate("internal/Plugin/invoketextaura", "Invoke required for text aura '{0}'", ax));
                mainform.Invoke(new Action<Context, ActionOverlayText>(TextAuraManagement), ctx, a);
                return;
            }
            switch (a.Operation)
            {
                case ActionOverlayText.OperationEnum.Activate:
                    {
                        string ax = ctx.EvaluateStringExpression(a.ActionContextLogger, ctx, a.Name);
                        FilteredAddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Plugin/acttextaura", "Activating text aura '{0}'", ax));
                        AuraContainerForm acf = null;
                        bool newAura = false;
                        try
                        {
                            lock (ctx.Plugin.textauras)
                            {
                                if (ctx.Plugin.textauras.ContainsKey(ax) == true)
                                {
                                    acf = ctx.Plugin.textauras[ax];
                                }
                                else
                                {
                                    acf = new AuraContainerForm(AuraContainerForm.AuraTypeEnum.Text);
                                    acf.plug = this;
                                    acf.AuraName = ax;
                                    newAura = true;
                                }
                                acf.AuraPrepare();
                                acf.ctx = ctx;
                                acf.TextExpression = a.Text;
                                acf.TextAlignment = (ActionOld.TextAuraAlignmentEnum)a.Alignment;
                                acf.TextColor = ExpressionTextBox.ParseColor(
                                    ctx.EvaluateStringExpression(a.ActionContextLogger, ctx, a.ForeColor),
                                    Color.Black);
                                acf.OutlineColor = ExpressionTextBox.ParseColor(
                                    ctx.EvaluateStringExpression(a.ActionContextLogger, ctx, a.OutlineColor),
                                    Color.Empty);
                                acf.UseOutline = acf.OutlineColor != Color.Empty;
                                if (acf.AuraFont != null)
                                {
                                    FontStyle fs = FontStyle.Regular;
                                    if ((a.Effect & ActionOverlayText.EffectEnum.Bold) != 0)
                                    {
                                        fs |= FontStyle.Bold;
                                    }
                                    if ((a.Effect & ActionOverlayText.EffectEnum.Italic) != 0)
                                    {
                                        fs |= FontStyle.Italic;
                                    }
                                    if ((a.Effect & ActionOverlayText.EffectEnum.Underline) != 0)
                                    {
                                        fs |= FontStyle.Underline;
                                    }
                                    if ((a.Effect & ActionOverlayText.EffectEnum.Strikeout) != 0)
                                    {
                                        fs |= FontStyle.Strikeout;
                                    }
                                    Font x = acf.AuraFont;
                                    if (x.Style != fs || x.Size != a.FontSize || x.Name != a.Font)
                                    {
                                        x.Dispose();
                                        acf.AuraFont = null;
                                    }
                                }
                                if (acf.AuraFont == null)
                                {
                                    FontStyle fs = FontStyle.Regular;
                                    if ((a.Effect & ActionOverlayText.EffectEnum.Bold) != 0)
                                    {
                                        fs |= FontStyle.Bold;
                                    }
                                    if ((a.Effect & ActionOverlayText.EffectEnum.Italic) != 0)
                                    {
                                        fs |= FontStyle.Italic;
                                    }
                                    if ((a.Effect & ActionOverlayText.EffectEnum.Underline) != 0)
                                    {
                                        fs |= FontStyle.Underline;
                                    }
                                    if ((a.Effect & ActionOverlayText.EffectEnum.Strikeout) != 0)
                                    {
                                        fs |= FontStyle.Strikeout;
                                    }
                                    float ex = a.FontSize;
                                    if (ex < 1)
                                    {
                                        ex = 1;
                                    }
                                    acf.AuraFont = new Font(a.Font, ex, fs, GraphicsUnit.Point);
                                }
                                acf.BackgroundColor = ExpressionTextBox.ParseColor(
                                    ctx.EvaluateStringExpression(a.ActionContextLogger, ctx, a.BackColor),
                                    Color.Transparent);
                                if (acf.BackgroundColor == Color.Transparent)
                                {
                                    acf.BackColor = acf.TransparencyKey;
                                }
                                acf.Left = acf.EvaluateNumericExpression(ctx, a.XIniExpression);
                                acf.Top = acf.EvaluateNumericExpression(ctx, a.YIniExpression);
                                int i = acf.EvaluateNumericExpression(ctx, a.WIniExpression);
                                if (i < 0)
                                {
                                    i = 0;
                                }
                                acf.Width = i;
                                if (i < 0)
                                {
                                    i = 0;
                                }
                                i = acf.EvaluateNumericExpression(ctx, a.HIniExpression);
                                if (i < 0)
                                {
                                    i = 0;
                                }
                                acf.Height = i;
                                i = acf.EvaluateNumericExpression(ctx, a.OIniExpression);
                                if (i < 0)
                                {
                                    i = 0;
                                }
                                if (i > 100)
                                {
                                    i = 100;
                                }
                                acf.PresentableOpacity = i;
                                acf.XExpression = a.XTickExpression;
                                acf.YExpression = a.YTickExpression;
                                acf.WExpression = a.WTickExpression;
                                acf.HExpression = a.HTickExpression;
                                acf.OExpression = a.OTickExpression;
                                acf.TTLExpression = a.TTLTickExpression;
                                acf.AuraActivate();
                                if (newAura == true)
                                {
                                    ctx.Plugin.textauras[ax] = acf;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            FilteredAddToLog(DebugLevelEnum.Error, I18n.Translate("internal/Plugin/exacttextaura", "Exception '{0}' when activating text aura '{1}'", ex.Message, ax));
                            if (acf != null)
                            {
                                if (newAura == true)
                                {
                                    acf.Dispose();
                                }
                                else
                                {
                                    acf.AuraDeactivate();
                                }
                            }
                        }
                    }
                    break;
                case ActionOverlayText.OperationEnum.Deactivate:
                    {
                        string ax = ctx.EvaluateStringExpression(a.ActionContextLogger, ctx, a.Name);
                        FilteredAddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Plugin/deacttextaura", "Deactivating text aura '{0}'", ax));
                        lock (ctx.Plugin.textauras)
                        {
                            if (ctx.Plugin.textauras.ContainsKey(ax) == true)
                            {
                                AuraContainerForm acf = ctx.Plugin.textauras[ax];
                                acf.AuraDeactivate();
                            }
                        }
                    }
                    break;
                case ActionOverlayText.OperationEnum.DeactivateAll:
                    {
                        FilteredAddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Plugin/deactalltextaura", "Deactivating all text auras"));
                        while (true)
                        {
                            lock (ctx.Plugin.textauras)
                            {
                                if (ctx.Plugin.textauras.Count > 0)
                                {
                                    AuraContainerForm acf = ctx.Plugin.textauras.ElementAt(0).Value;
                                    acf.AuraDeactivate();
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                    break;
                case ActionOverlayText.OperationEnum.DeactivateRegex:
                    {
                        string ax = ctx.EvaluateStringExpression(a.ActionContextLogger, ctx, a.Name);
                        Regex rex = new Regex(ax);
                        FilteredAddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Plugin/deacttextaurarex", "Deactivating text auras matching '{0}'", ax));
                        List<string> toRem = new List<string>();
                        lock (ctx.Plugin.textauras)
                        {
                            toRem.AddRange(from sx in ctx.Plugin.textauras where rex.IsMatch(sx.Key) select sx.Key);
                            foreach (string rem in toRem)
                            {
                                AuraContainerForm acf = ctx.Plugin.textauras[rem];
                                acf.AuraDeactivate();
                            }
                        }
                    }
                    break;
                case ActionOverlayText.OperationEnum.DeactivateTrigger:
                    {
                        FilteredAddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Plugin/deacttextauratrig", "Deactivating text auras from trigger '{0}' ({1})", ctx.Trigger.LogName, ctx.Trigger.Id));
                        List<string> toRem = new List<string>();
                        lock (ctx.Plugin.textauras)
                        {
                            toRem.AddRange(from sx in ctx.Plugin.textauras where sx.Value.ctx.Trigger == ctx.Trigger select sx.Key);
                            foreach (string rem in toRem)
                            {
                                AuraContainerForm acf = ctx.Plugin.textauras[rem];
                                acf.AuraDeactivate();
                            }
                        }
                    }
                    break;
            }
        }

        internal void RemoveAurasFromTrigger(Trigger t)
        {
            Context fakectx = new Context(t);
            ImageAuraManagement(fakectx, new ActionOverlayImage { Operation = ActionOverlayImage.OperationEnum.DeactivateTrigger });
            TextAuraManagement(fakectx, new ActionOverlayText { Operation = ActionOverlayText.OperationEnum.DeactivateTrigger });
        }
    }
}
