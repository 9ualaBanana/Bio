using System.Diagnostics;
using Bio.Win32;

namespace Bio;

/// <summary>
/// An abstraction over a low-level keyboard input hook running in a background.
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
    public bool IsSet => _hHook != IntPtr.Zero;

    /// <summary>
    /// The event raised upon receiving low-level keyboard input.
    /// </summary>
    /// <remarks>
    /// Use <see cref="Mute"/>/<see cref="Unmute"/> to control the behavior of this event.
    /// </remarks>
    public event KeyboardInputMessageEventHandler? OnInput;

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
        if (!_muted && nCode >= 0) OnInput?.Invoke(this, new(wParam, lParam));
        return User32.CallNextHookEx(Handle, nCode, wParam, lParam);
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
        GetMessage();
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
    static void GetMessage()
    {
        User32.GetMessage(out _, IntPtr.Zero, default, default);
    }

    /// <summary>
    /// Mutes <see cref="OnInput"/> event.
    /// </summary>
    /// <remarks>
    /// Does not affect the hook in any way.
    /// </remarks>
    public void Mute()
    {
        if (!_muted) ToggleEvents();
    }

    /// <summary>
    /// Unmutes <see cref="OnInput"/> event.
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

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        _cts.Cancel();
        _cts.Dispose();

        User32.UnhookWindowsHookEx(_hHook);
        _hHook = IntPtr.Zero;
    }
}
