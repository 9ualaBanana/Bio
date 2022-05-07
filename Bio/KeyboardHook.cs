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

using System.Diagnostics;
using Bio.Win32;

namespace Bio;

/// <summary>
/// An abstraction over a low-level keyboard hook running in a background.
/// </summary>
public class KeyboardHook : IDisposable
{
    bool _muted;
    IntPtr _hHook;
    readonly CancellationTokenSource _cts;
    readonly Task _hooking;
    /// <summary>
    /// Ensures the call to <see cref="User32.SetWindowsHookEx(WH, HOOKPROC, IntPtr, uint)"/>
    /// was made before returning from <see cref="Set(bool, bool)"/>.
    /// </summary>
    readonly ManualResetEventSlim _win32SetHookWasCalled = new();

    /// <summary>
    /// The pointer to the hook.
    /// </summary>
    public IntPtr Handle => _hHook;
    
    /// <summary>
    /// Checks the hook state.
    /// </summary>
    public bool IsSet => _hHook != IntPtr.Zero && !_cts.IsCancellationRequested;

    /// <summary>
    /// The event raised upon detecting low-level keyboard input of the specefied type.
    /// </summary>
    /// <remarks>
    /// Use <see cref="Mute"/>/<see cref="Unmute"/> to control the behavior of this event.
    /// </remarks>
    public event EventHandler<KeyboardInputMessage>? KeyDown;
    /// <summary>
    /// <inheritdoc cref="KeyDown"/>
    /// </summary>
    /// <remarks>
    /// <inheritdoc cref="KeyDown"/>
    /// </remarks>
    public event EventHandler<KeyboardInputMessage>? KeyUp;

    /// <summary>
    /// Instantiates an abstraction over a low-level keyboard hook.
    /// </summary>
    public KeyboardHook()
    {
        _cts = new();
        _hooking = new Task(Enable, _cts.Token, TaskCreationOptions.LongRunning);
    }

    /// <summary>
    /// Wraps the unmanaged event to the managed one and handles it.
    /// </summary>
    IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (!_muted && nCode >= 0) RaiseCorrespondingEvent(wParam, lParam);
        return User32.CallNextHookEx(_hHook, nCode, wParam, lParam);
    }

    void RaiseCorrespondingEvent(IntPtr wParam, IntPtr lParam)
    {
        var keyboardInputMessage = new KeyboardInputMessage(wParam, lParam);

        if (keyboardInputMessage.WM == WM.KEYDOWN || keyboardInputMessage.WM == WM.SYSKEYDOWN)
        {
            KeyDown?.Invoke(this, keyboardInputMessage);
        }
        else
        {
            KeyUp?.Invoke(this, keyboardInputMessage);
        }
    }

    /// <summary>
    /// Wrapper for <see cref="Kernel32.GetModuleHandle(string)"/>.
    /// </summary>
    /// <param name="module">The module for which the handle needs to be obtained.</param>
    /// <returns>The pointer to the module.</returns>
    static IntPtr GetModuleHandle(ProcessModule? module)
    {
        return Kernel32.GetModuleHandle(module?.ModuleName!);
    }

    /// <summary>
    /// Sets the low-level keyboard hook.
    /// </summary>
    /// <remarks>
    /// Blocks the thread on which it is set.
    /// </remarks>
    /// <param name="muted">The state of the events raised by the hook.</param>
    /// <param name="asynchronously">Defines if the hook should be set on another thread.</param>
    public void Set(bool muted = false, bool asynchronously = true)
    {
        _muted = muted;
        if (asynchronously) _hooking.Start();
        else _hooking.RunSynchronously();
        _win32SetHookWasCalled.Wait();
    }

    /// <summary>
    /// Sets the hook and starts a message loop.
    /// </summary>
    /// <remarks>
    /// Hook setting and starting a message loop must be done on the same thread.
    /// </remarks>
    void Enable()
    {
        SetHook();
        _win32SetHookWasCalled.Set();
        if (!CurrentThreadHasMessageLoop) StartMessageLoop();
    }
    void SetHook()
    {
        using var hookProcModule = Process.GetCurrentProcess().MainModule;
        _hHook = User32.SetWindowsHookEx(WH.KEYBOARD_LL, LowLevelKeyboardProc, GetModuleHandle(hookProcModule), 0);
    }
    /// <summary>
    /// Blocks the thread by indefinitely polling the system messages.
    /// </summary>
    /// <remarks>
    /// Must be called on the same thread where the hook was set for the system to be able to send messages
    /// to that thread, which is a requirement for the hook to work, because the call is made only if
    /// the thread where the hook was set has a receiver for those messages, i.e., a message loop.
    /// </remarks>
    static void StartMessageLoop()
    {
        User32.GetMessage(out _, default, default, default);
    }

    /// <summary>
    /// Checks if the current thread has a message loop.
    /// </summary>
    /// <remarks>
    /// Thread must have a running message loop for a hook callback to be called.
    /// </remarks>
    static bool CurrentThreadHasMessageLoop => User32.PostThreadMessage(
        (uint)Environment.CurrentManagedThreadId,
        default, default, default
        );

    /// <summary>
    /// Mutes <see cref="KeyDown"/> and <see cref="KeyUp"/> events.
    /// </summary>
    /// <remarks>
    /// Does not affect the hook in any way.
    /// </remarks>
    public void Mute()
    {
        if (!_muted) ToggleEvents();
    }

    /// <summary>
    /// Unmutes <see cref="KeyDown"/> and <see cref="KeyUp"/> events.
    /// </summary>
    /// <remarks>
    /// <inheritdoc cref="Mute"/>
    /// </remarks>
    public void Unmute()
    {
        if (_muted) ToggleEvents();
    }

    void ToggleEvents() => _muted = !_muted;

    /// <summary>
    /// An alias for <see cref="Dispose"/>.
    /// </summary>
    public void Remove() => Dispose();

    /// <inheritdoc/>
    public void Dispose()
    {
        GC.SuppressFinalize(this);

        if (!_cts.IsCancellationRequested) _cts.Cancel();
        _cts.Dispose();

        User32.UnhookWindowsHookEx(_hHook);
        _hHook = IntPtr.Zero;
    }
}
