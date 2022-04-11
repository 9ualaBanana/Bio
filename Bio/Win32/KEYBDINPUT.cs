using System.Runtime.InteropServices;

namespace Bio.Win32;

[StructLayout(LayoutKind.Sequential)]
public struct KEYBDINPUT
{
    public VirtualKey vkCode;
    public ScanCode scanCode;
    public KBDLLHOOKSTRUCTF flags;
    public uint time;
    public UIntPtr dwExtraInfo;
}
