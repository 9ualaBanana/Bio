namespace Bio;

/// <summary>
/// Defines a means for custom conversion of a typed object to any other type.
/// </summary>
/// <typeparam name="From">The type to convert.</typeparam>
/// <typeparam name="To">The type to conver to.</typeparam>
public interface ITranslator<in From, out To>
{
    /// <summary>
    /// Converts <paramref name="obj"/> to its <typeparamref name="To"/> representation.
    /// </summary>
    /// <param name="obj">The object to represent as <typeparamref name="To"/>.</param>
    /// <returns>The representation of <paramref name="obj"/> as <typeparamref name="To"/>.</returns>
    To Translate(From obj);
}
