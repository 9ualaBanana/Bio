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
