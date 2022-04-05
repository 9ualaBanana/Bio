using Xunit;
using InputListener;

namespace TestKeyboardInputListener.Tests;
 
public class TestInputListenerCore
{
    static KeyboardListener ListenerForAnyInput => new KeyboardListener();

    [Fact]
    public void Should_DoNothing_If_WasStoppedWithoutStartingFirst()
    {
        var listener = ListenerForAnyInput;

        listener.StopListening();

        Assert.False(listener.IsListening);
    }

    [Fact]
    public void Should_NotListen_After_StopListeningCalled()
    {
        var listener = ListenerForAnyInput;

        listener.StartListening();
        listener.StopListening();

        Assert.False(listener.IsListening);
    }

    [Fact]
    public void Should_NotListen_Until_StartListeningCalled()
    {
        var listener = ListenerForAnyInput;

        Assert.False(listener.IsListening);

        listener.StartListening();

        Assert.True(listener.IsListening);
    }
}
