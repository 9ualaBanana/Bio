namespace Bio;

/// <summary>
/// Defines generic extension methods for custom conversion of a typed object to any other type.
/// </summary>
public static class TranslationExtensions
{
    /// <summary>
    /// Converts <paramref name="obj"/> of type <typeparamref name="From"/> to the type <typeparamref name="To"/>
    /// by passing it to <paramref name="translator"/>.
    /// </summary>
    /// <typeparam name="From">The initial type of <paramref name="obj"/>.</typeparam>
    /// <typeparam name="To">The resulting type of <paramref name="obj"/>.</typeparam>
    /// <param name="obj">The object to be translated.</param>
    /// <param name="translator">The function that will be used for translation.</param>
    /// <returns>The representation of <paramref name="obj"/> after passing it through <paramref name="translator"/>.</returns>
    public static To TranslateWith<From, To>(this From obj, Func<From, To> translator) => translator(obj);
    /// <summary>
    /// Converts <paramref name="obj"/> of type <typeparamref name="From"/> to the type <typeparamref name="To"/>
    /// by passing it to <paramref name="translator"/>'s <see cref="ITranslator{From, To}.Translate(From)"/> method.
    /// </summary>
    /// <typeparam name="From">The initial type of <paramref name="obj"/>.</typeparam>
    /// <typeparam name="To">The resulting type of <paramref name="obj"/>.</typeparam>
    /// <param name="obj">The object to be translated.</param>
    /// <param name="translator">The instance of <see cref="ITranslator{From, To}"/> to be used for translation.</param>
    /// <returns>The representation of <paramref name="obj"/> after passing it through <paramref name="translator"/>.</returns>
    public static To TranslateWith<From, To>(this From obj, ITranslator<From, To> translator) => translator.Translate(obj);
}
