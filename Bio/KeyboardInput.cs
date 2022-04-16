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
