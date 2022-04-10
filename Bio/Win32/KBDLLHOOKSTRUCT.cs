using System.Runtime.InteropServices;

namespace Bio.Win32;

[StructLayout(LayoutKind.Sequential)]
public struct KBDLLHOOKSTRUCT
{
    public uint vkCode;
    public uint scanCode;
    public KBDLLHOOKSTRUCTFlags flags;
    public uint time;
    public UIntPtr dwExtraInfo;
}
