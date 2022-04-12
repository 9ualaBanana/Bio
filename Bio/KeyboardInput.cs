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

    readonly INPUT[] _input;

    /// <summary>
    /// Simulates keyboard input events (press and release) that can be injected into input stream.
    /// </summary>
    /// <param name="vkCodes"><inheritdoc cref="KeyboardInput(bool, VK[])"/></param>
    public KeyboardInput(params VK[] vkCodes) : this(true, vkCodes)
    {
    }

    /// <summary>
    /// Simulates keyboard input events that can be injected into input stream.
    /// </summary>
    /// <param name="keyUp">Specifies if key release event should be included along with key press event.</param>
    /// <param name="vkCodes">The virtual-key codes to be turned to input events.</param>
    public KeyboardInput(bool keyUp, params VK[] vkCodes)
    {
        _input = Simulate(keyUp, vkCodes);
    }

    /// <summary>
    /// <inheritdoc cref="KeyboardInput(bool, VK[])"/>
    /// </summary>
    /// <param name="keyUp"><inheritdoc cref="KeyboardInput(bool, VK[])"/></param>
    /// <param name="vkCodes"><inheritdoc cref="KeyboardInput(bool, VK[])"/></param>
    public static INPUT[] Simulate(bool keyUp = true, params VK[] vkCodes)
    {
        var input = new INPUT[keyUp ? vkCodes.Length * 2 : vkCodes.Length];
        if (keyUp)
        {
            for (int i = 1; i < vkCodes.Length; i += 2)
            {
                input[i] = Simulate(vkCodes[i], KEYEVENTF_KEYUP);
                input[i - 1] = Simulate(vkCodes[i]);
            }
        }
        else
        {
            for (int i = 0; i < vkCodes.Length; i++)
            {
                input[i] = Simulate(vkCodes[i]);
            }
        }
        return input;
    }

    static INPUT Simulate(VK vkCode, uint dwFlags = default)
    {
        var input = new INPUT() { type = INPUT_KEYBOARD };
        input.data.ki.wVk = vkCode;
        input.data.ki.dwFlags = dwFlags;
        return input;
    }

    /// <summary>
    /// Wrapper for <see cref="User32.SendInput(uint, INPUT[], int)"/>.
    /// </summary>
    /// <returns>The number of keystroke events successfully injected into the input stream.</returns>
    public uint Synthesize()
    {
        return Synthesize(_input);
    }

    /// <summary>
    /// <inheritdoc cref="Synthesize()"/>
    /// </summary>
    /// <param name="input">The keyboard input events to be injected into the input stream.</param>
    /// <returns><inheritdoc cref="Synthesize()"/></returns>
    public static uint Synthesize(INPUT[] input)
    {
        return User32.SendInput((uint)input.Length, input, Marshal.SizeOf<INPUT>());
    }
}
