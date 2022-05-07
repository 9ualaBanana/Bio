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

using Bio.Win32;
using System.Runtime.InteropServices;

namespace Bio;

/// <summary>
/// Provides an interface for synthesizing keyboard input events.
/// </summary>
public class KeyboardInput
{
    const uint INPUT_KEYBOARD = 1;
    const uint KEYEVENTF_KEYUP = 0x0002;

    /// <summary>
    /// Synthesizes both, <see cref="WM.KEYDOWN"/> and <see cref="WM.KEYUP"/> messages.
    /// </summary>
    /// <param name="vkCodes">The virtual-key codes for which input should be synthesized.</param>
    /// <returns>The number of keyboard input events successfully injected into the input stream.</returns>
    public static uint SynthesizePress(params VK[] vkCodes)
    {
        var input = new INPUT[vkCodes.Length * 2];
        for (int i = 1, vkCode = 0; vkCode < vkCodes.Length; i += 2, vkCode++)
        {
            input[i - 1] = Simulate(vkCodes[vkCode]);
            input[i] = Simulate(vkCodes[vkCode], KEYEVENTF_KEYUP);
        }
        return Synthesize(input);
    }

    /// <summary>
    /// Wrapper for <see cref="User32.SendInput(uint, INPUT[], int)"/>.
    /// </summary>
    /// <param name="message">The type of keyboard input event that should be synthesized.</param>
    /// <param name="vkCodes">The virtual-key codes for which input should be synthesized.</param>
    /// <returns>The number of keyboard input events successfully injected into the input stream.</returns>
    public static uint Synthesize(WM message, params VK[] vkCodes)
    {
        var input = new INPUT[vkCodes.Length];
        var dwFlags = message == WM.KEYUP || message == WM.SYSKEYUP ? KEYEVENTF_KEYUP : 0;
        for (int i = 0; i < vkCodes.Length; i++)
        {
            input[i] = Simulate(vkCodes[i], dwFlags);
        }
        return Synthesize(input);
    }

    static INPUT Simulate(VK vkCode, uint dwFlags = default)
    {
        var input = new INPUT() { type = INPUT_KEYBOARD };
        input.data.ki.wVk = vkCode;
        input.data.ki.dwFlags = dwFlags;
        return input;
    }

    static uint Synthesize(INPUT[] input)
    {
        return User32.SendInput((uint)input.Length, input, Marshal.SizeOf<INPUT>());
    }
}
