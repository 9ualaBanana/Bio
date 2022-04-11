using System.Runtime.InteropServices;
using Bio.Win32;

namespace Bio;

/// <summary>
/// The managed wrapper for a low-level keyboard input message.
/// </summary>
public class KeyboardInputMessageEventArgs : EventArgs
{
    public WM WM;
    public KEYBDINPUT KBDLLHOOKSTRUCT;

    /// <summary>
    /// Instantiates the wrapper for a low-level keyboard input message.
    /// </summary>
    /// <param name="wParam">The identifier of the keyboard message.</param>
    /// <param name="lParam">The pointer to the structure containing the keyboard input data.</param>
    public KeyboardInputMessageEventArgs(IntPtr wParam, IntPtr lParam) 
        : this((WM)wParam, Marshal.PtrToStructure<KEYBDINPUT>(lParam))
    {
    }

    KeyboardInputMessageEventArgs(WM wParam, KEYBDINPUT lParam)
    {
        WM = wParam;
        KBDLLHOOKSTRUCT = lParam;
    }
}
