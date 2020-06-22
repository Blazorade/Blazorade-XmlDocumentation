using System;
using System.Collections.Generic;
using System.Text;

namespace TestLibrary.SomeNamespace
{
    /// <summary>
    /// A class that implements the singleton pattern.
    /// </summary>
    /// <example>
    /// <para>
    /// This example demonstrates how to use the <c>Singleton</c> class. Note that the code below has a lot of unnecessary rows.
    /// They are there just to try out multiple lines of code.
    /// </para>
    /// <code>
    /// <![CDATA[
    /// var s = Singleton.GetInstance();
    /// if(s.HasStuff)
    /// {
    ///     for(int i = 0; i < 10; i++)
    ///     {
    ///         s.Write(i);
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class Singleton
    {

        private Singleton() { }

        private static Singleton Instance;
        /// <summary>
        /// Returns the singleton instance for the <see cref="Singleton"/> class.
        /// </summary>
        /// <returns></returns>
        public static Singleton GetInstance()
        {
            Func<Singleton> creator = () =>
            {
                Instance = new Singleton()
                {
                    HasStuff = true
                };
                return Instance;
            };
            return Instance ?? creator();
        }


        /// <summary>
        /// Just a boolean read-only property.
        /// </summary>
        public bool HasStuff { get; private set; }


        /// <summary>
        /// Pretends that you can write stuff using this method.
        /// </summary>
        /// <param name="text">The text to write.</param>
        public void Write(string text)
        {

        }

        /// <summary>
        /// The same as <see cref="Write(string)"/> but with an object.
        /// </summary>
        /// <param name="data">The data to write.</param>
        public void Write(object data)
        {

        }
    }
}
