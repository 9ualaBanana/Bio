using System.Threading.Tasks;
using Bio.Win32;
using FluentAssertions;
using Xunit;

namespace Bio.Test;

public class KeyboardHookTest
{
    [Fact]
    public void Set_SetsHook()
    {
        using var _hook = new KeyboardHook();
        _hook.Set();

        _hook.IsSet.Should().BeTrue();
    }

    [Fact]
    public void Set_DoesNotBlockThread()
    {
        using var _hook = new KeyboardHook();
        _hook.Set();
        bool blocks = false;

        _hook.IsSet.Should().BeTrue();
        blocks.Should().BeFalse();
    }

    [Fact]
    public void Set_Synchronously_BlocksThread()
    {
        using var _hook = new KeyboardHook();
        bool blocks = true;
        const int someTime = 10;
        var syncHookSetting = Task.Run(() =>
        {
            _hook.Set(asynchronously: false);
            blocks = false;
        });

        syncHookSetting.Wait(someTime);

        _hook.IsSet.Should().BeTrue();
        blocks.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(TestData.VkLetters), MemberType = typeof(TestData))]
    [MemberData(nameof(TestData.VkNumbers), MemberType = typeof(TestData))]
    public void Set_Muted_DoesNotRaiseEventsOnInput(VK vkCode)
    {
        using var _hook = new KeyboardHook();
        _hook.Set(muted: true);
        using var mutedHook = _hook.Monitor();

        KeyboardInput.SynthesizePress(vkCode);

        mutedHook.Should().NotRaise(nameof(_hook.KeyDown));
        mutedHook.Should().NotRaise(nameof(_hook.KeyUp));
    }

    [Theory]
    [MemberData(nameof(TestData.VkLetters), MemberType = typeof(TestData))]
    [MemberData(nameof(TestData.VkNumbers), MemberType = typeof(TestData))]
    public void Set_Unmuted_RaisesKeyDownOnInput(VK vkCode)
    {
        using var _hook = new KeyboardHook();
        _hook.Set(muted: false);
        using var unmutedHook = _hook.Monitor();

        KeyboardInput.Synthesize(WM.KEYDOWN, vkCode);

        unmutedHook.Should()
            .Raise(nameof(_hook.KeyDown))
            .WithSender(_hook)
            .WithArgs<KeyboardInputMessage>();
    }
    
    [Theory]
    [MemberData(nameof(TestData.VkLetters), MemberType = typeof(TestData))]
    [MemberData(nameof(TestData.VkNumbers), MemberType = typeof(TestData))]
    public void Set_Unmuted_RaisesKeyUpOnInput(VK vkCode)
    {
        using var _hook = new KeyboardHook();
        _hook.Set(muted: false);
        using var unmutedHook = _hook.Monitor();

        KeyboardInput.Synthesize(WM.KEYUP, vkCode);

        unmutedHook.Should()
            .Raise(nameof(_hook.KeyUp))
            .WithSender(_hook)
            .WithArgs<KeyboardInputMessage>();
    }
    
    [Theory]
    [MemberData(nameof(TestData.VkLetters), MemberType = typeof(TestData))]
    [MemberData(nameof(TestData.VkNumbers), MemberType = typeof(TestData))]
    public void Set_Unmuted_RaisesKeyUpAndKeyDownOnPress(VK vkCode)
    {
        using var _hook = new KeyboardHook();
        _hook.Set(muted: false);
        using var unmutedHook = _hook.Monitor();

        KeyboardInput.SynthesizePress(vkCode);

        unmutedHook.Should()
            .Raise(nameof(_hook.KeyDown))
            .WithSender(_hook)
            .WithArgs<KeyboardInputMessage>();
        unmutedHook.Should()
            .Raise(nameof(_hook.KeyUp))
            .WithSender(_hook)
            .WithArgs<KeyboardInputMessage>();
    }


    [Fact]
    public void Dispose_DisposesHook()
    {
        using var _hook = new KeyboardHook();
        _hook.Dispose();

        _hook.IsSet.Should().BeFalse();
    }

    [Fact]
    public void Dispose_CanBeCalledFromAnyThread()
    {
        using var _hook = new KeyboardHook();
        Task.Run(() => _hook.Dispose()).Wait();

        _hook.IsSet.Should().BeFalse();
    }
}
