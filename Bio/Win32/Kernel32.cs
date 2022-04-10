using System.Runtime.InteropServices;

namespace Bio.Win32;

internal class Kernel32
{
    [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
    internal extern static IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("Kernel32.dll")]
    internal extern static int GetLastError();
}
