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
using FluentAssertions;
using System;
using Xunit;

namespace Bio.Test;

public class KeyInfoTest
{
    [Theory]
    [MemberData(nameof(TestData.VkLetters), MemberType = typeof(TestData))]
    public void Shift_TogglesKeyCharCase(VK vkLetter)
    {
        var leftShift = new KeyInfo(vkLetter, ModifierKeys.ShiftLeft);
        var rightShift = new KeyInfo(vkLetter, ModifierKeys.ShiftRight);

        char.IsUpper(leftShift.KeyChar).Should().BeTrue();
        char.IsUpper(rightShift.KeyChar).Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(TestData.VkLetters), MemberType = typeof(TestData))]
    public void CapsLock_TogglesKeyCharCase(VK vkLetter)
    {
        if (!KeyInfo.CapsLockToggled) KeyboardInput.SynthesizePress(VK.CAPITAL);
        var upperKeyInfo = new KeyInfo(vkLetter, 0);
        KeyboardInput.SynthesizePress(VK.CAPITAL);
        var lowerKeyInfo = new KeyInfo(vkLetter, 0);

        char.IsUpper(upperKeyInfo.KeyChar).Should().BeTrue();
        char.IsLower(lowerKeyInfo.KeyChar).Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(TestData.VkLetters), MemberType = typeof(TestData))]
    public void ShiftAndCapsLock_CancelEachOther(VK vkLetter)
    {
        if (!KeyInfo.CapsLockToggled) KeyboardInput.SynthesizePress(VK.CAPITAL);
        var lowerKeyInfo = new KeyInfo(vkLetter, ModifierKeys.ShiftLeft);
        KeyboardInput.SynthesizePress(VK.CAPITAL);
        var upperKeyInfo = new KeyInfo(vkLetter, ModifierKeys.ShiftRight);

        char.IsLower(lowerKeyInfo.KeyChar).Should().BeTrue();
        char.IsUpper(upperKeyInfo.KeyChar).Should().BeTrue();
    }

    [Theory]
    [InlineData(ModifierKeys.ShiftLeft, ModifierKeys.ShiftRight)]
    [InlineData(ModifierKeys.ControlLeft, ModifierKeys.ControlRight)]
    [InlineData(ModifierKeys.AltLeft, ModifierKeys.AltRight)]
    public void ModifierKeys_TreatsLeftAndRightSeparately(ModifierKeys leftModifier, ModifierKeys rightModifier)
    {
        var keyInfo = new KeyInfo((uint)0, leftModifier | rightModifier);

        keyInfo.ModifierKeys.Should().HaveFlag(leftModifier);
        keyInfo.ModifierKeys.Should().HaveFlag(rightModifier);
    }
    
    [Theory]
    [InlineData(ModifierKeys.ShiftLeft, ModifierKeys.ShiftRight, ConsoleModifiers.Shift)]
    [InlineData(ModifierKeys.ControlLeft, ModifierKeys.ControlRight, ConsoleModifiers.Control)]
    [InlineData(ModifierKeys.AltLeft, ModifierKeys.AltRight, ConsoleModifiers.Alt)]
    public void ModifierKeys_TreatsLeftAndRightAsOne(
        ModifierKeys leftModifier,
        ModifierKeys rightModifier,
        ConsoleModifiers consoleModifier
        )
    {
        var keyInfo = new KeyInfo((uint)0, leftModifier | rightModifier);

        keyInfo.ConsoleModifiers.Should().HaveFlag(consoleModifier);
    }
}
