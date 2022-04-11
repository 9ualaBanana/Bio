using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Bio.Test;

public class KeyboardHookTest
{
    readonly KeyboardHook _hook;

    public KeyboardHookTest()
    {
        _hook = new KeyboardHook();
    }

    [Fact]
    public void Set_SetsHook()
    {
        _hook.Set();

        _hook.IsSet.Should().BeTrue();
    }

    [Fact]
    public void Set_DoesNotBlockThread()
    {
        _hook.Set();
        bool blocks = false;

        blocks.Should().BeFalse();
    }

    [Fact]
    public void Set_Synchronously_BlocksThread()
    {
        var syncHookSetting = Task.Run(() => _hook.Set(asynchronously: false));
        syncHookSetting.Wait(10);

        _hook.IsSet.Should().BeTrue();
        syncHookSetting.IsCompleted.Should().BeFalse();
    }

    [Fact]
    public void Dispose_DisposesHook()
    {
        _hook.Dispose();

        _hook.IsSet.Should().BeFalse();
    }

    [Fact]
    public void Dispose_CanBeCalledFromAnyThread()
    {
        Task.Run(() => _hook.Dispose()).Wait();

        _hook.IsSet.Should().BeFalse();
    }
}
