using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace TestLibrary.SomeNamespace
{
    /// <summary>
    /// A class with different types of properties.
    /// </summary>
    public class Class5
    {

        /// <summary>
        /// Just an indexed property.
        /// </summary>
        /// <param name="key">The key of the item to return.</param>
        /// <returns>Retrns the item that matches the given key.</returns>
        public string this[string key]
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// A indexed property with different type of indexer.
        /// </summary>
        /// <param name="index">The zero-based index of the item to return.</param>
        /// <returns>Returns the item at the given index.</returns>
        public string this[int index]
        {
            get { return null; }
            set { }
        }


        /// <summary>
        /// A read-write property.
        /// </summary>
        public object ReadWriteProperty { get; set; }

        /// <summary>
        /// A read-only property.
        /// </summary>
        public object ReadOnlyProperty { get; }

        /// <summary>
        /// A write-only property.
        /// </summary>
        public object WriteOnlyProperty { private get; set; }

        /// <summary>
        /// A property with actual implementation.
        /// </summary>
        public object ImplementedProperty
        {
            get { return default(object); }
            set { }
        }

    }
}
