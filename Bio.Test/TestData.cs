using Bio.Win32;
using System;
using System.Collections.Generic;

namespace Bio.Test;

public static class TestData
{
    public static IEnumerable<object[]> VkLetters
    {
        get
        {
            for (var vkCode = VK.KEY_A; vkCode <= VK.KEY_Z; vkCode++)
            {
                yield return new object[] { vkCode };
            }
        }
    }

    public static IEnumerable<object[]> VkNumbers
    {
        get
        {
            for (var vkCode = VK.KEY_0; vkCode <= VK.KEY_9; vkCode++)
            {
                yield return new object[] { vkCode };
            }
        }
    }
}
