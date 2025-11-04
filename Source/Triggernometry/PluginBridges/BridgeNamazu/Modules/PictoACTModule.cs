using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Triggernometry;
using Triggernometry.PluginBridges.BridgeNamazu.Vfx;
using Triggernometry.Utilities;
using Triggernometry.Utilities.Math;
using static System.Math;
using static Triggernometry.Utilities.DataStringHelper;

namespace Triggernometry.PluginBridges.BridgeNamazu.Modules
{
    /// <summary>
    /// 基于 VfxModule
    /// </summary>
    public class PictoACTModule : ModuleBase
    {
        public PictoACTModule()
        {
            ScanMethod = () => { };
        }

        static readonly Regex SplitMultiLineCmds = new Regex(@"(\r\n|\n|\r)\s*---\s*(?:\r\n|\n|\r)", RegexOptions.Compiled);

        // 管理延迟创建阶段的任务（只到 Execute 前）
        private static readonly Dictionary<string, List<CancellationTokenSource>> _delayTasks =
            new Dictionary<string, List<CancellationTokenSource>>(StringComparer.OrdinalIgnoreCase);
        private static readonly object _delayLock = new object();


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

        private void ExecuteWithDelayControl(MultiLineRawArgs data, Action<StaticVfx> createModifier = null)
        {
            // 判断是否需要延迟执行
            double delay;
            if (data.TryGet("Delay", out string rawDelay) &&
                (delay = rawDelay.FromDataString<double>()) > 0)
            {
                string tag = ParseTag(data);
                var cts = new CancellationTokenSource();

                // 注册任务
                lock (_delayLock)
                {
                    if (!_delayTasks.TryGetValue(tag, out var list))
                    {
                        list = new List<CancellationTokenSource>();
                        _delayTasks[tag] = list;
                    }
                    list.Add(cts);
                }

                Task.Run(async () =>
                {
                    try
                    {
                        await Task.Delay(TimeSpan.FromSeconds(delay), cts.Token).ConfigureAwait(false);
                        if (cts.IsCancellationRequested) return;
                        Execute(data, createModifier);
                    }
                    catch (OperationCanceledException)
                    {
                        // 被主动取消，不记录日志
                    }
                    catch (Exception ex)
                    {
                        ErrorLog($"[PictoACT] 执行绘制指令时出现错误：\n{ex}\n\n指令内容：\n{data}");
                    }
                    finally
                    {
                        // 清理已完成任务
                        lock (_delayLock)
                        {
                            if (_delayTasks.TryGetValue(tag, out var list))
                            {
                                list.Remove(cts);
                                if (list.Count == 0)
                                    _delayTasks.Remove(tag);
                            }
                        }
                    }
                });
            }
            else
            {
                Execute(data, createModifier);
            }
        }

        private static readonly Dictionary<VfxType, string> _actorCommandTemplates = new Dictionary<VfxType, string>
        {
            { VfxType.LockOn,       "vfx/lockon/eff/{0}.avfx" },
            { VfxType.Channeling,   "vfx/channeling/eff/{0}.avfx" },
            { VfxType.CastVfx,      "vfx/common/eff/{0}.avfx" },
            //{ VfxType.StatusLoopVfx, "" },
            { VfxType.ActorVfx,     "{0}" },
        };

        private static readonly Dictionary<VfxType, string> _staticCommandTemplates = new Dictionary<VfxType, string>
        {
            { VfxType.Omen, "vfx/omen/eff/{0}.avfx" },
            { VfxType.StaticVfx, "{0}" },
        };

        private void Execute(MultiLineRawArgs data, Action<StaticVfx> createModifier = null)
        {
            // 提取共通参数
            string action = data.TryGet("Action", out action) ? action.Trim() : null;
            bool shouldLog = data.TryGet("Log", out string rawLog) && rawLog.FromDataString<bool>(); // default false
            switch (action?.ToUpper())
            {
                case "CREATE":
                case null:
                    CreateVfx(data, shouldLog, createModifier);
                    break;
                case "MODIFY":
                case "CHANGE":
                    ModifyVfxs(data, shouldLog);
                    break;
                case "REMOVE":
                    RemoveVfxs(data, shouldLog); // 尚未实现 shouldLog
                    break;
                case "TRIANGULATE":
                case "△": // u25B3 Triangle
                case "Δ": // u0394 Delta
                case "∆": // u2206 Increment
                    IsoscelesTriangulate(data, shouldLog);
                    break;
                case "EXAFLARE":
                case "地火":
                    ExaFlare(data, shouldLog);
                    break;
                default:
                    throw new ArgumentException($"[PictoACT] 未知的 Action：{action}");
            }
        }

        private Vfx.Vfx CreateVfx(MultiLineRawArgs data, bool shouldLog, Action<StaticVfx> createModifier = null)
        {
            ParseTypeAndPath(data, out VfxType vfxType, out string vfxPath, out bool isActor);
            if (isActor)
            {
                if (GetConfig<bool>("ActorVfx") == false) 
                    return null;
                return CreateActorVfx(data, vfxType, vfxPath, shouldLog);
            }
            
            // static
            if (GetConfig<bool>("StaticVfx") == false)
                return null;

            if (!_staticCommandTemplates.TryGetValue(vfxType, out _))
                throw new ArgumentException($"[PictoACT] 不支持的 VfxType: {vfxType}");

            return CreateStaticVfx(data, vfxType, vfxPath, shouldLog, createModifier);
        }

        private ActorVfx CreateActorVfx(MultiLineRawArgs data, VfxType vfxType, string vfxPath, bool shouldLog)
        {
            string tag = ParseTag(data);
            throw new NotImplementedException("[PictoACT] 此回调暂时不支持 Actor VFX。");
        }

        private StaticVfx CreateStaticVfx(MultiLineRawArgs data, VfxType vfxType, string vfxPath, bool shouldLog, Action<StaticVfx> createModifier = null)
        {
            string tag = ParseTag(data);
            // 创建并运行
            var vfx = StaticVfx.Create(vfxPath, tag);
            vfx.Run();

            // 设置 vfx 参数并更新

            // to-do：这里才设置初始几何参数，如果在创建和设置之间有其他动作异步 Modify 时调用初始参数就会出问题
            // vfx 加一个 ready 参数？
            var modifiers = ParseStaticVfxModifiers(data, true);
            foreach (var mod in modifiers)
            {
                mod(vfx);
            }

            // 如果提供了时间参数，则安排移除
            _ = data.TryGet(out string rawTime, "Time", "t");
            if (rawTime != null)
            {
                vfx.ScheduleRemove(rawTime.FromDataString<double>());
            }
            // 额外的修饰（目前用于延迟执行额外操作）
            if (createModifier != null)
            {
                createModifier(vfx);
            }
            return vfx;
        }

        private void IsoscelesTriangulate(MultiLineRawArgs data, bool shouldLog)
        {
            if (GetConfig<bool>("StaticVfx") == false) return; // ignored
            // 解析与三角形本身无关的参数
            TryParseRotation(data, out double? rotation);
            TryParseCenter(data, out XIVCoord center);
            TryParseFlip(data, out bool? keepX, out bool? keepY);
            var colorModifier = ColorModifier(data, true);
            bool shouldUpdate = !data.TryGet("Update", out var rawUpdate) || rawUpdate.FromDataString<bool>(); // default true
            var t = data.TryGet(out string rawTime, "Time", "t") ? rawTime.FromDataString<double>() : 0.0;
            string tag = ParseTag(data);

            // 等腰直角三角形 Omen，顶点位于直角，朝向直角开口方向，斜边长 1
            var vfxPath = "vfx/omen/eff/x6d3_b2_triangle90_p1.avfx";

            // 解析顶点坐标，格式为 "x1, y1; x2, y2; ..."，并三角剖分
            var points = data.Get("Points").Split(';')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => ParseArgs<float, float>(s))
                .Select(tuple => new Vector2(tuple.Item1, tuple.Item2));
            var isoscelesTriangles = new Polygon(points).Triangulate()
                .SelectMany(triangle => triangle.IsoscelesTriangulate());

            var vfxs = new Dictionary<StaticVfx, IsoscelesTriangle>();
            // 对每个剖分出的等腰三角形创建 vfx
            foreach (var tri in isoscelesTriangles)
            {
                var vfx = StaticVfx.Create(vfxPath, tag);
                vfx.Run();
                vfxs[vfx] = tri;
            }
            foreach (var pair in vfxs)
            {
                var vfx = pair.Key;
                var tri = pair.Value;
                var pos = tri.Center;
                var angles = new Vector3(tri.θ, 0, 0);
                var transformer = LinearTransformer(true, pos, angles, center, rotation, keepX, keepY);
                transformer(vfx);
                vfx.Scales = new Vector3(tri.ScaleX, tri.ScaleY, 1f) * 1.414f;
                colorModifier?.Invoke(vfx);

                if (shouldUpdate) vfx.Update();
                // 如果提供了时间参数，则安排移除
                if (t > 0) vfx.ScheduleRemove(t);
            }
        }

        private void ExaFlare(MultiLineRawArgs data, bool shouldLog)
        {
            if (GetConfig<bool>("StaticVfx") == false) return; // ignored

            // staticvfx 类型
            ParseTypeAndPath(data, out VfxType vfxType, out string vfxPath, out bool isActor);
            if (isActor)
                throw new ArgumentException("[PictoACT] 地火模式不能设置 ActorVfx");
            if (!_staticCommandTemplates.ContainsKey(vfxType))
                throw new ArgumentException($"[PictoACT] 不支持的 VfxType: {vfxType}");

            // 地火特有的参数
            var n = data.Get("n", "count").FromDataString<int>();
            var dPos = data.TryGet("dpos", out string dPosRaw) ? XIVCoord.ParseRawData(dPosRaw) : null;
            var dθ = data.TryGet(out string dθRaw, "dθ", "dTheta") ? dθRaw.FromDataString<double>() : (double?)null;
            var dt = data.Get("dt").FromDataString<float>();
            _ = data.TryGet("color2", out string color2);
            var colorDelay = data.TryGet("colorDelay", out string rawColorDelay) ? rawColorDelay.FromDataString<float>() : 0f;
            var n0 = data.TryGet("n0", out string rawN0) ? rawN0.FromDataString<float>() : 0; // 初始地火在给定位置 = 0，在给定位置的下一个 = 1，...

            // 地火需要修改、检查的参数
            var delay0 = data.TryGet("Delay0", out string rawDelay) ? rawDelay.FromDataString<float>() : 0f;
            var t = data.Get("Time", "t").FromDataString<float>();
            var pos0 = TryParsePos(data, out XIVCoord pos) ? pos : new CartesianCoord(0, 0, 0);
            var θ0 = TryParseRotation(data, out double? θ) ? θ : -PI;
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

                Action<StaticVfx> asyncColorModifier = null;

                // 构造延迟颜色修改逻辑
                if (color2 != null && colorDelay > 0)
                {
                    var newData2 = new MultiLineRawArgs(newData);
                    newData2["color"] = color2;
                    var colorModifier = ColorModifier(newData2, isCreate: false);

                    asyncColorModifier = vfx =>
                    {
                        Task.Run(async () =>
                        {
                            await Task.Delay(TimeSpan.FromSeconds(colorDelay)).ConfigureAwait(false);
                            try
                            {
                                colorModifier(vfx);
                                vfx.Update();
                            }
                            catch (Exception ex)
                            {
                                ErrorLog($"[PictoACT] 修改颜色时出错：\n{ex}");
                            }
                        });
                    };
                }

                ExecuteWithDelayControl(newData, asyncColorModifier);
            }
        }

        private void ModifyVfxs(MultiLineRawArgs data, bool shouldLog)
        {
            // 判断类型：必须为二者之一（参数不共通）
            _ = data.TryGet("Type", out string type);
            bool isActor;
            switch (type?.ToLower())
            {
                case "actor":
                case "actorvfx":
                    isActor = true;
                    break;
                case "static":
                case "staticvfx":
                case null: // 未指定时默认为 static
                    isActor = false;
                    break;
                default:
                    throw new ArgumentException($"[PictoACT] Modify 动作指定的 Vfx 类型 {type} 无效。");
            }
            // 提取过滤器
            var filter = ParseFilter(data);
            // 执行更新
            if (isActor)
            {
                if (GetConfig<bool>("ActorVfx") == false) return;
                var vfxs = ActorVfx.Storage.Values.Where(vfx => filter(vfx.Tag)).ToList();
                var modifiers = ParseActorVfxModifiers(data, false);
                foreach (var vfx in vfxs)
                {
                    foreach (var mod in modifiers)
                    {
                        mod(vfx);
                    }
                }
            }
            else
            {
                if (GetConfig<bool>("StaticVfx") == false) return;
                var vfxs = StaticVfx.Storage.Values.Where(vfx => filter(vfx.Tag)).ToList();
                var modifiers = ParseStaticVfxModifiers(data, false);
                foreach (var vfx in vfxs)
                {
                    foreach (var mod in modifiers)
                    {
                        mod(vfx);
                    }
                }
            }
        }

        private void RemoveVfxs(MultiLineRawArgs data, bool shouldLog)
        {
            // 先中断匹配 Tag / Regex 的延迟创建任务
            var tagFilter = ParseFilter(data);
            lock (_delayLock)
            {
                var matched = _delayTasks.Keys.Where(tagFilter).ToList();
                foreach (var key in matched)
                {
                    foreach (var cts in _delayTasks[key].ToList()) // ToList 防止修改集合
                    {
                        cts.Cancel();
                    }
                    _delayTasks.Remove(key);
                }
            }
            
            // 然后中断正在运行的 Vfx
            // 判断类型
            _ = data.TryGet("Type", out string type);
            bool isActor, isStatic;
            switch (type?.ToLower())
            {
                case "actor":
                case "actorvfx":
                    isActor = true;
                    isStatic = false;
                    break;
                case "static":
                case "staticvfx":
                    isActor = false;
                    isStatic = true;
                    break;
                case "all":
                case null: // 未指定时默认为 all
                    isActor = true;
                    isStatic = true;
                    break;
                default:
                    throw new ArgumentException($"[PictoACT] Remove 动作指定的 Vfx 类型 {type} 无效。");
            }
            // 提取过滤器
            var filter = ParseFilter(data);
            
            // 执行移除
            if (isActor && GetConfig<bool>("ActorVfx") != false)
                ActorVfx.Storage.Values.Where(vfx => filter(vfx.Tag)).ToList().ForEach(vfx => vfx.TryRemove());
            if (isStatic && GetConfig<bool>("StaticVfx") != false)
                StaticVfx.Storage.Values.Where(vfx => filter(vfx.Tag)).ToList().ForEach(vfx => vfx.TryRemove());
        }

        private void ParseTypeAndPath(MultiLineRawArgs data, out VfxType vfxType, out string vfxPath, out bool isActor)
        {
            // 查找 LockOn、Channeling、Omen 等关键词 key
            string vfxName = default;
            vfxType = default;
            foreach (var pair in data.Data)
            {
                if (Enum.TryParse(pair.Key, ignoreCase: true, out vfxType))
                {
                    vfxName = pair.Value;
                    break;
                }
            }
            _ = vfxName ?? throw new ArgumentException($"[PictoACT] 未指定有效的 Actor 或 Static VFX 类型：\n{data} ");

            // 确定类型属于 Actor 或 Static 并模板化路径
            if (_actorCommandTemplates.TryGetValue(vfxType, out string templateA))
            {
                vfxPath = string.Format(templateA, vfxName);
                isActor = true;
            }
            else if (_staticCommandTemplates.TryGetValue(vfxType, out string templateS))
            {
                // 对于 Omen 类型，允许使用 _omenAbbrevs 定义的简称，如 Rect、Circle、Fan90 等
                if (vfxType == VfxType.Omen && _omenAbbrevs.TryGetValue(vfxName, out string orinigalName))
                {
                    vfxName = orinigalName;
                }
                vfxPath = string.Format(templateS, vfxName);
                isActor = false;
            }
            else // 不应进入这个分支 
            {
                throw new ArgumentException($"[PictoACT] 未指定有效的 Actor 或 Static VFX 类型：\n{data} ");
            }
            if (!vfxPath.ToLower().EndsWith(".avfx"))
                throw new ArgumentException($"[PictoACT] VFX 路径 {vfxPath} 必须以 .avfx 结尾。请检查输入。");
            if (vfxPath.Count(c => c == '.') > 1)
                throw new ArgumentException($"[PictoACT] VFX 路径 {vfxPath} 中包含多个点（.），请检查路径格式。");
        }

        private string ParseTag(MultiLineRawArgs data)
        {
            return data.TryGet("Tag", out string tag) ? tag : Vfx.Vfx.DefaultTag;
        }

        private Func<string, bool> ParseFilter(MultiLineRawArgs data)
        {
            if (data.TryGet("Tag", out string tag))
            {
                return (str) => string.Equals(str, tag, StringComparison.OrdinalIgnoreCase);
            }
            if (data.TryGet("Regex", out string regex))
            {
                var re = new Regex(regex, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                return str => str != null && re.IsMatch(str);
            }
            else
                return vfx => true; // 不提供时不过滤
        }

        private IEnumerable<Action<ActorVfx>> ParseActorVfxModifiers(MultiLineRawArgs data, bool isCreate)
        {
            throw new NotImplementedException();
        }

        private List<Action<StaticVfx>> ParseStaticVfxModifiers(MultiLineRawArgs data, bool isCreate)
        {
            var modifiers = new List<Action<StaticVfx>>();
            ParsePosAndAngleModifiers(data, isCreate, modifiers);
            ParseScaleModifier(data, isCreate, modifiers);
            ParseColorModifier(data, isCreate, modifiers);
            bool shouldUpdate = !data.TryGet("Update", out var rawUpdate) || rawUpdate.FromDataString<bool>(); // default true
            if (shouldUpdate && modifiers.Count > 0)
            {
                modifiers.Add(vfx => vfx.Update());
            }
            return modifiers;
        }

        private void ParsePosAndAngleModifiers(MultiLineRawArgs data, bool isCreate, List<Action<StaticVfx>> output)
        {
            var hasPos = TryParsePos(data, out XIVCoord pos);
            var hasAngle = TryParseAngles(data, out Vector3? angles);
            var hasRotation = TryParseRotation(data, out double? rotation);
            var hasCenter = TryParseCenter(data, out XIVCoord center);
            var hasFlip = TryParseFlip(data, out bool? keepX, out bool? keepY);
            if (!(isCreate || hasPos || hasAngle || hasCenter || hasRotation || hasFlip))
                return;
            var transformer = LinearTransformer(isCreate, pos, angles, center, rotation, keepX, keepY);
            output.Add(transformer);
        }

        private bool TryParsePos(MultiLineRawArgs data, out XIVCoord pos)
        {
            if (data.TryGet("Pos", out string rawPos))
            {
                pos = XIVCoord.ParseRawData(rawPos);
                return true;
            }
            pos = default;
            return false;
        }

        private bool TryParseAngles(MultiLineRawArgs data, out Vector3? angles)
        {
            if (data.TryGet("Angle", out string rawAngle))
            {
                var θ = rawAngle.FromDataString<float>();
                angles = new Vector3(θ, 0, 0);
                return true;
            }
            if (data.TryGet("Angle3D", out string rawAngle3D)) // 【需要优化括号和逗号的处理，优化之后考虑合并到 Angle】
            {
                var (θ, θx, θy) = ParseArgs<float, float, float>(rawAngle3D, (0, 0.0), (1, 0.0), (2, 0.0));
                angles = new Vector3(θ, θx, θy);
                return true;
            }
            angles = default;
            return false;
        }

        private bool TryParseCenter(MultiLineRawArgs data, out XIVCoord center)
        {
            if (data.TryGet(out string rawCenter, "O", "Center"))
            {
                center = XIVCoord.ParseRawData(rawCenter);
                return true;
            }
            center = default;
            return false;
        }

        private bool TryParseRotation(MultiLineRawArgs data, out double? rotation)
        {
            if (data.TryGet(out string rawRotation, "θ", "Theta"))
            {
                rotation = rawRotation.FromDataString<double>();
                return true;
            }
            rotation = default;
            return false;
        }

        private bool TryParseFlip(MultiLineRawArgs data, out bool? keepX, out bool? keepY)
        {
            keepX = true;
            keepY = true;
            bool hasX = data.TryGet("+X", out string rawKeepX);
            bool hasY = data.TryGet("+Y", out string rawKeepY);
            if (!hasX && !hasY)
            {
                return false;
            }
            if (hasX)
            {
                keepX = rawKeepX.FromDataString<bool>();
            }
            if (hasY)
            {
                keepY = rawKeepY.FromDataString<bool>();
            }
            return true;
        }

        private Action<StaticVfx> LinearTransformer(
            bool isCreate, XIVCoord pos, Vector3? angles, XIVCoord center, double? rotation, bool? keepX, bool? keepY)
        {
            if (isCreate)
            {
                pos = pos ?? new CartesianCoord(0, 0, 0);
                angles = angles ?? new Vector3((float)PI, 0, 0);
                center = center ?? new CartesianCoord(0, 0, 0);
                rotation = rotation ?? PI;
                keepX = keepX ?? true;
                keepY = keepY ?? true;
            }
            return vfx =>
            {
                // 从上次修改时缓存的值读取未提供项的 default，并将提供的新参数写入缓存
                vfx.PrevPos = pos ?? vfx.PrevPos;
                vfx.PrevAngles = angles ?? vfx.PrevAngles;
                vfx.PrevCenter = center ?? vfx.PrevCenter;
                vfx.PrevRotation = rotation ?? vfx.PrevRotation;
                vfx.PrevKeepX = keepX ?? vfx.PrevKeepX;
                vfx.PrevKeepY = keepY ?? vfx.PrevKeepY;

                var newPos = vfx.PrevPos.Duplicate();
                var newθ = vfx.PrevAngles.X;
                // flip
                if (!vfx.PrevKeepX)
                {
                    newPos = newPos.ScaleBy(-1, 1, 1);
                    newθ *= -1;
                }
                if (!vfx.PrevKeepY)
                {
                    newPos = newPos.ScaleBy(1, -1, 1);
                    newθ = (float)PI - newθ;
                }
                // rotate
                newPos = newPos.RotateTo(vfx.PrevRotation);
                newθ += (float)(vfx.PrevRotation - PI);
                // move
                newPos = newPos.MoveTo(vfx.PrevCenter);
                // apply
                vfx.Pos = (Vector3)newPos;
                vfx.Angles = new Vector3(newθ, vfx.PrevAngles.Y, vfx.PrevAngles.Z);
            };
        }

        private void ParseScaleModifier(MultiLineRawArgs data, bool isCreate, List<Action<StaticVfx>> output)
        {
            Vector3 scales;
            if (data.TryGet("ScaleCyl", out string rawScale3D))
            {
                // 适用于柱状 Omen，x = y
                // 给定一个参数：(a, a, a)
                // 给定两个参数：(a, a, b)
                var (x, _z) = ParseArgs<float, float?>(rawScale3D, (0, 1f), (1, null));
                var y = x;
                var z = _z ?? x;
                scales = new Vector3(x, y, z);
            }
            else if (data.TryGet("Scale", out string rawScale))
            {
                // 适用于平面 Omen，z 轴通常不需要缩放
                // 给定一个参数：(a, a, 1)
                // 给定两个参数：(a, b, 1)
                // 给定三个参数：(a, b, c)
                var (x, _y, _z) = ParseArgs<float, float?, float?>(rawScale, (0, 1f), (1, null), (2, null));
                scales = new Vector3(x, _y ?? x, _z ?? 1f);
            }
            else return; // 未给定参数则直接返回（暂时没用到 isCreate）

            // 如果任一缩放倍率不是 1，则缩放
            if (Abs(scales.X - 1) > 1e-5 || Abs(scales.Y - 1) > 1e-5 || Abs(scales.Z - 1) > 1e-5)
            {
                output.Add(vfx => vfx.Scales = scales);
            }
        }

        private void ParseColorModifier(MultiLineRawArgs data, bool isCreate, List<Action<StaticVfx>> output)
        {
            var mod = ColorModifier(data, isCreate);
            if (mod != null) // 不是 create 模式且没有指定 Color 时不处理
            {
                output.Add(mod);
            }
        }

        private Action<StaticVfx> ColorModifier(MultiLineRawArgs data, bool isCreate)
        {
            if (data.TryGet("Color", out string rawColor)) // 没有指定 Color 时不处理
            {
                var (r, g, b, a) = ParseArgs<float, float, float, float>(rawColor, (3, 1f));
                var color = new Vector4(r, g, b, a);
                return vfx => vfx.Color = color;
            }
            return null;
        }

        private static Dictionary<string, string> _omenAbbrevs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Rect", "general02f" },
            { "Rect2", "general_x02f" }, // 前后双向
            { "Circle", "general_1bf" },
            { "Cross", "n4fg_betaest_o0p" }, // 十字形线条
            { "Fan15", "gl_fan015_0x" },
            { "Fan20", "gl_fan020_0f" },
            { "Fan30", "gl_fan030_1bf" },
            { "Fan40", "z5fc_fan40_o0g" },
            { "Fan45", "gl_fan045_1bf" },
            { "Fan60", "gl_fan060_1bf" },
            { "Fan80", "gl_fan80_o0g" },
            { "Fan90", "gl_fan090_1bf" },
            { "Fan100", "er_gl_fan100_o0v" },
            { "Fan120", "gl_fan120_1bf" },
            { "Fan130", "gl_fan130_0x" },
            { "Fan135", "gl_fan135_c0g" },
            { "Fan145", "m0501_fan145_d1" },
            { "Fan150", "gl_fan150_1bf" },
            { "Fan180", "gl_fan180_1bf" },
            { "Fan210", "gl_fan210_1bf" },
            { "Fan225", "gl_fan225_c0k1" },
            { "Fan240", "x6d3_b1_fan240_p1" },
            { "Fan270", "gl_fan270_0100af" },
        };

    }

    public enum VfxType
    {
        LockOn,
        Channeling,
        CastVfx,
        // StatusLoopVfx,
        ActorVfx,
        Omen,
        StaticVfx,
    }

}