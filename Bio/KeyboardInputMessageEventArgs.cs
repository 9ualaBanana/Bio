using System.Runtime.InteropServices;
using Bio.Win32;

namespace Bio;

/// <summary>
/// The managed wrapper for a low-level keyboard input message.
/// </summary>
public class KeyboardInputMessageEventArgs : EventArgs
{
    public WM WM;
    public KBDLLHOOKSTRUCT KBDLLHOOKSTRUCT;

    /// <summary>
    /// Instntiates the wrapper for a low-level keyboard input message.
    /// </summary>
    /// <param name="wParam">The identifier of the keyboard message.</param>
    /// <param name="lParam">The pointer to the structure containing the keyboard input data.</param>
    public KeyboardInputMessageEventArgs(IntPtr wParam, IntPtr lParam) 
        : this((WM)wParam, Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam))
    {
    }

    KeyboardInputMessageEventArgs(WM wParam, KBDLLHOOKSTRUCT lParam)
    {
        WM = wParam;
        KBDLLHOOKSTRUCT = lParam;
    }
}
