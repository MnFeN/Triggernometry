using Scarborough.Drawing;
using System;
using Triggernometry.Expressions.String.Utils;
using Triggernometry.PluginBridges.BridgeNamazu.Modules;

var m = new BgmModule();
m.Scan();
m.RegisterAnnotatedMethods();

public class BgmModule : ModuleBase
{

    // https://github.com/perchbirdd/OrchestrionPlugin/blob/main/Orchestrion/BGMSystem/BGMAddressResolver.cs
    public IntPtr BasePtrPtr;

    // https://github.com/perchbirdd/OrchestrionPlugin/blob/main/Orchestrion/BGMSystem/BGMController.cs
    public IntPtr GetBGMSceneListPtr()
    {
        if (BasePtrPtr == IntPtr.Zero) return IntPtr.Zero;
        var basePtr = Memory.Read<IntPtr>(BasePtrPtr);
        if (basePtr == IntPtr.Zero) return IntPtr.Zero;
        return Memory.Read<IntPtr>(basePtr + 0xC0);
    }

    public BgmModule()
    {
        ScanMethod = () =>
        {
            BasePtrPtr = Scanner.TryScan("48 8B 05 * * * * 48 85 C0 74 51 83 78 08 0B", "basePtr");
        };
    }

    [CallbackMethod("BGM")]
    public void CbPlay(string data)
    {
        var songId = data.ParseData<ushort>();
        Play(songId);
    }

    public const byte Scene0ResumeFlag = 0x04;

    public void Play(ushort songId)
    {
        // bgmSceneListPtr 相当于连续布局的 BGMScene[12]，播放 bgm 的优先级高到低排列，这里只写最高优先级的 scene 0
        var scene = 0; // 0-11
        var bgmSceneListPtr = GetBGMSceneListPtr();
        if (bgmSceneListPtr == IntPtr.Zero)
            throw new Exception("BGM 播放数组为空指针");

        var scenePtr = bgmSceneListPtr + scene * 0xA0;

        // songId == 0 且 scene == 0 时，按插件的实现写 Resume，表示恢复原有 BGM
        if (songId == 0 && scene == 0)
            Memory.Write(scenePtr + 0x04, Scene0ResumeFlag);

        // BgmReference
        Memory.Write(scenePtr + 0x0C, songId);
        // BgmId
        Memory.Write(scenePtr + 0x0E, songId);
        // PreviousBgmId
        Memory.Write(scenePtr + 0x10, songId);
        // Timer
        Memory.Write(scenePtr + 0x14, 0f);
        // TimerEnable
        Memory.Write(scenePtr + 0x12, (byte)0);
    }

    /* https://github.com/perchbirdd/OrchestrionPlugin/blob/main/Orchestrion/BGMSystem/BGMScene.cs
    [StructLayout(LayoutKind.Explicit, Size = 0xA0)]
    public unsafe struct BGMScene
    {
        [FieldOffset(0x04)]
        public byte Flags;
        [FieldOffset(0x0C)]
        public ushort BgmReference;
        [FieldOffset(0x0E)]
        public ushort BgmId;
        [FieldOffset(0x10)]
        public ushort PreviousBgmId;
        [FieldOffset(0x12)]
        public byte TimerEnable;
        [FieldOffset(0x14)]
        public float Timer;
    }
    */
}