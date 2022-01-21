namespace InputListener;

/// <summary>
/// Listens for the specified input from the keyboard and notifies its subscribers about it.
/// </summary>
public class KeyboardListener
{
    HashSet<ConsoleKey>? _keysBeingListened;
    // Although, this field contains the default value for ConsoleKey enum, that can be one of the keys being listened,
    // the event will never be raised until the first input from the client is received by CaptureInput() method after StartListening().
    ConsoleKey _lastPressedKey;
    CancellationTokenSource _cancellationTokenSource = new();

    bool _started;
    /// <summary>
    /// Indicates whether the input listener is currently active.
    /// </summary>
    public bool IsListening => _started && !_cancellationTokenSource.IsCancellationRequested;

    /// <summary>
    /// Instantiates <see cref="KeyboardListener"/> that listens for any input from the keyboard.
    /// </summary>
    public KeyboardListener()
    {
        // Functionally almost the same as providing this(params ConsoleKey[]) with no arguments.
        // The only difference is that this one doesn't instantiate redundant collection for _keysBeingListened.
    }

    /// <summary>
    /// Instantiates <see cref="KeyboardListener"/> that listens only for the specified input from the keyboard.
    /// </summary>
    /// <param name="keys">keys to listen to.</param>
    public KeyboardListener(IEnumerable<ConsoleKey> keys)
    {
        _keysBeingListened = new(keys);
    }

    /// <summary>
    /// <inheritdoc cref="KeyboardListener(IEnumerable{ConsoleKey})"/>
    /// </summary>
    /// <param name="keys"><inheritdoc cref="KeyboardListener(IEnumerable{ConsoleKey})"/></param>
    public KeyboardListener(params ConsoleKey[] keys)
    {
        _keysBeingListened = new(keys);
    }

    /// <summary>
    /// Start listening for the specified input.
    /// </summary>
    public void StartListening()
    {
        _started = true;
        StartListeningInBackground();
    }
    void StartListeningInBackground()
    {
        Task.Run(
            NotifyClientsWhenInputOfInterestIsCaptured,
            _cancellationTokenSource.Token
            );
    }
    void NotifyClientsWhenInputOfInterestIsCaptured()
    {
        while (true)
        {
            CaptureInput();
            if (ClientsShouldBeNotified)
            {
                NotifyClients();
            }
        }
    }
    void CaptureInput()
    {
        _lastPressedKey = Console.ReadKey(true).Key;
    }
    bool ClientsShouldBeNotified => _keysBeingListened is null || _keysBeingListened.Contains(_lastPressedKey);
    void NotifyClients()
    {
        KeyBeingListenedPressed?.Invoke(this, _lastPressedKey);
    }

    /// <summary>
    /// Stop listening for the specified input.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This listener can't be started again after calling this method. New instance needs to be created.
    /// </para>
    /// </remarks>
    public void StopListening()
    {
        StopListeningInBackground();
    }
    void StopListeningInBackground()
    {
        if (!IsListening) return;

        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }

    /// <summary>
    /// Attach the <paramref name="receiver"/> to the event raised when the input of interest is captured.
    /// </summary>
    /// <param name="receiver">receiver of the captured input.</param>
    public void AttachInputReceiver(EventHandler<ConsoleKey> receiver) => KeyBeingListenedPressed += receiver;
    /// <summary>
    /// Disattach the <paramref name="receiver"/> from the event raised when the input of interest is captured.
    /// </summary>
    /// <param name="receiver"><inheritdoc cref="AttachInputReceiver(EventHandler{ConsoleKey})"/></param>
    public void DisattachInputReceiver(EventHandler<ConsoleKey> receiver) => KeyBeingListenedPressed -= receiver;
    event EventHandler<ConsoleKey>? KeyBeingListenedPressed;
}
