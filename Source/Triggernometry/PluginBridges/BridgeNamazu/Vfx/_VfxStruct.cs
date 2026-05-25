using System;
using System.Numerics;

namespace Triggernometry.PluginBridges.BridgeNamazu.Vfx
{
    public readonly struct VfxObject
    {
        public readonly IntPtr Address;

        public VfxObject(IntPtr address)
        {
            Address = address;
        }

        private static GreyMagicExternalProcessMemory Memory => BridgeNamazu.NamazuPlugin.Memory;

        private T Read<T>(int offset) where T : struct
        {
            return Memory.Read<T>(Address + offset);
        }

        private void Write<T>(int offset, T value) where T : struct
        {
            Memory.Write(Address + offset, value);
        }

        public byte Flags
        {
            get => Read<byte>(0x38);
            set => Write(0x38, value);
        }

        public Vector3 Position
        {
            get => Read<Vector3>(0x50);
            set => Write(0x50, value);
        }

        public Quaternion Rotation
        {
            get => Read<Quaternion>(0x60);
            set => Write(0x60, value);
        }

        public Vector3 Scale
        {
            get => Read<Vector3>(0x70);
            set => Write(0x70, value);
        }

        public IntPtr Unk80
        {
            get => Read<IntPtr>(0x80);
            set => Write(0x80, value);
        }

        public byte State88
        {
            get => Read<byte>(0x88);
            set => Write(0x88, value);
        }

        public byte State89
        {
            get => Read<byte>(0x89);
            set => Write(0x89, value);
        }

        public uint ActorCaster
        {
            get => Read<uint>(0x128);
            set => Write(0x128, value);
        }

        public uint ActorTarget
        {
            get => Read<uint>(0x130);
            set => Write(0x130, value);
        }

        public uint StaticCaster
        {
            get => Read<uint>(0x1B8);
            set => Write(0x1B8, value);
        }

        public uint StaticTarget
        {
            get => Read<uint>(0x1C0);
            set => Write(0x1C0, value);
        }

        public float Speed
        {
            get => Read<float>(0x250);
            set => Write(0x250, value);
        }

        public float FadeOutTimer
        {
            get => Read<float>(0x258);
            set => Write(0x258, value);
        }

        public Vector4 Color
        {
            get => Read<Vector4>(0x260);
            set => Write(0x260, value);
        }

        public IntPtr ApricotPtr
        {
            get => Read<IntPtr>(0x2A0);
            set => Write(0x2A0, value);
        }
    }
}