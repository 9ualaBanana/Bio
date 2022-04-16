using Bio.Win32;
using FluentAssertions;
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
}
