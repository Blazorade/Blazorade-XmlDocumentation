﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TestLibrary.SomeNamespace
{
    /// <summary>
    /// A generic dictionary class.
    /// </summary>
    /// <typeparam name="TKey">The type for the dictionary key.</typeparam>
    /// <typeparam name="TItem">The type for the values in the dictionary.</typeparam>
    public class Class3<TKey, TItem>
    {
        /// <summary>
        /// Creates an instance without loading stuff into the instance.
        /// </summary>
        public Class3() { }

        /// <summary>
        /// Creates an instance and loads it with stuff from <paramref name="source"/>.
        /// </summary>
        /// <param name="source"></param>
        public Class3(IDictionary<TKey, TItem> source) { }


        /// <summary>
        /// Returns the item with the given key.
        /// </summary>
        /// <param name="key">The key of the item to return.</param>
        /// <returns>Returns the item, or maybe not.</returns>
        public TItem this[TKey key] { get { return default(TItem); } }

        /// <summary>
        /// Returns the item at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The zero-based index for the item to return.</param>
        public TItem this[int index] { get { return default(TItem); } }



        /// <summary>
        /// A function with a generic dictionary as parameter.
        /// </summary>
        /// <param name="dictionary">A dictionary that uses the generic type parameters specified on the class.</param>
        public void Foo(IDictionary<TKey, TItem> dictionary) { }

    }

}
