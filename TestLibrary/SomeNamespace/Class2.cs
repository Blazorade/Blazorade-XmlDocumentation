using System;
using System.Collections.Generic;
using System.Text;

namespace TestLibrary.SomeNamespace
{
    /// <summary>
    /// Another class.
    /// </summary>
    public class Class2
    {

        /// <summary>
        /// Method with generic input.
        /// </summary>
        /// <typeparam name="TIn">Type for <paramref name="inParam"/>.</typeparam>
        /// <param name="inParam">The parameter, which is typed by <typeparamref name="TIn"/>.</param>
        public void Foo<TIn>(TIn inParam) { }

        /// <summary>
        /// Method with generic output.
        /// </summary>
        /// <typeparam name="TOut">The type for the return value.</typeparam>
        public TOut Foo<TOut>() { return default(TOut); }

    }
}
