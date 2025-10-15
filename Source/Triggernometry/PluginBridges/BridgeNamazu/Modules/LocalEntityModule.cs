using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using Triggernometry;
using Triggernometry.PluginBridges.BridgeNamazu;
using Triggernometry.PluginBridges.BridgeNamazu.Modules;
using Triggernometry.Utilities;
using static Triggernometry.Debug;
using static Triggernometry.Utilities.DataStringHelper;

public class LocalEntityModule : ModuleBase
{

    // FFXIVClientStructs/FFXIV/Client/Game/Object/ClientObjectManager.cs
    public IntPtr ClientObjectManagerPtr;
    public IntPtr CreateBattleCharacterFuncPtr;
    public IntPtr GetObjectByIndexFuncPtr;
    public IntPtr DeleteObjectByIndexFuncPtr;

    // FFXIVClientStructs/FFXIV/Client/Game/Character/Character.CharacterSetupContainer
    public Func<int> CharacterSetupContainerOffset;

    // FFXIVClientStructs/FFXIV/Client/Game/Character/CharacterSetupContainer
    public IntPtr CopyFromCharacterFuncPtr;
    public IntPtr SetupBNpcFuncPtr;

    public EntityModule entityModule => BridgeNamazu.GetModule<EntityModule>();

    public LocalEntityModule()
    {
        ScanMethod = () =>
        {
            ClientObjectManagerPtr = Scanner.TryScan("48 8D 0D * * * * E8 ?? ?? ?? ?? C7 43 60 FF FF FF FF", nameof(ClientObjectManagerPtr));
            CreateBattleCharacterFuncPtr = Scanner.TryScan("E8 * * * * 41 89 44 FC ??", nameof(CreateBattleCharacterFuncPtr));
            GetObjectByIndexFuncPtr = Scanner.TryScan("E8 * * * * 4C 8B C0 4D 85 C0", nameof(GetObjectByIndexFuncPtr));
            DeleteObjectByIndexFuncPtr = Scanner.TryScan("E8 * * * * C6 43 49 00", nameof(DeleteObjectByIndexFuncPtr));

            CharacterSetupContainerOffset = () => 0x1B00;

            CopyFromCharacterFuncPtr = Scanner.TryScan("E8 * * * * 8B 87 ?? ?? ?? ?? 85 C0 74 ?? 83 F8", nameof(CopyFromCharacterFuncPtr));
            SetupBNpcFuncPtr = Scanner.TryScan("E8 * * * * 45 0F B6 86 ?? ?? ?? ?? 48 8D 8F", nameof(SetupBNpcFuncPtr));
        };
    }

    public int CreateBattleCharacter(int index = -1, byte param = 0)
    {
        return Memory.CallInjected64<int>(CreateBattleCharacterFuncPtr, ClientObjectManagerPtr, index, param);
    }

    public IntPtr GetObjectByIndex(int idx)
    {
        return Memory.CallInjected64<IntPtr>(GetObjectByIndexFuncPtr, ClientObjectManagerPtr, (ushort)idx);
    }

    public IntPtr DeleteObjectByIndex(int idx, byte param)
    {
        return Memory.CallInjected64<IntPtr>(DeleteObjectByIndexFuncPtr, ClientObjectManagerPtr, (ushort)idx, param);
    }

    public IntPtr CopyFromCharacter(IntPtr targetPtr, IntPtr sourcePtr, CopyFlags flags)
    {
        var characterSetupContainerPtr = targetPtr + CharacterSetupContainerOffset();
        return Memory.CallInjected64<IntPtr>(CopyFromCharacterFuncPtr, characterSetupContainerPtr, sourcePtr, (uint)flags);
    }

    public void SetupBNpc(IntPtr targetPtr, uint bNpcBaseId, uint bNpcNameId = 0)
    {
        var characterSetupContainerPtr = targetPtr + CharacterSetupContainerOffset();
        Memory.CallInjected64(SetupBNpcFuncPtr, characterSetupContainerPtr, bNpcBaseId, bNpcNameId);
    }

    public IntPtr CreateLocalEntity(Vector3 pos, float heading = 0)
    {
        var idx = CreateBattleCharacter();
        var entityPtr = GetObjectByIndex(idx);
        entityModule.SetPos(entityPtr, pos.X, pos.Y, pos.Z);
        entityModule.SetDefaultPos(entityPtr, pos.X, pos.Y, pos.Z);
        entityModule.SetHeading(entityPtr, heading);
        entityModule.SetDefaultHeading(entityPtr, heading);
        return entityPtr;
    }

    [Flags]
    public enum CopyFlags : uint
    {
        None = 0x00,
        Mode = 0x1, // emote loop etc
        Mount = 0x2,
        ClassJob = 0x4,
        Companion = 0x20,
        WeaponHiding = 0x80,
        Target = 0x400,
        Name = 0x1000,
        LastAnimation = 0x8000,
        Position = 0x10000, // includes rotation
        UseSecondaryCharaId = 0x200000,
        Ornament = 0x400000
    }

}