using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using Triggernometry.Expressions.String.Models;
using Triggernometry.Expressions.String.Utils;
using Triggernometry.PluginBridges.BridgeNamazu.Vfx;
using Triggernometry.Utilities.Maths;
using static System.Math;
using static Triggernometry.Expressions.String.Utils.DataStringHelper;

namespace Triggernometry.PluginBridges.BridgeNamazu.Modules
{
    /// <summary>
    /// 基于 VfxModule，仅用作参数解析和指令分发
    /// </summary>
    public class PictoACTModule : ModuleBase
    {
        public PictoACTModule()
        {
            ScanMethod = () => { };
        }

        static readonly Regex SplitMultiLineCmds = new Regex(@"(\r\n|\n|\r)\s*---\s*(?:\r\n|\n|\r)", RegexOptions.Compiled);

        private enum RemoveVfxType
        {
            Static,
            Actor,
            All,
        }

        private RemoveVfxType ParseRemoveType(StaticVfxArgs args)
        {
            var raw = args.Raw != null && args.Raw.TryGet("Type", out string type)
                ? type
                : null;

            switch (raw?.Trim().ToLowerInvariant())
            {
                case null:
                case "":
                case "all":
                    return RemoveVfxType.All;

                case "static":
                case "staticvfx":
                    return RemoveVfxType.Static;

                case "actor":
                case "actorvfx":
                    return RemoveVfxType.Actor;

                default:
                    throw new ArgumentException($"[PictoACT] Remove 动作指定的 VFX 类型 {raw} 无效。");
            }
        }

        [CallbackMethod("PictoACT")]
        public void CbPictoACT(string rawCommands)
        {
            if (GetConfig<bool>("ActorVfx") == false && GetConfig<bool>("StaticVfx") == false)
                return; // ignored

            // 用 --- 拆分指令，每部分单独执行
            foreach (var rawCommand in SplitMultiLineCmds.Split(rawCommands).Where(s => !string.IsNullOrWhiteSpace(s)))
            {
                try
                {
                    var data = new MultiLineRawArgs(rawCommand.Trim());
                    ExecuteWithDelayControl(data);
                }
                catch (Exception ex)
                {
                    ErrorLog($"[PictoACT] 执行绘制指令时出现错误：\n{ex}\n\n原始指令：\n{rawCommand}");
                }
            }
        }

        private void ExecuteWithDelayControl(MultiLineRawArgs data)
        {
            var args = StaticVfxArgsParser.Parse(data);

            // 如果显式指定了 delay 就进队列异步执行
            if (args.Delay.HasValue)
            {
                var tag = args.CreateTag;
                var delay = args.Delay.Value;
                if (delay < 0) 
                    delay = 0;

                VfxManager.ScheduleDelayedAction(tag, delay, () => SafeExecute(args));
                return;
            }

            // 没指定 delay 时同步执行
            SafeExecute(args);
        }

        private void SafeExecute(StaticVfxArgs args)
        {
            try
            {
                if (args == null)
                    throw new ArgumentNullException(nameof(args));

                // todo 应该放进 staticvfxargs
                // 尚未实现 shouldLog
                bool shouldLog = args.Raw?.TryGet("Log", out string rawLog) == true && rawLog.ParseData<bool>(); // default false

                switch (args.Action)
                {
                    case StaticVfxAction.Create:
                        CreateStaticVfx(args, shouldLog);
                        break;

                    case StaticVfxAction.Modify:
                        ModifyStaticVfxs(args, shouldLog);
                        break;

                    case StaticVfxAction.Remove:
                        RemoveVfxs(args, shouldLog); 
                        break;

                    case StaticVfxAction.Triangulate:
                        IsoscelesTriangulate(args, shouldLog);
                        break;

                    case StaticVfxAction.ExaFlare:
                        ExaFlare(args, shouldLog);
                        break;

                    default:
                        throw new ArgumentException($"[PictoACT] 未知的 Action：{args.Action}");
                }
            }
            catch (Exception ex)
            {
                ErrorLog($"[PictoACT] 执行绘制指令时出现错误：\n{ex}\n\n指令内容：\n{args?.Raw}");
            }
        }

        private StaticVfx CreateStaticVfx(StaticVfxArgs args, bool shouldLog)
        {
            if (GetConfig<bool>("StaticVfx") == false)
                return null;

            if (string.IsNullOrWhiteSpace(args.VfxPath))
                throw new ArgumentException("[PictoACT] Create 动作必须指定 Omen 或 StaticVfx。");

            var vfx = VfxManager.InitStatic(args.VfxPath, args.CreateTag, _vfx =>
            {
                // 设置 vfx 参数并更新
                ApplyStaticVfxArgs(_vfx, args, isCreate: true);
            });

            // 如果提供了时间参数，则写入移除时间
            if (args.Time.HasValue)
            {
                vfx.ScheduleRemove(args.Time.Value);
            }

            return vfx;
        }

        private void ModifyStaticVfxs(StaticVfxArgs args, bool shouldLog)
        {
            if (GetConfig<bool>("StaticVfx") == false)
                return;

            var filter = args.TagFilter();

            var vfxs = VfxManager.StaticVfxs.Values
                .Where(vfx => filter(vfx.Tag))
                .ToList();

            foreach (var vfx in vfxs)
            {
                ApplyStaticVfxArgs(vfx, args, isCreate: false);
            }
        }

        private void RemoveVfxs(StaticVfxArgs args, bool shouldLog)
        {
            var tagFilter = args.TagFilter();
            var type = ParseRemoveType(args);

            // 只在移除 Static / All 时中断延迟任务。
            var isStatic = type == RemoveVfxType.Static || type == RemoveVfxType.All;
            var isActor = type == RemoveVfxType.Actor || type == RemoveVfxType.All;

            if (isStatic && GetConfig<bool>("StaticVfx") != false)
            {
                // 先中断匹配 Tag / Regex 的延迟任务。
                // 注：由于旧版回调语法以及代码遗留原因，当前移除不区分 Static/Actor。
                // 理论上 Type: Static 会取消同 Tag 的延迟 Actor Remove，
                // 但 PictoACT 只保留 Actor Remove，且延迟 Actor Remove 基本不用，暂不拆分任务类型。
                VfxManager.CancelDelayedActions(tagFilter);

                // 然后中断正在运行的 Vfx
                VfxManager.StaticVfxs.Values
                    .Where(vfx => tagFilter(vfx.Tag))
                    .ToList()
                    .ForEach(vfx => vfx.TryRemove());
            }

            if (isActor && GetConfig<bool>("ActorVfx") != false)
            {
                VfxManager.ActorVfxs.Values
                    .Where(vfx => tagFilter(vfx.Tag))
                    .ToList()
                    .ForEach(vfx => vfx.TryRemove());
            }
        }

        private void ApplyStaticVfxArgs(StaticVfx vfx, StaticVfxArgs args, bool isCreate)
        {
            if (vfx == null)
                return;

            // PictoACT 只负责把解析后的参数更新到 StaticVfx 当前状态上
            vfx.ApplyArgs(args, isCreate);

            if (isCreate)
            {
                // Create 阶段发生在 Run 前，需要把初始参数解析并写入 native 字段
                VfxManager.ApplyResolvedStaticState(vfx, null);
            }
            else
            {
                // Modify 阶段不在这里直接 Refresh/Update，交给 VFX 循环统一处理
                vfx.PendingUpdate = true;
            }
        }

        // to-do 重构
        private void IsoscelesTriangulate(StaticVfxArgs args, bool shouldLog)
        {
            if (GetConfig<bool>("StaticVfx") == false) return; // ignored

            var raw = args.Raw ?? throw new ArgumentException("[PictoACT] Triangulate 缺少原始参数。");

            var t = args.Time ?? 0.0;
            string tag = args.CreateTag;

            // 等腰直角三角形 Omen，顶点位于直角，朝向直角开口方向，斜边长 1
            var vfxPath = "vfx/omen/eff/x6d3_b2_triangle90_p1.avfx";

            // 解析顶点坐标，格式为 "x1, y1; x2, y2; ..."，并三角剖分
            var points = raw.Get("Points").Split(';')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.ParseArgs<float, float>())
                .Select(tuple => new Vector2(tuple.Item1, tuple.Item2));

            var isoscelesTriangles = new Polygon(points).Triangulate()
                .SelectMany(triangle => triangle.IsoscelesTriangulate());

            // 对每个剖分出的等腰三角形创建 vfx
            foreach (var tri in isoscelesTriangles)
            {
                var vfx = VfxManager.InitStatic(vfxPath, tag, v =>
                {
                    var triArgs = new StaticVfxArgs
                    {
                        Pos = DynamicCoordArg.FromCoord(tri.Center),
                        Angle3D = new Vector3(tri.θ, 0, 0),
                        TransformCenter = args.TransformCenter?.Duplicate(),
                        TransformNorthAngle = args.TransformNorthAngle,
                        TransformNorthTarget = args.TransformNorthTarget?.Duplicate(),
                        TransformKeepX = args.TransformKeepX,
                        TransformKeepY = args.TransformKeepY,
                        Color = args.Color
                    };

                    v.ApplyArgs(triArgs, isCreate: true);
                    VfxManager.ApplyResolvedStaticState(v, null);

                    v.Scales = new Vector3(tri.ScaleX, tri.ScaleY, 1f) * 1.414f;
                });

                // 如果提供了时间参数，则安排移除
                if (t > 0)
                    vfx.ScheduleRemove(t);
            }
        }

        // to-do 重构
        private void ExaFlare(StaticVfxArgs args, bool shouldLog)
        {
            if (GetConfig<bool>("StaticVfx") == false) return; // ignored

            var data = args.Raw
                ?? throw new ArgumentException("[PictoACT] ExaFlare 缺少原始参数。");

            if (string.IsNullOrWhiteSpace(args.VfxPath))
                throw new ArgumentException("[PictoACT] 地火模式必须设置 Omen 或 StaticVfx。");

            // 地火特有的参数
            var n = data.Get("n", "count").ParseData<int>();
            var dPos = data.TryGet("dpos", out string dPosRaw) ? XIVCoord.ParseRawData(dPosRaw) : null;
            var dθ = data.TryGet(out string dθRaw, "dθ", "dTheta") ? dθRaw.ParseData<double>() : (double?)null;
            var dt = data.Get("dt").ParseData<float>();
            var n0 = data.TryGet("n0", out string rawN0) ? rawN0.ParseData<float>() : 0; // 初始地火在给定位置 = 0，在给定位置的下一个 = 1，...

            // 地火需要修改、检查的参数
            var delay0 = data.TryGet("Delay0", out string rawDelay) ? rawDelay.ParseData<float>() : 0f;
            var t = args.Time.HasValue ? (float)args.Time.Value : data.Get("Time", "t").ParseData<float>();

            // 这里只处理固定 Pos；如果 Pos 是实体 id，后续应交给 StaticVfx.Refresh 的动态解析逻辑处理
            XIVCoord pos0 = null;
            if (args.Pos != null)
            {
                if (!args.Pos.IsFixed)
                    throw new NotImplementedException($"[PictoACT] ExaFlare 不支持实体坐标作为初始位置：0x{args.Pos.EntityId:X8}");

                pos0 = args.Pos.FixedCoord.Duplicate();
            }
            pos0 = pos0 ?? new CartesianCoord(0, 0, 0);

            var θ0 = args.TransformNorthAngle ?? -PI;

            for (var step = 0; step < n; step++)
            {
                var newData = new MultiLineRawArgs(data);
                newData.Set("Action", "Create");

                // 修改时间
                var newDelay = delay0 + step * dt;
                if (newDelay < 0)
                {
                    // 第一个可能产生负延迟，直接将时间点提前
                    var newT = t + newDelay;
                    newDelay = 0;
                    newData.Set("time", newT);
                    newData.Set("t", newT);
                }
                newData.Set("delay", newDelay);

                // 修改相对位置
                if (dPos != null)
                {
                    var newPos = pos0 + (n0 + step) * dPos;
                    newData.Set("Pos", ((Vector3)newPos).ToDataString());
                }

                // 修改相对角度
                if (dθ != null)
                {
                    var newθ = θ0 + (n0 + step) * dθ.Value;
                    newData.Set("θ", newθ.ToDataString());
                    newData.Set("theta", newθ.ToDataString());
                }

                ExecuteWithDelayControl(newData);
            }
        }
    }
}