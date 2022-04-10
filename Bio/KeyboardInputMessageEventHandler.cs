namespace Bio;

/// <summary>
/// The event handler for low-level keyboard input messages intercepted by a hook.
/// </summary>
/// <param name="sender">The hook that intercepted the message.</param>
/// <param name="e">The low-level keyboard input message intercepted by the hook.</param>
public delegate void KeyboardInputMessageEventHandler(object? sender, KeyboardInputMessageEventArgs e);
