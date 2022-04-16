# Bio

*Full control of input on any level.*

## ✅ Why choose Bio?

**Bio** allows monitoring keyboard input on *any* level of abstraction. It provides an easy-to-use wrapper for the native Win32 low-level keyboard hook as well as the class for working with detected input in a sophisticated way with a * rich* set of information about each input event.

## 🔩 Usage
### Low-level Hook
```C#
using Bio;

using var hook = new KeyboardHook();
hook.Set();
hook.KeyDown += Hook_HandleLowLevelInputMessage;
hook.KeyUp += Hook_HandleLowLevelInputMessage;
hook.Mute();
hook.Unmute();

void Hook_HandleLowLevelInputMessage(object? sender, KeyboardInputMessage e)
{
    var wm = e.WM;
    var lowLevelKeyInfo = e.KBDLLHOOKSTRUCT;
}
```

### High-level Input Detector
```C#
using Bio;
using Bio.Win32;

using var keySpy = new KeySpy();
keySpy.Activate();
keySpy.InputDetected += KeySpy_HandleHighLevelInputEvent;
keySpy.Mute();
keySpy.Unmute();

void KeySpy_HandleHighLevelInputEvent(object? sender, KeyInfo e)
{
    // KeyInfo is basically an advanced version of ConsoleKeyInfo that supports all keys and can differentiate between left and right modifier keys.
    VK vkCode = e.VK;
    char keyChar = e.KeyChar;
    ConsoleKey consoleKey = e.ConsoleKey;
    ModifierKeys modifierKeys = e.ModifierKeys;
}
```

### Input Synthesizer
```C#
using Bio;
using Bio.Win32;

KeyboardInput.SynthesizePress(VK.LSHIFT, VK.KEY_A);
KeyboardInput.Synthesize(WM.KEYDOWN, VK.KEY_0);
KeyboardInput.Synthesize(WM.KEYUP, VK.KEY_0);
```

## 💿 Installation

[Download page](https://www.nuget.org/packages/Bio/)

For guidance on how to install NuGet packages refer [here](https://docs.microsoft.com/en-us/nuget/quickstart/install-and-use-a-package-using-the-dotnet-cli) and [here](https://docs.microsoft.com/en-us/nuget/quickstart/install-and-use-a-package-in-visual-studio).

## 💡 Suggestions

If you would like to see some additional functionality that isn't provided by this library yet, 
feel free to leave your proposals in [**Issues**](https://github.com/GualaBanana/Bio/issues) section.  Any feedback is highly appreciated.
