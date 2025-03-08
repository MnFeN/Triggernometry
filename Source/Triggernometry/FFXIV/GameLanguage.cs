using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triggernometry.Utilities;

namespace Triggernometry.FFXIV
{
    public static class GameLanguage
    {
        
        private static IntPtr _frameworkPtrPtr = IntPtr.Zero;

        internal static void ScanOffsets()
        {
            var moduleData = Memory.ReadModuleData(Memory.XivProc);
            var offset = Memory.ScanPoint(moduleData, "49 8B C4 48 8B 0D ? ? ? ? 48 8D 15 ? ? ? ? 48 89 05 * * * *", false) // 7.0 CN
                      ?? Memory.ScanPoint(moduleData, "49 8B DC 48 89 1D * * * *", false); // 7.0 global
            if (!offset.HasValue)
            {
                _frameworkPtrPtr = IntPtr.Zero;
                return;
            }
            _frameworkPtrPtr = Memory.XivBaseAddress + offset.Value;
        }

        public static IntPtr FrameworkPtr
        {
            get 
            {
                if (_frameworkPtrPtr == IntPtr.Zero)
                {
                    return IntPtr.Zero;
                }
                return Memory.Read<IntPtr>(Memory.XivProcHandle, _frameworkPtrPtr);
            }
        }

        public static GameLanguageEnum Language
        {
            get
            {
                if (FrameworkPtr == IntPtr.Zero)
                {
                    return GameLanguageEnum.None;
                }
                byte language = Memory.Read<byte>(Memory.XivProcHandle, FrameworkPtr + 0x580);
                return (GameLanguageEnum)language;
            }
        }
    }

    public enum GameLanguageEnum : byte
    {
        JP = 0,
        EN = 1,
        DE = 2,
        FR = 3,
        CN = 4,
        KR = 5,
        None = 0xFF
    }
}
