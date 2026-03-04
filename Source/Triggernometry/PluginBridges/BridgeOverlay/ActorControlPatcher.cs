using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Triggernometry.Core;
using Triggernometry.Expressions.Maths;

namespace Triggernometry.PluginBridges
{
    public static class ActorControlPatcher
    {
        public static void RegisterCategoriesCallback(object _, string data)
        { 
            var newCategories = data.Split(',').Select(raw => (ushort)MathParser.Parse(raw)).ToArray();
            RegisterCategories(newCategories);
        }

        public static void RegisterCategories(ushort[] newCategories)
        {
            var asm = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == "OverlayPlugin.Core")
                ?? throw new Exception("OverlayPlugin.Core not found");

            var targetType = asm.GetType("RainbowMage.OverlayPlugin.NetworkProcessors.LineActorControlExtra")
                ?? throw new Exception("LineActorControlExtra not found");

            var field = targetType.GetField("AllowedActorControlCategories", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                ?? throw new Exception("AllowedActorControlCategories not found");

            var enumType = asm.GetType("RainbowMage.OverlayPlugin.NetworkProcessors.Server_ActorControlCategory")
                ?? throw new Exception("Server_ActorControlCategory not found");

            // original categories
            var original = (Array)field.GetValue(null);
            var originalObjects = original.Cast<object>().ToList();

            // filter new categories
            var toAdd = new List<object>();
            foreach (ushort val in newCategories)
            {
                var enumValue = Enum.ToObject(enumType, val);
                if (!originalObjects.Any(v => v.Equals(enumValue)))
                {
                    toAdd.Add(enumValue);
                }
            }

            // overwrite if any new categories
            if (toAdd.Count > 0)
            {
                var newArray = Array.CreateInstance(enumType, original.Length + toAdd.Count);

                Array.Copy(original, newArray, original.Length);
                for (int i = 0; i < toAdd.Count; i++)
                {
                    newArray.SetValue(toAdd[i], original.Length + i);
                }
                field.SetValue(null, newArray);

                RealPlugin.Instance.UnfilteredAddToLog(RealPlugin.DebugLevelEnum.Info, 
                    $"已添加 {toAdd.Count} 个 ActorControl 新分类：{string.Join(", ", toAdd.Select(o => $"0x{(ushort)o:X4}"))}");
            }
        }
    }
}