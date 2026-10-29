using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Triggernometry.Utilities
{
    public class WindowsUtils
    {

        [Flags]
        public enum MouseEventFlags : uint
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010,
            //XDOWN = 0x00000080,
            //XUP = 0x00000100,
            //WHEEL = 0x00000800,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
        }

        public enum MouseEventDataXButtons : uint
        {
            NONE = 0x00000000,
            XBUTTON1 = 0x00000001,
            XBUTTON2 = 0x00000002,
        }

        const uint WM_KEYUP = 0x101;
        const uint WM_KEYDOWN = 0x100;

        public struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }

        const int SW_UNKNOWN = -1;
        const UInt32 SW_HIDE = 0;
        const UInt32 SW_SHOWNORMAL = 1;
        const UInt32 SW_NORMAL = 1;
        const UInt32 SW_SHOWMINIMIZED = 2;
        const UInt32 SW_SHOWMAXIMIZED = 3;
        const UInt32 SW_MAXIMIZE = 3;
        const UInt32 SW_SHOWNOACTIVATE = 4;
        const UInt32 SW_SHOW = 5;
        const UInt32 SW_MINIMIZE = 6;
        const UInt32 SW_SHOWMINNOACTIVE = 7;
        const UInt32 SW_SHOWNA = 8;
        const UInt32 SW_RESTORE = 9;

        const int SM_CXSCREEN = 0x0;
        const int SM_CYSCREEN = 0x01;

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowTextLength(IntPtr hWnd);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        static extern int GetSystemMetrics(int smIndex);
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);

        public static string GetWindowTextFromHandle(IntPtr hWnd)
        {
            int len = GetWindowTextLength(hWnd);
            if (len > 0)
            {
                var builder = new StringBuilder(len + 1);
                GetWindowText(hWnd, builder, len + 1);
                return builder.ToString();
            }
            return string.Empty;
        }

        public static void SendMouse(MouseEventFlags flags, MouseEventDataXButtons buttons, int x, int y)
        {
            if ((flags & MouseEventFlags.ABSOLUTE) == MouseEventFlags.ABSOLUTE)
            {
                int mx = GetSystemMetrics(SM_CXSCREEN);
                int my = GetSystemMetrics(SM_CYSCREEN);
                x = (int)(65536.0 / mx * x);
                y = (int)(65536.0 / my * y);
            }
            mouse_event((uint)flags, x, y, (uint)buttons, 0);
        }

        public static List<IntPtr> FindWindows(int procid, string titleRegex)
        {
            if (string.IsNullOrWhiteSpace(titleRegex))
            {
                titleRegex = ".*";
            }
            return FindWindows(procid, new Regex(titleRegex));
        }

        public static List<IntPtr> FindWindows(int procid, Regex titleRegex)
        {
            List<IntPtr> wins = new List<IntPtr>();
            EnumWindows((hWnd, lParam) =>
            {
                // 指定进程时先筛选，避免读取和匹配其他进程的窗口标题。
                if (procid > 0)
                {
                    GetWindowThreadProcessId(hWnd, out uint windowProcId);
                    if (windowProcId != procid) return true;
                }
                try
                {
                    if (titleRegex.IsMatch(GetWindowTextFromHandle(hWnd)))
                    {
                        wins.Add(hWnd);
                        // 0 只取枚举顺序中的首个匹配；负数及指定进程保留全部匹配。
                        return procid != 0;
                    }
                }
                catch (Exception)
                {
                }
                return true;
            }, IntPtr.Zero);
            return wins;
        }

        public static void SendKeycode(int procid, string titleRegex, int keycode)
        {
            Keys key = (Keys)keycode;
            List<int> keycodes = new List<int>();

            if (key.HasFlag(Keys.Control))
                keycodes.Add((int)Keys.ControlKey);

            if (key.HasFlag(Keys.Shift))
                keycodes.Add((int)Keys.ShiftKey);

            if (key.HasFlag(Keys.Alt))
                keycodes.Add((int)Keys.Menu);

            keycodes.Add((int)(key & Keys.KeyCode));

            SendKeycodes(procid, titleRegex, keycodes.ToArray());
        }

        public static void SendKeycodes(int procid, string titleRegex, params int[] keycodes)
        {
            List<IntPtr> wins = FindWindows(procid, titleRegex);
            foreach (var keycode in keycodes)
            {
                foreach (IntPtr win in wins)
                {
                    SendMessage(win, WM_KEYDOWN, (IntPtr)keycode, IntPtr.Zero);
                }
            }
            Thread.Sleep(10);
            foreach (var keycode in keycodes.Reverse())
            {
                foreach (IntPtr win in wins)
                {
                    SendMessage(win, WM_KEYUP, (IntPtr)keycode, IntPtr.Zero);
                }
            }
        }

        public static void SendMessageToWindow(int procid, string windowtitle, uint code, IntPtr wparam, IntPtr lparam)
        {
            List<IntPtr> wins = FindWindows(procid, windowtitle);
            foreach (IntPtr win in wins)
            {
                SendMessage(win, code, wparam, lparam);
            }
        }

        public static bool IsInFocus(string windowtitle)
        {
            IntPtr hwnd = FindWindow(null, windowtitle);
            WINDOWPLACEMENT wp = new WINDOWPLACEMENT();
            wp.showCmd = SW_UNKNOWN;
            if (hwnd != IntPtr.Zero)
            {
                wp.length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
                if (GetWindowPlacement(hwnd, ref wp) == true)
                {
                    if (wp.showCmd == SW_SHOWMINIMIZED)
                    {
                        return false;
                    }
                    IntPtr hwnd2 = GetForegroundWindow();
                    return (hwnd == hwnd2);
                }
            }
            return true;
        }

    }
}
