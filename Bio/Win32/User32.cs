using System.Runtime.InteropServices;

namespace Bio.Win32;

internal static class User32
{
    [DllImport("User32.dll")]
    internal static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

    [DllImport("User32.dll")]
    internal static extern IntPtr GetKeyboardLayout(uint idThread);

    [DllImport("User32.dll")]
    internal static extern uint MapVirtualKeyEx(uint uCode, uint uMapType, IntPtr dwhkl);

    [DllImport("User32.dll")]
    internal static extern int GetAsyncKeyState(VK vKeys);

    [DllImport("User32.dll")]
    internal static extern short GetKeyState(VK nVirtKey);

    [DllImport("User32.dll")]
    internal static extern int GetMessage(out IntPtr lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

    [DllImport("User32.dll")]
    internal static extern bool PostThreadMessage(uint threadId, uint msg, UIntPtr wParam, IntPtr lParam);

    [DllImport("User32.dll")]
    internal static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("User32.dll")]
    internal static extern IntPtr SetWindowsHookEx(WH hookType, HOOKPROC lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("User32.dll")]
    internal static extern bool UnhookWindowsHookEx(IntPtr hhk);
}
