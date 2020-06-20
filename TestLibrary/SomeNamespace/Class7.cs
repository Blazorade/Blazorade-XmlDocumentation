using System;
using System.Collections.Generic;
using System.Text;

namespace TestLibrary.SomeNamespace
{
    /// <summary>
    /// A generic class with two generic type arguments and two different constructors using
    /// these generic arguments as parameters.
    /// </summary>
    /// <typeparam name="T1">Type #1</typeparam>
    /// <typeparam name="T2">Type #2</typeparam>
    public class Class7<T1, T2>
    {
        /// <summary>
        /// Constructor with the first generic parameter.
        /// </summary>
        /// <param name="t1">Parameter #1</param>
        public Class7(T1 t1) { }

        /// <summary>
        /// Constructor with the second generic parameter.
        /// </summary>
        /// <param name="t2">Type #2</param>
        public Class7(T2 t2) { }

        /// <summary>
        /// Constructor with two generic parameters.
        /// </summary>
        /// <param name="t1">Parameter #1</param>
        /// <param name="t2">Parameter #2</param>
        public Class7(T1 t1, T2 t2) { }

    }
}
