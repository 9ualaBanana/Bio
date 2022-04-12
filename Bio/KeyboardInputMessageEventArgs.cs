using System.Runtime.InteropServices;
using Bio.Win32;

namespace Bio;

/// <summary>
/// The managed wrapper for a low-level keyboard input message.
/// </summary>
public class KeyboardInputMessageEventArgs : EventArgs
{
    /// <summary>
    /// The indentifier of the keyboard message specifying the system action.
    /// </summary>
    public WM WM;
    /// <summary>
    /// The pointer to the structure containing the keyboard input data.
    /// </summary>
    public KBDLLHOOKSTRUCT KBDLLHOOKSTRUCT;

    /// <summary>
    /// Instantiates the wrapper for a low-level keyboard input message.
    /// </summary>
    /// <param name="WM"><inheritdoc cref="WM"/></param>
    /// <param name="KBDLLHOOKSTRUCT"><inheritdoc cref="KBDLLHOOKSTRUCT"/></param>
    public KeyboardInputMessageEventArgs(IntPtr WM, IntPtr KBDLLHOOKSTRUCT) 
        : this((WM)WM, Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(KBDLLHOOKSTRUCT))
    {
    }

    KeyboardInputMessageEventArgs(WM WM, KBDLLHOOKSTRUCT KBDLLHOOKSTRUCT)
    {
        this.WM = WM;
        this.KBDLLHOOKSTRUCT = KBDLLHOOKSTRUCT;
    }
}
