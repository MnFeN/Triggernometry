using System;
using System.Collections.Generic;
using System.Linq;
using Triggernometry.Expressions.Maths;
using static Triggernometry.Expressions.String.Utils.DataStringHelper;

namespace Triggernometry.PluginBridges.BridgeNamazu.Modules
{
    public class CameraModule : ModuleBase
    {

        public Dictionary<string, int> Offsets => Plugin.IsCN ? OffsetsCN : OffsetsGlobal;

        public IntPtr CameraPtrPtr;
        public IntPtr CameraPtr
        {
            get 
            { 
                CheckIfAnyZeroPtr(CameraPtrPtr);
                return Memory.Read<IntPtr>(CameraPtrPtr); 
            }
        }

        public CameraModule()
        {
            ScanMethod = () =>
            {
                CameraPtrPtr = Scanner.TryScanMultiple(new string[] {
                    "48 8B F1 48 8B FA 48 8D 0D * * * * E8 ? ? ? ? 48 8B D8 48 85 C0 75 ?", // 7.2
                    "48 89 5C 24 ? 48 89 74 24 ? 57 48 83 EC ? 48 8B D9 48 8D 35 * * * * 48 8B 09 BF 01", // 7.2 备用
                }, nameof(CameraPtrPtr));
            };
        }

        public float AngleV
        {
            get => Memory.Read<float>(CameraPtr + Offsets["AngleV"]);
            set => Memory.Write<float>(CameraPtr + Offsets["AngleV"], value);
        }

        public float AngleH // 和游戏的角度（南 = 0）是反的，补偿 pi
        {
            get
            {
                var actualValue = Memory.Read<float>(CameraPtr + Offsets["AngleH"]);
                var convertedValue = MathParser.ModFunction(actualValue, 2 * Math.PI) - Math.PI;
                return (float)convertedValue;
            }
            set // 这个角度似乎不是底层的数值，手动修改（增加或减少）时，改变量的绝对值 θ 会变为 max(θ - pi/40, 0) （即少变化 pi/40）
            {
                double errθ = Math.PI / 40;
                double oldθ = Memory.Read<float>(CameraPtr + Offsets["AngleH"]);
                double newθ = MathParser.ModFunction(value, 2 * Math.PI) - Math.PI;  // 补偿
                double dθ = MathParser.ModFunction(newθ - oldθ + Math.PI, 2 * Math.PI) - Math.PI;
                if (Math.Abs(dθ) >= 3.05)
                {
                    Memory.Write<float>(CameraPtr + Offsets["AngleH"], (float)oldθ + Math.Sign(dθ));
                    oldθ += Math.Sign(dθ) * (1 - errθ); // 实际变化的量
                    dθ = MathParser.ModFunction(newθ - oldθ + Math.PI, 2 * Math.PI) - Math.PI;
                }
                double writeValue = newθ + Math.Sign(dθ) * errθ;
                Memory.Write<float>(CameraPtr + Offsets["AngleH"], (float)writeValue);
            }
        }

        public float GetParam(string param)
        {
            if (Offsets.TryGetValue(param, out int offset))
            {
                return Memory.Read<float>(CameraPtr + offset);
            }
            else
            {
                ErrorLog($"[鲶鱼精邮差扩展] 错误的相机参数 ({param})。");
                return default;
            }
        }

        public void SetParam(string param, float newValue)
        {
            switch (param.ToLower())
            {
                case "angleh": AngleH = newValue; break;
                case "anglev": AngleV = newValue; break;
                default:
                    if (Offsets.TryGetValue(param, out int offset))
                    {
                        IntPtr address = CameraPtr + offset;
                        Memory.Write<float>(address, newValue);
                        Custom2Log($"[鲶鱼精邮差扩展] 成功设置相机参数 {param} = {newValue}");
                    }
                    else
                    {
                        ErrorLog($"[鲶鱼精邮差扩展] 错误的相机参数 ({param})。");
                    };
                    break;
            }
        }

        /// <summary> 接收单个关键词：reset apply clearconfig initconfig; <br />
        /// 或每行一个指定修改的数据，如 Distance = 10 </summary>
        [CallbackMethod("SetCameraParams")]
        public void SetCameraParams(string cmd)
        {
            CheckBeforeExecution(cmd);
            switch (cmd.Trim().ToLower())
            {
                case "reset": // 临时重置当前视距、视角及其范围为游戏默认值
                    foreach (var kvp in OriginalParams)
                    {
                        SetParam(kvp.Key, kvp.Value);
                    }
                    break;
                case "apply": // 将配置中的当前视距、视角及其范围应用到游戏
                    //if (GetConfig<bool>("camera_enabled") != true) return;
                    foreach (var kvp in OriginalParams)
                    {
                        var value = GetConfig<float>($"camera_{kvp.Key}");
                        if (value == null) continue;
                        SetParam(kvp.Key, value.Value);
                    }
                    break;
                case "clearconfig": // 清除相关配置，并恢复游戏默认值
                    SetConfig("camera_enabled", false);
                    SetCameraParams("reset");
                    break;
                case "initconfig": // 初始化相关配置并应用到游戏
                    foreach (var kvp in DefaultEditedParams)
                    {
                        SetConfig($"camera_{kvp.Key}", kvp.Value);
                        SetParam(kvp.Key, kvp.Value);
                    }
                    break;
                default:
                    var kvps = cmd.Split('\n').Select(data => data.Split(new char[] { '=', ':' }, 2)).Where(data => data.Length == 2);
                    foreach (var kvp in kvps)
                    {
                        var key = kvp[0].Trim();
                        if (!Offsets.ContainsKey(key)) continue;
                        var value = (float)MathParser.Parse(kvp[1]);
                        SetParam(key, value);
                    }
                    break;
            }
        }

        public Dictionary<string, int> OffsetsCN = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) // 7.3
        {
            { "Distance", 0x124 },
            { "MinDistance", 0x128 },
            { "MaxDistance", 0x12C },
            { "FoV", 0x130 },
            { "MinFoV", 0x134 },
            { "MaxFoV", 0x138 },
            { "AngleH", 0x140 }, // 这个角度似乎不是底层的数值，手动修改（增加或减少）时，改变量的绝对值 dθ 会变为 max(dθ - pi/40, 0)
            { "AngleV", 0x144 }, // 上 -pi/2   下 pi/2
            { "MinAngleV", 0x158 },
            { "MaxAngleV", 0x15C },
        };

        public Dictionary<string, int> OffsetsGlobal = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)  // 7.3
        {
            { "Distance", 0x124 },
            { "MinDistance", 0x128 },
            { "MaxDistance", 0x12C },
            { "FoV", 0x130 },
            { "MinFoV", 0x134 },
            { "MaxFoV", 0x138 },
            { "AngleH", 0x140 }, // 这个角度似乎不是底层的数值，手动修改（增加或减少）时，改变量的绝对值 dθ 会变为 max(dθ - pi/40, 0)
            { "AngleV", 0x144 }, // 上 -pi/2   下 pi/2
            { "MinAngleV", 0x158 },
            { "MaxAngleV", 0x15C },
        };

        // 游戏默认
        public readonly Dictionary<string, float> OriginalParams = new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase)
        {
            { "MinDistance", 1.5f },
            { "MaxDistance", 20.0f },
            { "MinFoV", 0.69f },
            { "MaxFoV", 0.78f },
            { "MinAngleV", -1.483529806f },
            { "MaxAngleV", 0.7853981853f },
        };

        public readonly Dictionary<string, float> DefaultEditedParams = new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase)
        {
            { "MinDistance", 0.5f },
            { "MaxDistance", 9999f },
            { "MinFoV", 0.69f },
            { "MaxFoV", 0.78f },
            { "MinAngleV", -1.569f }, // 超过这个数值时第一人称视角难以旋转
            { "MaxAngleV", 1.569f },  // 超过这个数值时视角会反转
        };
    }
}
