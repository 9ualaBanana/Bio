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
