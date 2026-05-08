using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

// https://github.com/moewcorp/NyaDraw/tree/master

namespace Triggernometry.FFXIV.Vfx
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct _VfxStruct
    {
        [FieldOffset(0x38)] public byte Flags;
        [FieldOffset(0x50)] public Vector3 Position;
        [FieldOffset(0x60)] public Quaternion Rotation;
        [FieldOffset(0x70)] public Vector3 Scale;

        // 以下三个是 StaticVfxRemove 的时候调用的
        [FieldOffset(0x80)] public IntPtr Unk80; // 如果非空，会调用某个全局 cleanup，然后清零
        [FieldOffset(0x88)] public byte State88;
        [FieldOffset(0x89)] public byte State89;

        [FieldOffset(0x128)] public int ActorCaster;
        [FieldOffset(0x130)] public int ActorTarget;

        [FieldOffset(0x1B8)] public int StaticCaster;
        [FieldOffset(0x1C0)] public int StaticTarget;
        [FieldOffset(0x2A0)] public Apricot* Apricot;

        // 0x248: flag, 0x40 bit 在 fadeout 时设为 1
        [FieldOffset(0x250)] public float Speed;
        [FieldOffset(0x258)] public float FadeOutTimer; // 1f = 0.0167 s
        // 0x25C: int, fadeout 时设为 0
        [FieldOffset(0x260)] public Vector4 Color;
    }
    /*
public unsafe void VfxSetPos(IntPtr ptr, float x, float y, float z)
{
    VfxStruct* vfx = (VfxStruct*)ptr;
    vfx->Position = new Vector3(x, z, y);
}

public unsafe void VfxSetRotation(IntPtr ptr, float θ, float θx = 0, float θy = 0)
{
    VfxStruct* vfx = (VfxStruct*)ptr;
    var q = Quaternion.CreateFromYawPitchRoll(θy, θx, θ);
    vfx->Rotation = new Quaternion(q.X, q.Z, q.Y, q.W);
}

public unsafe void VfxSetScale(IntPtr ptr, float sx, float sy, float sz)
{
    VfxStruct* vfx = (VfxStruct*)ptr;
    vfx->Scale = new Vector3(sx, sz, sy);
}

public unsafe void VfxSetColor(IntPtr ptr, float r, float g, float b, float a)
{
    VfxStruct* vfx = (VfxStruct*)ptr;
    vfx->Color = new Vector4(r, g, b, a);
}

public unsafe void VfxSetSpeed(IntPtr ptr, float speed)
{
    VfxStruct* vfx = (VfxStruct*)ptr;
    vfx->Speed = speed;
}

public unsafe void VfxUpdate(IntPtr ptr)
{
    VfxStruct* vfx = (VfxStruct*)ptr;
    vfx->Flags |= 0x2;
}
*/
}
