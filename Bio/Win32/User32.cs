using System.Runtime.InteropServices;

namespace Bio.Win32;

internal static class User32
{
    [DllImport("User32.dll")]
    internal static extern int GetMessage(out IntPtr lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

    [DllImport("User32.dll")]
    internal extern static IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("User32.dll")]
    internal static extern IntPtr SetWindowsHookEx(WH hookType, HOOKPROC lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("User32.dll")]
    internal extern static bool UnhookWindowsHookEx(IntPtr hhk);
}
