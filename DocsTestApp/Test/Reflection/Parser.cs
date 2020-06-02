using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocsTestApp.Test.Reflection
{
    /// <summary>
    /// This is a parser, which is capable of parsing stuff.
    /// </summary>
    public class Parser
    {

        /// <summary>
        /// The precision of parsing where 0 is the works and 1 is the best precision.
        /// </summary>
        /// <remarks>
        /// The value must be set to a value between 0 and 1 inclusively.
        /// </remarks>
        /// <example>
        /// <para>
        /// Just a very simple example of how this property is used.
        /// </para>
        /// <code>
        /// var parser = new Parser();
        /// parser.Precision = .5;
        /// </code>
        /// </example>
        /// <exception cref="ArgumentException">The exception that is thrown if <see cref="Precision"/> is less than 0 or greater than 1.</exception>
        public float Precision { get; set; }


        /// <summary>
        /// Does nothing.
        /// </summary>
        public void Foo()
        {

        }

        /// <summary>
        /// Really does nothing.
        /// </summary>
        /// <param name="too"></param>
        public void Foo(string too)
        {

        }

        /// <summary>
        /// Still does nothing.
        /// </summary>
        /// <typeparam name="TInput">An arbitrary input that does absolutely nothing.</typeparam>
        /// <param name="what">Stuff typed as <typeparamref name="TInput"/>.</param>
        public void Foo<TInput>(TInput what)
        {

        }
    }
}
