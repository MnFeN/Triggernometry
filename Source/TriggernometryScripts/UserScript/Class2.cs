using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Triggernometry;
using Triggernometry.PluginBridges.BridgeNamazu;
using Triggernometry.PluginBridges.BridgeNamazu.Modules;


var m = new MapEffectScannerModule();
m.Scan();
m.DumpMapEffects();

public class MapEffectScannerModule : ModuleBase
{

    public IntPtr GetMapEffectSlotHandlePtr;
    public IntPtr CheckMapEffectValidityPtr;

    // FFXIVClientStructs/FFXIV/Client/Game/InstanceContent/ContentDirector.cs
    // [FieldOffset(0xC90)] public MapEffectList* MapEffects;
    // 如果有变化且 FFCS 没来得及更新，可以用附近不断变化的 public float ContentTimeLeft 的偏移 - 0x60 作为参考
    public Func<int> MapEffectListOffset = () => 0xC90; // 7.3 CE8

    public IntPtr EventFrameworkPtrPtr => BridgeNamazu.GetModule<EnvironmentEffectModule>().EventFrameworkPtrPtr;
    public IntPtr ContentDirectorPtr => BridgeNamazu.GetModule<EnvironmentEffectModule>().ContentDirectorPtr;

    public MapEffectScannerModule()
    {
        ScanMethod = () =>
        {
            // 下层 MapEffect 函数中的 v5 = sub_14070D2E0(a1, *(unsigned int *)(v4 + 12LL * a2), 0LL);
            GetMapEffectSlotHandlePtr = Scanner.TryScan("E9 * * * * 8B 43 ? 45 33 C0 8B D0", nameof(GetMapEffectSlotHandlePtr));
            // 下层 MapEffect 函数中的 if ( (unsigned __int8)sub_1406BEF50(v6, v8) == 1 )
            CheckMapEffectValidityPtr = Scanner.TryScan("E8 * * * * 84 C0 74 ? 48 8B 8B ? ? ? ? 44 8B C6 8B D7", nameof(CheckMapEffectValidityPtr));
        };
    }

    IntPtr GetMapEffectSlotHandle(uint slotLayoutId)
    {
        return Memory.CallInjected64<IntPtr>(GetMapEffectSlotHandlePtr, (byte)6, (int)slotLayoutId, (int)0);
    }

    bool CheckMapEffectValidity(IntPtr slotHandle, int bitIndex)
    {
        return Memory.CallInjected64<bool>(CheckMapEffectValidityPtr, slotHandle, (uint)bitIndex);
    }

    public void DumpMapEffects()
    {
        CheckIfAnyZeroPtr(GetMapEffectSlotHandlePtr, CheckMapEffectValidityPtr, EventFrameworkPtrPtr, ContentDirectorPtr);
        try
        {
            var contentDirectorPtr = ContentDirectorPtr;
            if (contentDirectorPtr == IntPtr.Zero)
            {
                Debug.Log("[鲶鱼精邮差扩展] 当前地图不存在 Director");
                return;
            }
            Debug.Log($"contentDirectorPtr = {(long)contentDirectorPtr:X}");

            var mapEffectListPtr = Memory.Read<IntPtr>(contentDirectorPtr + MapEffectListOffset());
            if (mapEffectListPtr == IntPtr.Zero)
            {
                Debug.Log("[鲶鱼精邮差扩展] MapEffectList 指针为空");
                return;
            }
            Debug.Log($"mapEffectListPtr = {(long)mapEffectListPtr:X}");

            // 末尾 +1538 (0x602) 为 count
            ushort count = Memory.Read<ushort>(mapEffectListPtr + 0x602);
            int max = Math.Min((int)count, 128);
            Debug.Log($"Count = {count}");

            List<string> results = new List<string>();
            for (int slotIdx = 0; slotIdx < max; slotIdx++)
            {
                List<ushort> slotResults = new List<ushort>();
                var slotPtr = mapEffectListPtr + slotIdx * 12;
                var slotLayoutId = Memory.Read<uint>(slotPtr + 0x0);
                if (slotLayoutId == 0)
                {
                    WarningLog($"[鲶鱼精邮差扩展] Slot Index {slotIdx} 对应的 LayoutID 为空");
                    continue;
                }

                var slotHandle = GetMapEffectSlotHandle(slotLayoutId);
                if (slotHandle == IntPtr.Zero)
                {
                    WarningLog($"[鲶鱼精邮差扩展] Slot Index {slotIdx} 对应的 Handle 为空");
                    continue;
                }

                // 每个槽位分别检测 16 个 bit
                for (int bit = 0; bit < 16; bit++)
                {
                    if (CheckMapEffectValidity(slotHandle, bit))
                    {
                        slotResults.Add((ushort)(1 << bit));
                    }
                }
                if (slotResults.Any())
                {
                    var result = $"{slotIdx:X2}: {string.Join(", ", slotResults.Select(i => $"{i:X4}"))}";
                    results.Add(result);
                    Debug.Log(result);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"[鲶鱼精邮差扩展] DumpMapEffects 失败:\n{ex.Message}");
        }

    }

    /* FFXIVClientStructs/FFXIV/Client/Game/InstanceContent/ContentDirector.cs
    
    [StructLayout(LayoutKind.Explicit, Size = 0x608)]
    public partial struct MapEffectList
    {
        [FieldOffset(0x00)]  internal FixedSizeArray128<MapEffectItem> _items;
        [FieldOffset(0x600)] public ushort ContentDirectorManagedSGRowId;
        [FieldOffset(0x602)] public ushort ItemCount;
        [FieldOffset(0x604)] public byte Dirty;
    }

    [StructLayout(LayoutKind.Explicit, Size = 0xC)]
    public struct MapEffectItem
    {
        [FieldOffset(0x00)] public uint LayoutId;
        [FieldOffset(0x05)] public byte Unknown1;
        [FieldOffset(0x08)] public ushort State;
        [FieldOffset(0x0A)] public byte Flags;
    }
    */

}