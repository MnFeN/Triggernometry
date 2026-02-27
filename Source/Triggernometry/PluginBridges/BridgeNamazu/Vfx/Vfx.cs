using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Triggernometry.PluginBridges.BridgeNamazu.Modules;

namespace Triggernometry.PluginBridges.BridgeNamazu.Vfx
{
    public abstract class Vfx
    {
        public IntPtr Ptr { get; set; }
        public string Path { get; set; }
        public string Tag { get; set; }
        public bool Removed { get; set; } = false;

        public const string DefaultTag = "Auto";
        public static VfxModule Module => BridgeNamazu.GetModule<VfxModule>();
        public static GreyMagicExternalProcessMemory Memory => BridgeNamazu.NamazuPlugin.Memory;
        public abstract bool TryRemove();

        public void ScheduleRemove(double duration)
        {
            if (duration > 0 && Ptr != IntPtr.Zero)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        await Task.Delay(TimeSpan.FromSeconds(duration)).ConfigureAwait(false);
                        Memory.ExecuteWithLock(() => TryRemove());
                    }
                    catch (Exception ex)
                    {
                        Module.ErrorLog($"[PictoACT] 延迟移除时出错：\n{ex}");
                    }
                });
            }
        }

        public void Update()
        { 
            if (Removed) return;
            Flag |= 0x2;
        }

        public byte Flag
        {
            get => Memory.Read<byte>(Ptr + 0x38);
            set
            {
                if (Removed) return;
                Memory.Write(Ptr + 0x38, value);
            }
        }

        public Vector3 Pos
        {
            get
            {
                var raw = Memory.Read<Vector3>(Ptr + 0x50);
                return new Vector3(raw.X, raw.Z, raw.Y);
            }
            set
            {
                if (Removed) return;
                Memory.Write(Ptr + 0x50, new Vector3(value.X, value.Z, value.Y));
            }
        }

        public float Angle
        {
            get => Angles.X;
            set => Angles = new Vector3(value, 0, 0);
        }

        public Vector3 Angles
        {
            get
            {
                var raw = Memory.Read<Vector4>(Ptr + 0x60);
                var q = new Quaternion(raw.X, raw.Z, raw.Y, raw.W);

                float yaw, pitch, roll;

                // pitch (x‑axis rotation = θx)
                float sinp = 2f * (q.W * q.X + q.Y * q.Z);
                float cosp = 1f - 2f * (q.X * q.X + q.Y * q.Y);
                pitch = (float)Math.Atan2(sinp, cosp);

                // yaw (y‑axis rotation = θy)
                float siny = 2f * (q.W * q.Y - q.Z * q.X);
                if (Math.Abs(siny) >= 1f)
                    yaw = (float)(Math.PI / 2 * Math.Sign(siny));
                else
                    yaw = (float)Math.Asin(siny);

                // roll (z‑axis rotation = θ)
                float sinr = 2f * (q.W * q.Z + q.X * q.Y);
                float cosr = 1f - 2f * (q.Y * q.Y + q.Z * q.Z);
                roll = (float)Math.Atan2(sinr, cosr);

                return new Vector3(roll, pitch, yaw);
            }
            set
            {
                if (Removed) return;
                var q = Quaternion.CreateFromYawPitchRoll(value.Z, value.Y, value.X); // θy, θx, θ
                Memory.Write(Ptr + 0x60, new Vector4(q.X, q.Z, q.Y, q.W));
            }
        }

        public Vector3 Scales
        {
            get
            {
                var raw = Memory.Read<Vector3>(Ptr + 0x70);
                return new Vector3(raw.X, raw.Z, raw.Y);
            }
            set
            {
                if (Removed) return;
                Memory.Write(Ptr + 0x70, new Vector3(value.X, value.Z, value.Y));
            }
        }

        public uint ActorVfxSource
        {
            get => Memory.Read<uint>(Ptr + 0x128);
            set
            {
                if (Removed) return;
                Memory.Write(Ptr + 0x128, value);
            }
        }

        public uint ActorVfxTarget
        {
            get => Memory.Read<uint>(Ptr + 0x130);
            set
            {
                if (Removed) return;
                Memory.Write(Ptr + 0x130, value);
            }
        }

        public uint StaticVfxSource
        {
            get => Memory.Read<uint>(Ptr + 0x1B8);
            set
            {
                if (Removed) return;
                Memory.Write(Ptr + 0x1B8, value);
            }
        }

        public uint StaticVfxTarget
        {
            get => Memory.Read<uint>(Ptr + 0x1C0);
            set
            {
                if (Removed) return;
                Memory.Write(Ptr + 0x1C0, value);
            }
        }

        public float Speed
        {
            get => Memory.Read<float>(Ptr + 0x250);
            set
            {
                if (Removed) return;
                Memory.Write(Ptr + 0x250, value);
            }
        }

        public Vector4 Color
        {
            get => Memory.Read<Vector4>(Ptr + 0x260);
            set
            {
                if (Removed) return;
                Memory.Write(Ptr + 0x260, value);
            }
        }


    }
}
