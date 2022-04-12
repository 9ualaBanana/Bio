using System.Runtime.InteropServices;

namespace Bio.Win32;

[StructLayout(LayoutKind.Sequential)]
public struct INPUT
{
    internal uint type;
    internal InputUnion data;
    internal static int Size
    {
        get { return Marshal.SizeOf(typeof(INPUT)); }
    }
}
