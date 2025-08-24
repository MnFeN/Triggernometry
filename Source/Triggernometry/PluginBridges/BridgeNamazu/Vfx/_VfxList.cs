using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

// https://github.com/moewcorp/NyaDraw/tree/master

namespace Triggernometry.FFXIV.Vfx
{
    [StructLayout(LayoutKind.Explicit, Size = 0x88 * 0x800)]
    public unsafe struct VFXList : IEnumerable<VFXListData>
    {
        public static HashSet<IntPtr> vfxHandlesSet = new HashSet<IntPtr>();
        private static VFXList* vfxListInstance;
        public static bool CheckVFXHandleExists(IntPtr vfxHandle)
        {
            return vfxHandlesSet.Contains(vfxHandle);
        }
        /*
        // 每次更新时同步 HashSet，确保最新的数据
        public static void SyncVfxHandles()
        {
            vfxHandlesSet.Clear();
            Span<VFXListData> listSpan;
            try
            {
                listSpan = vfxListInstance->ListSpan;
            }
            catch (NullReferenceException)
            {
                vfxListInstance = Instance();
                listSpan = vfxListInstance->ListSpan;
            }

            foreach (var vfx in listSpan)
            {
                if (vfx.IsValid())
                {
                    vfxHandlesSet.Add(vfx.VFXHandle);
                }
            }
        }
        */
        [FieldOffset(0x0)] 
        private fixed byte list[0x88 * 0x800];

        public Span<VFXListData> ListSpan
        {
            get
            {
                fixed (byte* ptr = list)
                {
                    return new Span<VFXListData>(ptr, 0x800);
                }
            }
        }
        public VFXListData* GetVFXListDataByIndex(int index)
        {
            if (index < 0 || index >= 800)
            {
                throw new ArgumentOutOfRangeException(nameof(index), $"Index {index} must be between 0-799.");
            }
            return (VFXListData*)Unsafe.AsPointer(ref ListSpan[index]);
        }
        /*
        public static VFXList* Instance()
        {
            if (Svc.SigScanner.TryScanText("E8 ?? ?? ?? ?? 48 ?? ?? ?? ?? ?? ?? 83 BC 98", out var ptr))
            {
                return (VFXList*)(*(long*)(((delegate* unmanaged[Stdcall]<long>)ptr)() + 0xD30) + 0x2000);
            }
            return null;
        }
        */
        public IEnumerator<VFXListData> GetEnumerator()
        {
            foreach (var vfx in ListSpan.ToArray())
            {
                if (vfx.IsValid())
                    yield return vfx;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    [StructLayout(LayoutKind.Explicit, Size = 0x1E0)]
    public unsafe struct OmenVFXHandle
    {
        [FieldOffset(0x1b8)] public Apricot* Apricot;
        public bool HasInit() => Apricot != null && Apricot->HasInit();
        public void SetColor(Vector4 color)
        {
            /*
            var data = Apricot->GetVFXListData();
            if (data != null)
            {
                var pControl = data->pControl;
                if (pControl != null)
                {
                    pControl->SetColor(color);
                }
            }
            */
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 0xC0)]//v5 = MemoryManager_Alloc(0xC0i64, 0i64, v4, 16i64)
    public unsafe struct Apricot
    {
        //ctor(this,vfxHandle,pCompiled) 48 89 5c 24 ? 48 89 74 24 ? 48 89 7c 24 ? 55 48 ? ? 48 ? ? ? ? ? ? 33
        //Init(this) e8 * * * * 48 ? ? ? f6 81 ? ? ? ? ? 74
        [FieldOffset(0x00)] public void* vtbl;
        [FieldOffset(0x08)] public IntPtr pCompiled; // ResourceHandle = Client::System::Resource::ResourceManager_GetResourceAsync(...)+0xC0
        [FieldOffset(0x10)] public IntPtr Ref; //指向存放自己的变量的地址
        [FieldOffset(0x18)] public IntPtr OmenVFXHandle;
        [FieldOffset(0x20)] public fixed float Matrix[4 * 4];
        [FieldOffset(0x50)] public Vector3 Position;
        [FieldOffset(0x60)] public long Id;
        [FieldOffset(0x60)] public uint CRC;
        [FieldOffset(0x64)] public uint Index;
        [FieldOffset(0x68)] public float Unkf1;
        [FieldOffset(0x6C)] public float Unkf2;
        [FieldOffset(0xB0)] public int Time;
        [FieldOffset(0xB4)] public byte Unk1;
        [FieldOffset(0xB5)] public byte Unk2;
        [FieldOffset(0xB7)] public byte NeedInit;//可能是flag
        [FieldOffset(0xA0)] public Vector4 ColorScale;
        public bool HasInit() => (NeedInit & 1) == 0;
        /*
        public VFXListData* GetVFXListData()
        {
            var instance = VFXList.Instance();
            var res = instance->GetVFXListDataByIndex((int)Index);
            if (res->Id == Id && res->VFXHandle == OmenVFXHandle && res->pCompiled != IntPtr.Zero)
                return res;
            return null;
        }
        */
    }

    [StructLayout(LayoutKind.Explicit, Size = 0x500)]//未知大小
    public unsafe struct OmenControl
    {
        //vtbl + 0x28 GetPos
        //vtbl + 0x30 GetTransform
        [FieldOffset(0x00)] public byte* vtbl;
        [FieldOffset(0x2C)] public Vector3 Pos;
        [FieldOffset(0x1D4)] public float Life;
        [FieldOffset(0x1DC)] public float Speed;
        [FieldOffset(0x1F0)] public uint DestroyFlag;
        [FieldOffset(0x208)] public IntPtr pCompiled;
        [FieldOffset(0x228)] public OmenData* Data;
        [FieldOffset(0x474)] public uint Index;
        public void SetColor(Vector4 color)
        {
            Data->Color = color;
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 0x500)] //未知大小
    public unsafe struct OmenData
    {
        [FieldOffset(0x30)] public Vector4 Color;
        //[FieldOffset(0x40)] public Transform Transform;
    }

    [StructLayout(LayoutKind.Explicit, Size = 0x88)]
    public unsafe struct VFXListData
    {
        [FieldOffset(0x00)] public fixed float Matrix[12];
        [FieldOffset(0x30)] public OmenControl* pControl;//可能是控制相关
        [FieldOffset(0x38)] public IntPtr pCompiled;//解析后的VFX ResourceHandle+0xC0
        [FieldOffset(0x40)] public IntPtr VFXHandle;
        [FieldOffset(0x48)] public IntPtr pUnk2;//未知
        [FieldOffset(0x52)] public byte Unk1_52;//remove flag
        [FieldOffset(0x53)] public byte Unk1_53;//remove flag
        [FieldOffset(0x56)] public ushort Destroy;//remove flag
        [FieldOffset(0x58)] public ushort Destroy2;//remove flag
        [FieldOffset(0x60)] public long Id;
        [FieldOffset(0x60)] public uint CRC;
        [FieldOffset(0x64)] public uint Index;
        [FieldOffset(0x68)] public Vector4 ColorScale;
        [FieldOffset(0x78)] public fixed float UnkScale[4];
        public bool IsValid() => (IntPtr)pControl != IntPtr.Zero && pCompiled != IntPtr.Zero && VFXHandle != IntPtr.Zero;
    }
}
