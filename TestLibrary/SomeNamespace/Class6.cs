using System;
using System.Collections.Generic;
using System.Text;

namespace TestLibrary.SomeNamespace
{
    /// <summary>
    /// A generic class with one constructor taking a generic parameter.
    /// </summary>
    /// <typeparam name="T">The generic type.</typeparam>
    public class Class6<T>
    {
        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="input">The input for the constructor.</param>
        public Class6(T input) { }

        /// <summary>
        /// A constructor with one normal parameter and one generic.
        /// </summary>
        /// <param name="text">Just some text.</param>
        /// <param name="input">The generic parameter.</param>
        public Class6(string text, T input) { }
    }
}
