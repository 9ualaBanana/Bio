using System.Runtime.InteropServices;

namespace Bio.Win32;

[StructLayout(LayoutKind.Explicit)]
internal struct InputUnion
{
    [FieldOffset(0)]
    internal MOUSEINPUT mi;
    [FieldOffset(0)]
    internal KEYBDINPUT ki;
    [FieldOffset(0)]
    internal HARDWAREINPUT hi;
}
