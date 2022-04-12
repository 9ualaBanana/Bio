using System.Runtime.InteropServices;

namespace Bio.Win32;

[StructLayout(LayoutKind.Sequential)]
public struct KBDLLHOOKSTRUCT
{
    public VK vkCode;
    public SC scanCode;
    public KBDLLHOOKSTRUCTF flags;
    public uint time;
    public UIntPtr dwExtraInfo;
}
