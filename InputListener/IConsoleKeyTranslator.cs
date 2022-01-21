namespace InputListener.Translation;

/// <summary>
/// Defines a generalized method for translation of raw <see cref="ConsoleKey"/> input to <see cref="TTranslated"/>.
/// </summary>
/// <typeparam name="TTranslated">translation result type.</typeparam>
public interface IConsoleKeyTranslator<TTranslated>
{
    /// <summary>
    /// Translates raw input to a value of an arbitrary type.
    /// </summary>
    /// <param name="input">raw input to translate.</param>
    /// <returns>Translated input value</returns>
    TTranslated TranslateInput(ConsoleKey input);
}
