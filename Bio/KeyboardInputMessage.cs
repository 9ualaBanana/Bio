//MIT License

//Copyright (c) 2022 GualaBanana

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System.Runtime.InteropServices;
using Bio.Win32;

namespace Bio;

/// <summary>
/// The managed wrapper for a low-level keyboard input message.
/// </summary>
public class KeyboardInputMessage
{
    /// <summary>
    /// The indentifier of the keyboard message specifying the system action.
    /// </summary>
    public WM WM;
    /// <summary>
    /// The structure containing the keyboard input data.
    /// </summary>
    public KBDLLHOOKSTRUCT KBDLLHOOKSTRUCT;

    /// <summary>
    /// Instantiates the wrapper for a low-level keyboard input message.
    /// </summary>
    /// <param name="WM"><inheritdoc cref="WM"/></param>
    /// <param name="KBDLLHOOKSTRUCT"><inheritdoc cref="KBDLLHOOKSTRUCT"/></param>
    public KeyboardInputMessage(IntPtr WM, IntPtr KBDLLHOOKSTRUCT) 
        : this((WM)WM, Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(KBDLLHOOKSTRUCT))
    {
    }

    KeyboardInputMessage(WM WM, KBDLLHOOKSTRUCT KBDLLHOOKSTRUCT)
    {
        this.WM = WM;
        this.KBDLLHOOKSTRUCT = KBDLLHOOKSTRUCT;
    }
}
