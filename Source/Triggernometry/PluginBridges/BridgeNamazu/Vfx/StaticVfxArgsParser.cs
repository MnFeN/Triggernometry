using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using Triggernometry.Expressions.String.Models;
using Triggernometry.Expressions.String.Utils;
using Triggernometry.Utilities.Maths;

namespace Triggernometry.PluginBridges.BridgeNamazu.Vfx
{
    internal enum VfxType
    {
        Omen,
        StaticVfx,
    }

    internal static class StaticVfxArgsParser
    {
        public static StaticVfxArgs Parse(MultiLineRawArgs data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var args = new StaticVfxArgs();

            args.Action = ParseAction(data);
            ParseTypeAndPath(data, args);
            ParseControl(data, args);
            ParseColor(data, args);
            ParsePose(data, args);
            ParseScale(data, args);
            ParseTransform(data, args);

            Validate(args);
            return args;
        }

        private static StaticVfxAction ParseAction(MultiLineRawArgs data)
        {
            if (!data.TryGet("Action", out string rawAction) || string.IsNullOrWhiteSpace(rawAction))
                return StaticVfxAction.Create;

            switch (rawAction.Trim().ToUpperInvariant())
            {
                case "CREATE":
                    return StaticVfxAction.Create;

                case "MODIFY":
                case "CHANGE":
                    return StaticVfxAction.Modify;

                case "REMOVE":
                    return StaticVfxAction.Remove;

                case "TRIANGULATE":
                case "△": // u25B3 Triangle
                case "Δ": // u0394 Delta
                case "∆": // u2206 Increment
                    return StaticVfxAction.Triangulate;

                case "EXAFLARE":
                case "地火":
                    return StaticVfxAction.ExaFlare;

                default:
                    throw new ArgumentException($"[PictoACT] 未知的 Action：{rawAction}");
            }
        }

        private static void ParseControl(MultiLineRawArgs data, StaticVfxArgs args)
        {
            args.Raw = data;

            if (data.TryGet("Tag", out string tag))
                args.Tag = tag;

            if (data.TryGet("Regex", out string regex))
                args.Regex = regex;

            if (data.TryGet("Delay", out string rawDelay))
                args.Delay = rawDelay.ParseData<double>();

            if (data.TryGet(out string rawTime, "Time", "t"))
                args.Time = rawTime.ParseData<double>();
        }

        private static void ParseColor(MultiLineRawArgs data, StaticVfxArgs args)
        {
            if (!data.TryGet("Color", out string rawColor))
                return;

            var (r, g, b, a) = rawColor.ParseArgs<float, float, float, float>((3, 1f));
            args.Color = new Vector4(r, g, b, a);
        }

        private static void ParsePose(MultiLineRawArgs data, StaticVfxArgs args)
        {
            if (data.TryGet("Pos", out string rawPos))
                args.Pos = DynamicCoordArg.Parse(rawPos);

            if (data.TryGet("Target", out string rawTarget))
                args.Target = DynamicCoordArg.Parse(rawTarget);

            if (data.TryGet("Angle", out string rawAngle))
            {
                var θ = rawAngle.ParseData<float>();
                args.Angle3D = new Vector3(θ, 0, 0);
                return;
            }
            else if (data.TryGet("Angle3D", out string rawAngle3D))
            {
                var (θ, θx, θy) =
                    rawAngle3D.ParseArgs<float, float, float>((0, 0.0), (1, 0.0), (2, 0.0));

                args.Angle3D = new Vector3(θ, θx, θy);
            }
        }

        private static void ParseScale(MultiLineRawArgs data, StaticVfxArgs args)
        {
            if (data.TryGet("ScaleCyl", out string rawScaleCyl))
            {
                args.Scale = ScaleArg.ParseCyl(rawScaleCyl);
                return;
            }

            if (data.TryGet("Scale", out string rawScale))
            {
                args.Scale = ScaleArg.Parse(rawScale);
            }
        }

        private static void ParseTransform(MultiLineRawArgs data, StaticVfxArgs args)
        {
            if (data.TryGet(out string rawCenter, "O", "Center"))
                args.TransformCenter = DynamicCoordArg.Parse(rawCenter);

            ParseTransformNorth(data, args);

            if (data.TryGet(out string rawKeepX, "+X", "KeepX"))
                args.TransformKeepX = rawKeepX.ParseData<bool>();

            if (data.TryGet(out string rawKeepY, "+Y", "KeepY"))
                args.TransformKeepY = rawKeepY.ParseData<bool>();
        }

        private static readonly Regex _dirRegex =
            new Regex(@"^dir(?<isNegative>N)?(?<segments>\d+)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static void ParseTransformNorth(MultiLineRawArgs data, StaticVfxArgs args)
        {
            // 指定了 θ 参数
            if (data.TryGet(out string rawNorth, "θ", "Theta"))
            {
                rawNorth = rawNorth.Trim();

                // 如 θ: clear（取消指定 θ，恢复默认朝向）
                if (rawNorth.Equals("clear", StringComparison.OrdinalIgnoreCase))
                {
                    // 哨兵值，表示取消 θ 指定，恢复默认朝向。与未指定（null）不同，未指定时不会做增量修改。
                    args.TransformNorthAngle = double.NaN; 
                    return;
                }

                // 如 θ: 40001234（实体 ID）
                // 注意必须放在角度表达式之前，否则纯数字实体 ID 会先被解析成角度。
                if (DynamicCoordArg.TryParseEntityId(rawNorth, out var entityId))
                {
                    args.TransformNorthTarget = DynamicCoordArg.FromEntityId(entityId);
                    return;
                }

                // 如 θ: π/2、θ: DirToRad(3, 8)、θ: θ(x1, y1, x2, y2)
                // 先尝试表达式解析，避免函数参数里的逗号被误判为坐标。
                if (rawNorth.TryParseData<double>(out var θ))
                {
                    args.TransformNorthAngle = θ;
                    return;
                }

                // 如 θ: 120, 100, 0（相对 O 指向的坐标）
                try
                {
                    var coord = XIVCoord.ParseRawData(rawNorth);
                    args.TransformNorthTarget = DynamicCoordArg.FromCoord(coord);
                    return;
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"[PictoACT] θ/Theta 必须是角度表达式、坐标或实体 ID：{rawNorth}", ex);
                }
            }

            // 查找 dir 指定的方向，如 dir8 = 3, dirN4 = 1
            foreach (var pair in data.Data)
            {
                var match = _dirRegex.Match(pair.Key);
                if (match.Success) // 如 dirN4 = 1
                {
                    var index = pair.Value.ParseData<double>(); // 1
                    var segments = int.Parse(match.Groups["segments"].Value); // 4

                    if (segments <= 0)
                        throw new ArgumentException("[PictoACT] dir 的分段数必须大于 0。");

                    if (match.Groups["isNegative"].Success) // 斜向等分改为正向等分，dirN4 = 1 转换为 dir4 = 1.5
                        index += 0.5;

                    index = (index % segments + segments) % segments;
                    args.TransformNorthAngle = index / segments * 2 * Math.PI - Math.PI;
                    break;
                }
            }

            // 未指定 θ 时，如果中心 O 不是实体则默认绝对北，如果是实体则默认实体面向。
        }

        private static void ParseTypeAndPath(MultiLineRawArgs data, StaticVfxArgs args)
        {
            // 尝试找到参数中指定的 Vfx 类型，如：
            // Omen: Circle
            // StaticVfx: vfx/xxx.avfx
            foreach (var pair in data.Data)
            {
                if (Enum.TryParse(pair.Key, true, out VfxType vfxType) && _staticCommandTemplates.ContainsKey(vfxType))
                {
                    string vfxName = pair.Value;
                    args.VfxPath = NormalizeStaticVfxPath(vfxType, vfxName);
                    break;
                }
            }
        }

        private static void Validate(StaticVfxArgs args)
        {
            if (args.Action == StaticVfxAction.Create)
            {
                if (args.Scale?.HasDistanceToken == true && args.Target == null)
                    throw new ArgumentException("[PictoACT] 创建 VFX 时若在 Scale 中使用 _d，必须指定 Target。");

                if (args.TransformNorthTarget != null && args.TransformCenter == null)
                    throw new ArgumentException("[PictoACT] θ 指定为坐标或实体 id 时，必须同时指定 O/Center。");
            }

            if (args.Target != null && args.Angle3D.HasValue)
                throw new ArgumentException("[PictoACT] Target 和 Angle/Angle3D 不能同时指定。");
        }

        private static readonly Dictionary<VfxType, string> _staticCommandTemplates = new Dictionary<VfxType, string>
        {
            [VfxType.Omen] = "vfx/omen/eff/{0}.avfx",
            [VfxType.StaticVfx] = "{0}",
        };

        /// <summary>
        /// 根据 VFX 类型和参数中指定的名称或路径，生成最终的 VFX 路径。<br />
        /// </summary>
        public static string NormalizeStaticVfxPath(VfxType vfxType, string vfxNameOrPath)
        {
            if (string.IsNullOrWhiteSpace(vfxNameOrPath))
                throw new ArgumentException("[PictoACT] VFX 名称或路径不能为空。", nameof(vfxNameOrPath));

            // 如果参数不是一个完整的路径（不以 .avfx 结尾），根据类型模板生成路径；
            // 否则跳过这步，直接无视 type 当做完整路径使用。

            var path = vfxNameOrPath;
            if (!path.ToLowerInvariant().EndsWith(".avfx"))
            {
                if (!_staticCommandTemplates.TryGetValue(vfxType, out string template))
                    throw new ArgumentException($"[PictoACT] 不支持的 Static VFX 类型：{vfxType}");

                // 如果是 Omen 类型，允许使用简称（如 Circle）代替全名（如 general_1bf）。如果找不到对应的简称，则原样使用参数值。
                var name = vfxNameOrPath;
                if (vfxType == VfxType.Omen && _omenAbbrevs.TryGetValue(name, out string originalName))
                {
                    name = originalName;
                }

                path = string.Format(template, name);

                // 防御性检查，不应该存在不以 avfx 结尾的模板。不以 .avfx 结尾的路径传入 StaticVfxCreate 会直接崩溃。
                if (!path.ToLowerInvariant().EndsWith(".avfx"))
                    throw new ArgumentException($"[PictoACT] VFX 路径 {path} 必须以 .avfx 结尾。请检查输入。");
            }

            if (path.Count(c => c == '.') > 1)
                throw new ArgumentException($"[PictoACT] VFX 路径 {path} 中包含多个点（.），请检查路径格式。");

            return path;
        }

        /// <summary> Omen 的缩写映射表，常用形状可以用简称代替 Omen 全名 </summary>
        private static readonly Dictionary<string, string> _omenAbbrevs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Rect"] = "general02f",
            ["Rect2"] = "general_x02f",
            ["Circle"] = "general_1bf",
            ["Cross"] = "n4fg_betaest_o0p",
            ["Fan15"] = "gl_fan015_0x",
            ["Fan20"] = "gl_fan020_0f",
            ["Fan30"] = "gl_fan030_1bf",
            ["Fan40"] = "z5fc_fan40_o0g",
            ["Fan45"] = "gl_fan045_1bf",
            ["Fan60"] = "gl_fan060_1bf",
            ["Fan80"] = "gl_fan80_o0g",
            ["Fan90"] = "gl_fan090_1bf",
            ["Fan100"] = "er_gl_fan100_o0v",
            ["Fan120"] = "gl_fan120_1bf",
            ["Fan130"] = "gl_fan130_0x",
            ["Fan135"] = "gl_fan135_c0g",
            ["Fan145"] = "m0501_fan145_d1",
            ["Fan150"] = "gl_fan150_1bf",
            ["Fan180"] = "gl_fan180_1bf",
            ["Fan210"] = "gl_fan210_1bf",
            ["Fan225"] = "gl_fan225_c0k1",
            ["Fan240"] = "x6d3_b1_fan240_p1",
            ["Fan270"] = "gl_fan270_0100af",
        };
    }
}