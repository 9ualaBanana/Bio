using System.Runtime.InteropServices;

namespace Bio.Win32;

internal class Kernel32
{
    [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
    internal static extern IntPtr GetModuleHandle(string lpModuleName);
}
