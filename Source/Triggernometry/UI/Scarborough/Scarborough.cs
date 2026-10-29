using Scarborough.Drawing;
using Scarborough.PInvoke;
using Scarborough.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Triggernometry.Core;

namespace Scarborough
{

    class Scarborough : IDisposable
    {

        public class ItemAction
        {

            public enum ActionTypeEnum
            {
                Activate,
                Deactivate,
                DeactivateAll,
                RenderingOn,
                RenderingOff,
                DeactivateRegex,
                DeactivateTrigger
            }

            public enum ItemTypeEnum
            {
                Image,
                Text
            }

            public ActionTypeEnum Action { get; set; }
            public ScarboroughItem Item { get; set; }
            public ItemTypeEnum ItemType { get; set; }
            public ManualResetEvent Completed { get; set; } = null;
            public Action CompletedAction { get; set; }
            public string Id { get; set; }

        }

        private Int64 CurOrdinal = 1;
        private bool renderOrderChanged = true;
        private ManualResetEvent exitEvent = null;
        private Thread drawThread = null;
        internal bool RenderingActive { get; set; }
        internal Triggernometry.Core.RealPlugin plug { get; set; }
        private Queue<ItemAction> ItemActions { get; set; } = new Queue<ItemAction>();

        // UI name completion snapshots these collections under their mutation locks.
        public Dictionary<string, ScarboroughImage> imageitems = new Dictionary<string, ScarboroughImage>();
        public Dictionary<string, ScarboroughText> textitems = new Dictionary<string, ScarboroughText>();

        public Scarborough()
        {
            User32.InitializeWindowClass();
            RenderingActive = true;
            exitEvent = new ManualResetEvent(false);
            drawThread = new Thread(Render);
            drawThread.Start();
        }

        public void Dispose()
        {
            Shutdown();
            if (exitEvent != null)
            {
                exitEvent.Dispose();
                exitEvent = null;
            }
        }

        public void Shutdown()
        {
            if (exitEvent != null)
            {
                exitEvent.Set();
            }
            if (drawThread != null)
            {
                drawThread.Join();
            }
        }

        /// <summary>按本轮开始时的队列长度处理命令，耗时操作不占用提交锁。</summary>
        private void ExecuteActions()
        {
            int count;
            lock (ItemActions) count = ItemActions.Count;
            for (int i = 0; i < count; i++)
            {
                ItemAction action;
                lock (ItemActions) action = ItemActions.Dequeue();
                try
                {
                    ExecuteAction(action);
                }
                catch (Exception ex)
                {
                    LogActionException(action, ex);
                }
                finally
                {
                    action.Completed?.Set();
                    action.CompletedAction?.Invoke();
                }
            }
        }

        /// <summary>记录渲染线程执行命令时的异常。</summary>
        private void LogActionException(ItemAction action, Exception ex)
        {
            plug?.FilteredAddToLog(RealPlugin.DebugLevelEnum.Error,
                Triggernometry.Localization.I18n.Translate("internal/Scarborough/actionerror",
                    "Aura command '{0}' for '{1}' failed: {2}", action.Action, action.Id, ex.ToString()), action.Item?.ctx?.Trigger);
        }

        /// <summary>退出绘图线程时取出并释放全部活动项。</summary>
        private void DisposeItems<T>(Dictionary<string, T> items) where T : ScarboroughItem
        {
            T[] activeItems;
            lock (items)
            {
                activeItems = items.Values.ToArray();
                items.Clear();
            }
            foreach (T item in activeItems)
            {
                try { item.Dispose(); }
                catch (Exception ex)
                {
                    LogActionException(new ItemAction { Action = ItemAction.ActionTypeEnum.Deactivate, Id = item.Name, Item = item }, ex);
                }
            }
        }

        private void ExecuteAction(ItemAction ia)
        {
            // 只有集合或前后顺序可能变化的命令才使渲染顺序失效；失败的命令也可能已完成部分修改。
            if (ia.Action != ItemAction.ActionTypeEnum.RenderingOn && ia.Action != ItemAction.ActionTypeEnum.RenderingOff)
                renderOrderChanged = true;
            switch (ia.Action)
            {
                case ItemAction.ActionTypeEnum.RenderingOn:
                    RenderingActive = true;
                    break;
                case ItemAction.ActionTypeEnum.RenderingOff:
                    RenderingActive = false;
                    break;
                case ItemAction.ActionTypeEnum.Activate:
                    {
                        if (ia.Item is ScarboroughImage)
                        {
                            ia.Item.Name = ia.Id;
                            ActivateImage(ia.Id, (ScarboroughImage)ia.Item);
                        }
                        else if (ia.Item is ScarboroughText)
                        {
                            ia.Item.Name = ia.Id;
                            ActivateText(ia.Id, (ScarboroughText)ia.Item);
                        }
                    }
                    break;
                case ItemAction.ActionTypeEnum.DeactivateRegex:
                    {
                        Regex rex = new Regex(ia.Id);
                        List<string> toRem = new List<string>();
                        switch (ia.ItemType)
                        {
                            case ItemAction.ItemTypeEnum.Image:
                                {
                                    toRem.AddRange(from sx in imageitems where rex.IsMatch(sx.Key) select sx.Key);
                                    foreach (string rem in toRem)
                                    {
                                        ScarboroughImage si = null;
                                        si = imageitems[rem];
                                        lock (imageitems) imageitems.Remove(rem);
                                        if (si != null)
                                        {
                                            si.Dispose();
                                        }
                                    }
                                }
                                break;
                            case ItemAction.ItemTypeEnum.Text:
                                {
                                    toRem.AddRange(from sx in textitems where rex.IsMatch(sx.Key) select sx.Key);
                                    foreach (string rem in toRem)
                                    {
                                        ScarboroughText si = null;
                                        si = textitems[rem];
                                        lock (textitems) textitems.Remove(rem);
                                        if (si != null)
                                        {
                                            si.Dispose();
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    break;
                case ItemAction.ActionTypeEnum.DeactivateTrigger:
                    {
                        List<string> toRem = new List<string>();
                        switch (ia.ItemType)
                        {
                            case ItemAction.ItemTypeEnum.Image:
                                {
                                    // 脚本创建的悬浮窗可能没有触发器归属，按触发器清理时跳过。
                                    toRem.AddRange(from sx in imageitems where sx.Value.ctx?.Trigger?.Id.ToString() == ia.Id select sx.Key);
                                    foreach (string rem in toRem)
                                    {
                                        ScarboroughImage si = null;
                                        si = imageitems[rem];
                                        lock (imageitems) imageitems.Remove(rem);
                                        si?.Dispose();
                                    }
                                }
                                break;
                            case ItemAction.ItemTypeEnum.Text:
                                {
                                    // 脚本创建的悬浮窗可能没有触发器归属，按触发器清理时跳过。
                                    toRem.AddRange(from sx in textitems where sx.Value.ctx?.Trigger?.Id.ToString() == ia.Id select sx.Key);
                                    foreach (string rem in toRem)
                                    {
                                        ScarboroughText si = null;
                                        si = textitems[rem];
                                        lock (textitems) textitems.Remove(rem);
                                        si?.Dispose();
                                    }
                                }
                                break;
                        }
                    }
                    break;
                case ItemAction.ActionTypeEnum.Deactivate:
                    {
                        switch (ia.ItemType)
                        {
                            case ItemAction.ItemTypeEnum.Image:
                                {
                                    ScarboroughImage si = null;
                                    if (imageitems.ContainsKey(ia.Id) == true)
                                    {
                                        si = imageitems[ia.Id];
                                        lock (imageitems) imageitems.Remove(ia.Id);
                                    }
                                    if (si != null)
                                    {
                                        si.Dispose();
                                    }
                                }
                                break;
                            case ItemAction.ItemTypeEnum.Text:
                                {
                                    ScarboroughText si = null;
                                    if (textitems.ContainsKey(ia.Id) == true)
                                    {
                                        si = textitems[ia.Id];
                                        lock (textitems) textitems.Remove(ia.Id);
                                    }
                                    if (si != null)
                                    {
                                        si.Dispose();
                                    }
                                }
                                break;
                        }
                    }
                    break;
                case ItemAction.ActionTypeEnum.DeactivateAll:
                    {
                        switch (ia.ItemType)
                        {
                            case ItemAction.ItemTypeEnum.Image:
                                {
                                    List<ScarboroughImage> toRem = new List<ScarboroughImage>();
                                    foreach (KeyValuePair<string, ScarboroughImage> si in imageitems)
                                    {
                                        toRem.Add(si.Value);
                                    }
                                    lock (imageitems) imageitems.Clear();
                                    foreach (ScarboroughImage si in toRem)
                                    {
                                        si.Dispose();
                                    }
                                }
                                break;
                            case ItemAction.ItemTypeEnum.Text:
                                {
                                    List<ScarboroughText> toRem = new List<ScarboroughText>();
                                    foreach (KeyValuePair<string, ScarboroughText> si in textitems)
                                    {
                                        toRem.Add(si.Value);
                                    }
                                    lock (textitems) textitems.Clear();
                                    foreach (ScarboroughText si in toRem)
                                    {
                                        si.Dispose();
                                    }
                                }
                                break;

                        }
                    }
                    break;
            }
        }

        public void Activate(string id, ScarboroughItem si)
        {
            lock (ItemActions)
            {
                ItemActions.Enqueue(new ItemAction() { Action = ItemAction.ActionTypeEnum.Activate, Id = id, Item = si });
            }
        }

        public void Deactivate(string id, ItemAction.ItemTypeEnum it, Action completed = null)
        {
            lock (ItemActions)
            {
                ItemActions.Enqueue(new ItemAction() { Action = ItemAction.ActionTypeEnum.Deactivate, Id = id, ItemType = it, CompletedAction = completed });
            }
        }

        public void DeactivateRegex(string rex, ItemAction.ItemTypeEnum it)
        {
            lock (ItemActions)
            {
                ItemActions.Enqueue(new ItemAction() { Action = ItemAction.ActionTypeEnum.DeactivateRegex, Id = rex, ItemType = it });
            }
        }

        public void DeactivateTrigger(Trigger t, ItemAction.ItemTypeEnum it)
        {
            lock (ItemActions)
            {
                ItemActions.Enqueue(new ItemAction() { Action = ItemAction.ActionTypeEnum.DeactivateTrigger, Id = t.Id.ToString(), ItemType = it });
            }
        }

        private Int64 GetNextOrdinal()
        {
            return Interlocked.Increment(ref CurOrdinal);
        }

        private void ActivateImage(string id, ScarboroughImage si)
        {
            ScarboroughImage existing = GetImage(id);
            if (existing != null)
            {
                existing.Ordinal = GetNextOrdinal();
                existing.Left = si.EvaluateNumericExpression(si.ctx, si.InitXExpression);
                existing.Top = si.EvaluateNumericExpression(si.ctx, si.InitYExpression);
                existing.Width = si.EvaluateNumericExpression(si.ctx, si.InitWExpression);
                existing.Height = si.EvaluateNumericExpression(si.ctx, si.InitHExpression);
                existing.Opacity = string.IsNullOrWhiteSpace(si.InitOExpression) ? 100 : si.EvaluateNumericExpression(si.ctx, si.InitOExpression);
                existing.UpdateXExpression = si.UpdateXExpression;
                existing.UpdateYExpression = si.UpdateYExpression;
                existing.UpdateWExpression = si.UpdateWExpression;
                existing.UpdateHExpression = si.UpdateHExpression;
                existing.UpdateOExpression = si.UpdateOExpression;
                existing.TTLExpression = si.TTLExpression;
                existing.Display = si.Display;
                existing.ctx = si.ctx;
                si.ImageFilename = si.EvaluateStringExpression(si.ctx, si.ImageExpression);
                if (existing.ImageFilename != si.ImageFilename)
                {
                    existing.ImageFilename = si.ImageFilename;
                    existing.NeedImage = true;
                }
            }
            else
            {
                si.Ordinal = GetNextOrdinal();
                si.NeedImage = true;
                si.plug = plug;
                si.ImageFilename = si.EvaluateStringExpression(si.ctx, si.ImageExpression);
                si.Left = si.EvaluateNumericExpression(si.ctx, si.InitXExpression);
                si.Top = si.EvaluateNumericExpression(si.ctx, si.InitYExpression);
                si.Width = si.EvaluateNumericExpression(si.ctx, si.InitWExpression);
                si.Height = si.EvaluateNumericExpression(si.ctx, si.InitHExpression);
                si.Opacity = string.IsNullOrWhiteSpace(si.InitOExpression) ? 100 : si.EvaluateNumericExpression(si.ctx, si.InitOExpression);
                lock (imageitems) imageitems[id] = si;
            }
        }

        private void ActivateText(string id, ScarboroughText si)
        {
            ScarboroughText existing = GetText(id);
            if (existing != null)
            {
                existing.Ordinal = GetNextOrdinal();
                existing.Left = si.EvaluateNumericExpression(si.ctx, si.InitXExpression);
                existing.Top = si.EvaluateNumericExpression(si.ctx, si.InitYExpression);
                existing.Width = si.EvaluateNumericExpression(si.ctx, si.InitWExpression);
                existing.Height = si.EvaluateNumericExpression(si.ctx, si.InitHExpression);
                existing.Opacity = string.IsNullOrWhiteSpace(si.InitOExpression) ? 100 : si.EvaluateNumericExpression(si.ctx, si.InitOExpression);
                existing.UpdateXExpression = si.UpdateXExpression;
                existing.UpdateYExpression = si.UpdateYExpression;
                existing.UpdateWExpression = si.UpdateWExpression;
                existing.UpdateHExpression = si.UpdateHExpression;
                existing.UpdateOExpression = si.UpdateOExpression;
                existing.TTLExpression = si.TTLExpression;
                existing.TextAlignment = si.TextAlignment;
                existing.UseOutline = si.UseOutline;
                existing.TextColor = si.TextColor;
                existing.OutlineColor = si.OutlineColor;
                existing.BackgroundColor = si.BackgroundColor;
                existing.NeedFont |= existing.FontName != si.FontName ||
                                     existing.FontSize != si.FontSize || existing.FontStyle != si.FontStyle;
                existing.FontName = si.FontName;
                existing.FontSize = si.FontSize;
                existing.FontStyle = si.FontStyle;
                existing.Text = si.EvaluateStringExpression(si.ctx, si.TextExpression);
                existing.TextExpression = si.TextExpression;
                existing.ctx = si.ctx;
            }
            else
            {
                si.Ordinal = GetNextOrdinal();
                si.plug = plug;
                si.Left = si.EvaluateNumericExpression(si.ctx, si.InitXExpression);
                si.Top = si.EvaluateNumericExpression(si.ctx, si.InitYExpression);
                si.Width = si.EvaluateNumericExpression(si.ctx, si.InitWExpression);
                si.Height = si.EvaluateNumericExpression(si.ctx, si.InitHExpression);
                si.Opacity = string.IsNullOrWhiteSpace(si.InitOExpression) ? 100 : si.EvaluateNumericExpression(si.ctx, si.InitOExpression);
                si.Text = si.EvaluateStringExpression(si.ctx, si.TextExpression);
                si.NeedFont = true;
                lock (textitems) textitems[id] = si;
            }
        }

        public ScarboroughImage GetImage(string id)
        {
            if (imageitems.ContainsKey(id) == true)
            {
                return imageitems[id];
            }
            return null;
        }

        public ScarboroughText GetText(string id)
        {
            if (textitems.ContainsKey(id) == true)
            {
                return textitems[id];
            }
            return null;
        }

        public void DeactivateAllImages()
        {
            lock (ItemActions)
            {
                ItemActions.Enqueue(new ItemAction() { Action = ItemAction.ActionTypeEnum.DeactivateAll, ItemType = ItemAction.ItemTypeEnum.Image });
            }
        }

        public void DeactivateAllText()
        {
            lock (ItemActions)
            {
                ItemActions.Enqueue(new ItemAction() { Action = ItemAction.ActionTypeEnum.DeactivateAll, ItemType = ItemAction.ItemTypeEnum.Text });
            }
        }

        public class DeferredMessage
        {

            public Context ctx { get; set; } = null;
            public Triggernometry.Core.RealPlugin plug { get; set; } = null;
            public Triggernometry.Core.RealPlugin.DebugLevelEnum level { get; set; } = Triggernometry.Core.RealPlugin.DebugLevelEnum.None;
            public string Message { get; set; } = "";

        }

        private void ProcessMessages(IEnumerable<DeferredMessage> msgs)
        {
            if (msgs == null) return;
            foreach (DeferredMessage msg in msgs)
            {
                if (msg.ctx != null)
                {
                    msg.ctx.Trigger.AddToLog(msg.level, msg.Message);
                }
                else
                {
                    plug.FilteredAddToLog(msg.level, msg.Message);
                }
            }
        }

        /// <summary>共用图片和文本的更新、删除及延迟日志流程。</summary>
        private void UpdateItems<T>(Dictionary<string, T> items, int numTicks) where T : ScarboroughItem
        {
            List<string> toRem = null;
            List<DeferredMessage> messages = null;
            foreach (KeyValuePair<string, T> si in items)
            {
                try
                {
                    if (si.Value.Logic(numTicks) == false)
                    {
                        (toRem ?? (toRem = new List<string>())).Add(si.Key);
                    }
                    else
                    {
                        if (si.Value.Changed == true)
                        {
                            si.Value.NeedRender = true;
                            si.Value.Changed = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (si.Value.ctx != null && si.Value.ctx.Trigger != null)
                    {
                        (messages ?? (messages = new List<DeferredMessage>())).Add(new DeferredMessage()
                        {
                            ctx = si.Value.ctx,
                            plug = plug,
                            level = Triggernometry.Core.RealPlugin.DebugLevelEnum.Error,
                            Message = Triggernometry.Localization.I18n.Translate("internal/AuraContainer/updateerrortrigger", "Deactivating aura '{0}' from trigger '{1}' due to update exception: {2}", si.Key, si.Value.ctx.Trigger.LogName, ex.Message)
                        }
                        );
                    }
                    else
                    {
                        (messages ?? (messages = new List<DeferredMessage>())).Add(new DeferredMessage()
                        {
                            ctx = null,
                            plug = plug,
                            level = Triggernometry.Core.RealPlugin.DebugLevelEnum.Error,
                            Message = Triggernometry.Localization.I18n.Translate("internal/AuraContainer/updateerror", "Deactivating aura '{0}' due to update exception: {1}", si.Key, ex.Message)
                        }
                        );
                    }
                    (toRem ?? (toRem = new List<string>())).Add(si.Key);
                }
            }
            if (toRem != null)
            {
                renderOrderChanged = true;
                foreach (string si in toRem)
                {
                    T sit = items[si];
                    if (sit.ctx != null && sit.ctx.Trigger != null)
                    {
                        (messages ?? (messages = new List<DeferredMessage>())).Add(new DeferredMessage()
                        {
                            ctx = sit.ctx,
                            plug = plug,
                            level = Triggernometry.Core.RealPlugin.DebugLevelEnum.Verbose,
                            Message = Triggernometry.Localization.I18n.Translate("internal/AuraContainer/closingaura", "Closing aura window")
                        }
                        );
                    }
                    else
                    {
                        (messages ?? (messages = new List<DeferredMessage>())).Add(new DeferredMessage()
                        {
                            ctx = null,
                            plug = plug,
                            level = Triggernometry.Core.RealPlugin.DebugLevelEnum.Verbose,
                            Message = Triggernometry.Localization.I18n.Translate("internal/AuraContainer/closingaura", "Closing aura window")
                        }
                        );
                    }
                    lock (items) items.Remove(si);
                    sit.Dispose();
                }
            }
            ProcessMessages(messages);
        }

        internal void HideAllItems()
        {
            lock (ItemActions)
            {
                ItemActions.Enqueue(new ItemAction() { Action = ItemAction.ActionTypeEnum.RenderingOff });
            }
        }

        internal void ShowAllItems()
        {
            lock (ItemActions)
            {
                ItemActions.Enqueue(new ItemAction() { Action = ItemAction.ActionTypeEnum.RenderingOn });
            }
        }

        private void Render(List<ScarboroughItem> items)
        {
            List<ScarboroughItem> toRem = null;
            List<DeferredMessage> messages = null;
            foreach (ScarboroughItem si in items)
            {
                try
                {
                    si.Render();
                }
                catch (Exception ex)
                {
                    if (si.ctx != null && si.ctx.Trigger != null)
                    {
                        (messages ?? (messages = new List<DeferredMessage>())).Add(new DeferredMessage()
                        {
                            ctx = si.ctx,
                            plug = plug,
                            level = Triggernometry.Core.RealPlugin.DebugLevelEnum.Error,
                            Message = Triggernometry.Localization.I18n.Translate("internal/AuraContainer/updateerrortrigger", "Deactivating aura '{0}' from trigger '{1}' due to update exception: {2}", si.Name, si.ctx.Trigger.LogName, ex.Message)
                        }
                        );
                    }
                    else
                    {
                        (messages ?? (messages = new List<DeferredMessage>())).Add(new DeferredMessage()
                        {
                            ctx = null,
                            plug = plug,
                            level = Triggernometry.Core.RealPlugin.DebugLevelEnum.Error,
                            Message = Triggernometry.Localization.I18n.Translate("internal/AuraContainer/updateerror", "Deactivating aura '{0}' due to update exception: {1}", si.Name, ex.Message)
                        }
                        );
                    }
                    (toRem ?? (toRem = new List<ScarboroughItem>())).Add(si);
                }
            }
            if (toRem != null)
            {
                renderOrderChanged = true;
                foreach (ScarboroughItem si in toRem)
                {
                    if (si.ctx != null && si.ctx.Trigger != null)
                    {
                        (messages ?? (messages = new List<DeferredMessage>())).Add(new DeferredMessage()
                        {
                            ctx = si.ctx,
                            plug = plug,
                            level = Triggernometry.Core.RealPlugin.DebugLevelEnum.Verbose,
                            Message = Triggernometry.Localization.I18n.Translate("internal/AuraContainer/closingaura", "Closing aura window")
                        }
                        );
                    }
                    else
                    {
                        (messages ?? (messages = new List<DeferredMessage>())).Add(new DeferredMessage()
                        {
                            ctx = null,
                            plug = plug,
                            level = Triggernometry.Core.RealPlugin.DebugLevelEnum.Verbose,
                            Message = Triggernometry.Localization.I18n.Translate("internal/AuraContainer/closingaura", "Closing aura window")
                        }
                        );
                    }
                    if (si is ScarboroughImage)
                    {
                        lock (imageitems)
                            if (imageitems.TryGetValue(si.Name, out var current) && ReferenceEquals(current, si))
                                imageitems.Remove(si.Name);
                    }
                    if (si is ScarboroughText)
                    {
                        lock (textitems)
                            if (textitems.TryGetValue(si.Name, out var current) && ReferenceEquals(current, si))
                                textitems.Remove(si.Name);
                    }
                    // 本轮之后仍可能继续渲染，不能保留已经释放的项目。
                    items.Remove(si);
                    si.Dispose();
                }
            }
            ProcessMessages(messages);
        }

        /// <summary>集合或激活顺序变化时重建渲染列表，普通更新继续复用原顺序。</summary>
        private void UpdateRenderOrder(List<ScarboroughItem> items)
        {
            if (!renderOrderChanged) return;
            items.Clear();
            items.AddRange(imageitems.Values);
            items.AddRange(textitems.Values);
            items.Sort((a, b) => a.Ordinal.CompareTo(b.Ordinal));
            renderOrderChanged = false;
        }

        public void Render()
        {
            int numTicks = 0;
            long prevTick = Stopwatch.GetTimestamp(), tickTime;
            double msSince, lag = 0.0;
            var items = new List<ScarboroughItem>();
            try
            {
                while (true)
                {
                    try
                    {
                        if (exitEvent.WaitOne(5) == true)
                        {
                            return;
                        }
                        tickTime = Stopwatch.GetTimestamp();
                        msSince = (tickTime - prevTick) * 1000.0 / Stopwatch.Frequency + lag;
                        numTicks = (int)Math.Floor(msSince / 20.0);
                        lag = msSince - (numTicks * 20);
                        prevTick = tickTime;
                        if (numTicks > 0)
                        {
                            ExecuteActions();
                            UpdateItems(imageitems, numTicks);
                            UpdateItems(textitems, numTicks);
                        }
                        // 更新或释放失败后也会在下一轮重新读取集合，避免继续使用过期的列表。
                        UpdateRenderOrder(items);
                        if (items.Count > 0)
                        {
                            Render(items);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            finally
            {
                DisposeItems(imageitems);
                DisposeItems(textitems);
            }
        }

    }

}
