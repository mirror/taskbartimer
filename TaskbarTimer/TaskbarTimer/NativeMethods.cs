using System;
using System.Runtime.InteropServices;

namespace TaskbarTimer {
    /// <summary>
    /// Necessary native methods to import 
    /// </summary>
    internal static class NativeMethods {
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern bool FlashWindow(IntPtr hwnd, bool bInvert);

        [DllImport("winmm.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        public static extern bool PlaySound(string pszSound, IntPtr hMod, PlaySoundFlags sf);
    }
}
