using Bio.Win32;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using System;
using System.Collections.Generic;

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
    [MemberData(nameof(Input))]
    public void Set_Muted_SetsMutedHook(KeyboardInput input)
    {
        using var _hook = new KeyboardHook();
        _hook.Set(muted: true);
        using var mutedHook = _hook.Monitor();

        input.Synthesize();

        mutedHook.Should().NotRaise(nameof(_hook.OnInput));
    }

    [Theory]
    [MemberData(nameof(Input))]
    public void OnInput_Unmuted_RaisesEvents(KeyboardInput input)
    {
        using var _hook = new KeyboardHook();
        _hook.Set(muted: false);
        using var unmutedHook = _hook.Monitor();

        input.Synthesize();

        unmutedHook.Should()
            .Raise(nameof(_hook.OnInput))
            .WithSender(_hook)
            .WithArgs<KeyboardInputMessageEventArgs>();
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

    static IEnumerable<object[]> Input()
    {
        foreach (var vkCode in Enum.GetValues<VK>())
        {
            if (vkCode >= VK.KEY_0 && vkCode <= VK.KEY_Z)
                yield return new object[] { new KeyboardInput(vkCode) };
        }
    }
}
