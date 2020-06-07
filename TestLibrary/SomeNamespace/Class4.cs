using System;
using System.Collections.Generic;
using System.Text;

namespace TestLibrary.SomeNamespace
{
    /// <summary>
    /// Just a <c>class</c>...
    /// </summary>
    public class Class4<T>
    {

        /// <summary>
        /// A read-only indexed property.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                return default(T); 
            }
        }
    }
}
