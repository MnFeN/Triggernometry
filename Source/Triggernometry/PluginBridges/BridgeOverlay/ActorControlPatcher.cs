using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Triggernometry.PluginBridges
{
    public static class ActorControlPatcher
    {
        public static void Patch()
        {
            var asm = BridgeOverlay.WrappedPlugin.pluginObj.GetType().Assembly;
            var targetType = asm.GetType("RainbowMage.OverlayPlugin.NetworkProcessors.LineActorControlExtra")
                ?? throw new Exception("LineActorControlExtra not found");

            var field = targetType.GetField("AllowedActorControlCategories", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                ?? throw new Exception("AllowedActorControlCategories not found");

            var enumType = asm.GetType("RainbowMage.OverlayPlugin.NetworkProcessors.PacketHelper.Server_ActorControlCategory")
                ?? throw new Exception("Server_ActorControlCategory not found");

            var original = (Array)field.GetValue(null);

            // 新枚举值
            var newEnumValue = Enum.ToObject(enumType, 999);

            if (original.Cast<object>().Any(v => v.Equals(newEnumValue)))
                return;

            // 组合新数组
            var newArray = Array.CreateInstance(enumType, original.Length + 1);
            Array.Copy(original, newArray, original.Length);
            newArray.SetValue(newEnumValue, original.Length);

            // 修改 readonly 字段的值
            field.SetValue(null, newArray);

            Console.WriteLine("成功修改 AllowedActorControlCategories");
        }
    }
}
