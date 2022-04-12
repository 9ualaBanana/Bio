using System.Runtime.InteropServices;

namespace Bio.Win32;

[StructLayout(LayoutKind.Sequential)]
internal struct HARDWAREINPUT
{
    internal int uMsg;
    internal short wParamL;
    internal short wParamH;
}
