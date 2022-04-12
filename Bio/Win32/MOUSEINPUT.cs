using System.Runtime.InteropServices;

namespace Bio.Win32;

[StructLayout(LayoutKind.Sequential)]
internal struct MOUSEINPUT
{
    internal int dx;
    internal int dy;
    internal int mouseData;
    internal MOUSEEVENTF dwFlags;
    internal uint time;
    internal UIntPtr dwExtraInfo;
}
