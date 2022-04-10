namespace Bio;

public static class ConsoleKeyExtensions
{
    public static TTranslated TranslateWith<TTranslated>(
        this ConsoleKey key,
        Func<ConsoleKey,
        TTranslated> translator
        ) => translator(key);
    public static TTranslated TranslateWith<TTranslated>(
        this ConsoleKey key,
        IConsoleKeyTranslator<TTranslated> translator
        ) => translator.TranslateInput(key);
}
