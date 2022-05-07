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
/// Monitors keyboard input.
/// </summary>
public class KeySpy : IDisposable
{
    readonly KeyboardHook _hook;
    bool _muted;

    /// <summary>
    /// Indicates the status.
    /// </summary>
    public bool IsActive => _hook.IsSet;

    /// <summary>
    /// Raised when keyboard input is detected.
    /// </summary>
    public event EventHandler<KeyInfo>? InputDetected;

    /// <summary>
    /// Instantiates <see cref="KeySpy"/> that monitors input from the keyboard.
    /// </summary>
    public KeySpy()
    {
        _hook = new KeyboardHook();
        _hook.KeyDown += OnKeyDown;
    }

    void OnKeyDown(object? sender, KeyboardInputMessage e)
    {
        var vkCode = e.KBDLLHOOKSTRUCT.vkCode;
        var modifierKeys = GetPressedModifierKeys(vkCode);
        var keyInfo = new KeyInfo(vkCode, modifierKeys);

        if (!_muted) InputDetected?.Invoke(this, keyInfo);
    }

    static ModifierKeys GetPressedModifierKeys(VK vkCode)
    {
        ModifierKeys modifierKeys = default;
        // If the pressed key is a modifier itself, it must be added too.
        if (KeyInfo.IsModifier(vkCode)) modifierKeys |= vkCode.TranslateWith(VKToModifierKey);
        for (var modifierKeyVkCode = VK.LSHIFT; modifierKeyVkCode <= VK.RMENU; modifierKeyVkCode++)
        {
            if (IsPressed(User32.GetAsyncKeyState(modifierKeyVkCode)))
                modifierKeys |= modifierKeyVkCode.TranslateWith(VKToModifierKey);
        }
        return modifierKeys;
    }

    static bool IsPressed(int asyncKeyState) => (asyncKeyState & 0x8000) != 0;

    static ModifierKeys VKToModifierKey(VK vkCode) => vkCode switch
    {
        VK.LSHIFT => ModifierKeys.ShiftLeft,
        VK.RSHIFT => ModifierKeys.ShiftRight,
        VK.LCONTROL => ModifierKeys.ControlLeft,
        VK.RCONTROL => ModifierKeys.ControlRight,
        VK.LMENU => ModifierKeys.AltLeft,
        VK.RMENU => ModifierKeys.AltRight,
        _ => default,
    };

    /// <summary>
    /// Start monitoring keyboard input.
    /// </summary>
    /// <remarks>
    /// Blocks the thread on which it is activated.
    /// </remarks>
    /// <param name="muted">Specifies the state of <see cref="InputDetected"/> events.</param>
    /// <param name="asynchronously">Defines if it should be activated on another thread.</param>
    public void Activate(bool muted = false, bool asynchronously = true)
    {
        _hook.Set(muted , asynchronously);
    }

    /// <summary>
    /// An alias for <see cref="Dispose"/>.
    /// </summary>
    public void Deactivate() => Dispose();

    /// <summary>
    /// Mutes <see cref="InputDetected"/> events.
    /// </summary>
    public void Mute()
    {
        if (!_muted) ToggleEvents();
    }

    /// <summary>
    /// Unmutes <see cref="InputDetected"/> events.
    /// </summary>
    public void Unmute()
    {
        if (_muted) ToggleEvents();
    }

    void ToggleEvents() => _muted = !_muted;

    /// <inheritdoc/>
    public void Dispose()
    {
        GC.SuppressFinalize(this);

        _hook?.Dispose();
    }
}
