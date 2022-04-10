namespace Bio;
// Namespaces are: In (main one) and Out(with translators, input synthesizers and all that shit).

// ? I need to simply replace current KeyboardListener implementation with the one built on top of LowLevelKeyboardHook
// and not to add it as another implementation, but simply replace insides of the one I currently have.
// It also should have a better name.
public class HookedKeyboardListener
{
    // All the bellow ideas should be implemented in high-level HookedKeyboardListener.
    // Event will send low-level key press information inside EventArgs.
    // InputListener.Translation will define a default translator for translating low-level key info to high-level ConsoleKey or smth.
    // It will also define a specific delegate for the translators of this kind.
    // LowLevelKeyboardHook needs to define the interface endpoint that will send preprocessed notifications that already got
    // translated into high-level ConsoleKey or smth.
    // That implies separate events for low- and high-level key press representations.
    // There will be 2 types of endpoints as I already said, and translated one will be just raised at the same time as
    // the one sending raw non-translated data via the method that will bundle those calls.
}
