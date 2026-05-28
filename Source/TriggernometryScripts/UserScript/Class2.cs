using System;
using Triggernometry.PluginBridges.BridgeNamazu.Modules;
using Triggernometry.Expressions.String.Utils;

var m = new MountModule();
m.Scan();
m.RegisterAnnotatedMethods();

public class MountModule : ModuleBase
{
    public IntPtr CreateAndSetupMountFuncPtr;
    public MountModule()
    {
        ScanMethod = () =>
        {
            CreateAndSetupMountFuncPtr = Scanner.TryScan("E8 * * * * 0F B7 56 66", nameof(CreateAndSetupMountFuncPtr));
        };
    }

    [CallbackMethod("Mount")]
    public void CbCreateAndSetupMount(string args)
    {
        (IntPtr characterAddress, short mountId, uint buddyModelTop, uint buddyModelBody, uint buddyModelLegs, byte buddyStain, byte unk6, byte unk7)
            = args.ParseArgs<IntPtr, short, uint, uint, uint, byte, byte, byte>((2, 0), (3, 0), (4, 0), (5, 0), (6, 0), (7, 0));
        var mountContainerPtr = GetMountContainer(characterAddress);
        CreateAndSetupMount(mountContainerPtr, mountId, buddyModelTop, buddyModelBody, buddyModelLegs, buddyStain, unk6, unk7);
    }

    public void CreateAndSetupMount(IntPtr mountContainerPtr, short mountId, uint buddyModelTop, uint buddyModelBody, uint buddyModelLegs, byte buddyStain, byte unk6, byte unk7)
    {
        CheckIfAnyZeroPtr(CreateAndSetupMountFuncPtr);
        Plugin.Call(CreateAndSetupMountFuncPtr, mountContainerPtr, mountId, buddyModelTop, buddyModelBody, buddyModelLegs, buddyStain, unk6, unk7);
    }

    private IntPtr GetMountContainer(IntPtr characterPtr)
    {
        if (characterPtr == IntPtr.Zero)
            throw new ArgumentException("Character pointer is null", nameof(characterPtr));
        return characterPtr + 0x670;
    }
}