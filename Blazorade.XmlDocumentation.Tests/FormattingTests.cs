using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TestLibrary;
using System.Linq;

namespace Blazorade.XmlDocumentation.Tests
{
    [TestClass]
    public class FormattingTests
    {

        [TestMethod]
        public void MethodDisplayName01()
        {
            var t = typeof(Class2);
            var names = from x in t.GetMethods() select x.ToDisplayName();

            var expectedNames = new string[]
            {
                "Foo<TIn>(TIn inParam)",
                "TOut Foo<TOut>()"
            };

            foreach(var en in expectedNames)
            {
                Assert.IsTrue(names.Contains(en), $"There must be a name with the display name '{en}'");
            }
        }
    }
}
