using System.Runtime.InteropServices;

namespace Bio.Win32;

[StructLayout(LayoutKind.Sequential)]
internal struct KEYBDINPUT
{
    internal VK wVk;
    internal SC wScan;
    internal uint dwFlags;
    internal int time;
    internal UIntPtr dwExtraInfo;
}
