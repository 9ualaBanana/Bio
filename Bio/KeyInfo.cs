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

namespace Bio;

/// <summary>
/// Represents a key and modifier keys.
/// </summary>
public struct KeyInfo
{
    /// <summary>
    /// Virtual-key code representing the instance.
    /// </summary>
    public VK VK;
    /// <summary>
    /// <see cref="char"/> representing the instance.
    /// </summary>
    public char KeyChar;
    /// <summary>
    /// <see cref="ConsoleKey"/> representing the instance.
    /// </summary>
    public ConsoleKey ConsoleKey;
    /// <summary>
    /// Distinguishes left and right modifiers and treats them as different keys.
    /// </summary>
    public ModifierKeys ModifierKeys;
    /// <summary>
    /// Does not distinguish left and right modifiers and treats them as the same keys.
    /// </summary>
    public ConsoleModifiers ConsoleModifiers
    {
        get
        {
            ConsoleModifiers consoleModifiers = 0;
            if (ModifierKeys.HasFlag(ModifierKeys.ShiftLeft) || ModifierKeys.HasFlag(ModifierKeys.ShiftRight))
                consoleModifiers |= ConsoleModifiers.Shift;
            if (ModifierKeys.HasFlag(ModifierKeys.ControlLeft) || ModifierKeys.HasFlag(ModifierKeys.ControlRight))
                consoleModifiers |= ConsoleModifiers.Control;
            if (ModifierKeys.HasFlag(ModifierKeys.AltLeft) || ModifierKeys.HasFlag(ModifierKeys.AltRight))
                consoleModifiers |= ConsoleModifiers.Alt;
            return consoleModifiers;
        }
    }

    /// <summary>
    /// <inheritdoc cref="KeyInfo(VK, ModifierKeys, IntPtr)"/>
    /// </summary>
    /// <param name="vkCode">The virtual-key code representing the key.</param>
    /// <param name="modifierKeys">The modifier keys involved.</param>
    public KeyInfo(VK vkCode, ModifierKeys modifierKeys) : this((uint)vkCode, modifierKeys)
    {
    }

    /// <summary>
    /// <inheritdoc cref="KeyInfo(VK, ModifierKeys, IntPtr)"/>
    /// </summary>
    /// <param name="vkCode">The virtual-key code representing the key.</param>
    /// <param name="modifierKeys">The modifier keys involved.</param>
    /// <param name="inputLocaleIdentifier">The input locale identifier to use for
    /// translating <paramref name="vkCode"/> to <see cref="char"/>.</param>
    public KeyInfo(VK vkCode, ModifierKeys modifierKeys, IntPtr inputLocaleIdentifier)
        : this((uint)vkCode, modifierKeys, inputLocaleIdentifier)
    {
    }

    /// <summary>
    /// <inheritdoc cref="KeyInfo(VK, ModifierKeys, IntPtr)"/>
    /// </summary>
    /// <param name="vkCode">The virtual-key code representing the key.</param>
    /// <param name="modifierKeys">The modifier keys involved.</param>
    public KeyInfo(uint vkCode, ModifierKeys modifierKeys) : this(vkCode, modifierKeys, User32.GetKeyboardLayout(0))
    {
    }

    // It should take into account the modifiers that affect a case of a character.
    /// <summary>
    /// Instantiates an object containing the information about a key along with modifier keys.
    /// </summary>
    /// <param name="vkCode">The virtual-key code representing the key.</param>
    /// <param name="modifierKeys">The modifier keys involved.</param>
    /// <param name="inputLocaleIdentifier">The input locale identifier to use for
    /// translating <paramref name="vkCode"/> to <see cref="char"/>.</param>
    public KeyInfo(uint vkCode, ModifierKeys modifierKeys, IntPtr inputLocaleIdentifier)
    {
        VK = (VK)vkCode;
        KeyChar = GetKeyChar(vkCode, modifierKeys, inputLocaleIdentifier);
        ConsoleKey = GetConsoleKey(vkCode);
        ModifierKeys = modifierKeys;
    }

    static char GetKeyChar(uint vkCode, ModifierKeys modifierKeys, IntPtr inputLocaleIdentifier)
    {
        const uint MAPVK_VK_TO_CHAR = 2;
        var keyChar = (char)User32.MapVirtualKeyEx(vkCode, MAPVK_VK_TO_CHAR, inputLocaleIdentifier);
        keyChar = char.ToLower(keyChar);
        if (CapsLockToggled) ToggleCase(ref keyChar);
        if (modifierKeys.HasFlag(ModifierKeys.ShiftLeft)
            || modifierKeys.HasFlag(ModifierKeys.ShiftRight)) ToggleCase(ref keyChar);
        return keyChar;
    }

    static internal bool CapsLockToggled => (User32.GetKeyState(VK.CAPITAL) & 0x0001) != 0;

    static void ToggleCase(ref char keyChar)
    {
        keyChar = char.IsLower(keyChar) ? char.ToUpper(keyChar) : char.ToLower(keyChar);
    }

    static ConsoleKey GetConsoleKey(uint vkCode)
    {
        // `vkCode` must be int because `IsDefined` requires it to be of enum's underlying type.
        if (Enum.IsDefined(typeof(ConsoleKey), (int)vkCode))
            return Enum.Parse<ConsoleKey>(vkCode.ToString());
        return default;
    }

    /// <summary>
    /// Checks if <paramref name="vkCode"/> represents a modifier key.
    /// </summary>
    /// <param name="vkCode">The virtual-key code of the potential modifier.</param>
    /// <returns><c>true</c> if <paramref name="vkCode"/> represents a key modifier; <c>false</c> otherwise.</returns>
    public static bool IsModifier(VK vkCode)
    {
        return vkCode >= VK.LSHIFT && vkCode <= VK.RMENU;
    }
}
