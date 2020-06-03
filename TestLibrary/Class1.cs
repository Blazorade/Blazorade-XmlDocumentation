using System;

namespace TestLibrary
{
    /// <summary>
    /// Just a class for no reason.
    /// </summary>
    public class Class1
    {

        /// <summary>
        /// The default constructor.
        /// </summary>
        public Class1() { }

        /// <summary>
        /// An overloaded version of the constructor.
        /// </summary>
        /// <param name="input"></param>
        public Class1(string input) { }



        /// <summary>
        /// A foo method.
        /// </summary>
        public void Foo() { }

        /// <summary>
        /// A foo method with a parameter.
        /// </summary>
        /// <param name="what">A simple string parameter with some <c>fancy</c> docs.</param>
        public void Foo(string what) { }

        /// <summary>
        /// A generic method with no parameters.
        /// </summary>
        /// <typeparam name="TOut">The type for the return value.</typeparam>
        public TOut Foo<TOut>() { return default(TOut); }

        /// <summary>
        /// A foo method with a generic parameter.
        /// </summary>
        /// <typeparam name="TWhat">The type for the <paramref name="what"/> parameter. Currently, this can be anything.</typeparam>
        /// <param name="what">The parameter documentation.</param>
        public void Foo<TWhat>(TWhat what) { }

        /// <summary>
        /// Yet another foo method without a reason.
        /// </summary>
        /// <typeparam name="T1">A type parameter. Specifies type for <paramref name="in1"/>.</typeparam>
        /// <typeparam name="T2">Another type parameter. Specifies type for <paramref name="in2"/>.</typeparam>
        /// <param name="in1">The type for <paramref name="in1"/>.</param>
        /// <param name="in2">The type for <paramref name="in2"/>.</param>
        public void Foo<T1, T2>(T1 in1, T2 in2) { }

        /// <summary>
        /// A generic foo method with both input and output being defined as generic types.
        /// </summary>
        /// <typeparam name="TIn">The type to put in to the method.</typeparam>
        /// <typeparam name="TOut">The type to get out from the method.</typeparam>
        /// <param name="input">The actual input, which is typed by <typeparamref name="TIn"/>.</param>
        public TOut Foo<TIn, TOut>(TIn input) { return default(TOut); }

        /// <summary>
        /// A Foo method with lots of generics.
        /// </summary>
        public TOut Foo<T1, T2, T3, TOut>(T1 in1, T2 in2, T3 in3)
        {
            return default(TOut);
        }
    }
}
