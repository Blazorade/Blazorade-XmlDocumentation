using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TestLibrary;
using System.Linq;
using System.Reflection;
using Blazorade.XmlDocumentation;
using TestLibrary.SomeNamespace;
using Blazorade.XmlDocumentation.Components.Services;

namespace Blazorade.XmlDocumentation.Tests
{
    [TestClass]
    public class FormattingTests
    {

        [TestInitialize]
        public void TestInit()
        {
            var f = Shared.GetFactory(); // Just make sure the proper assemblies are loaded for all tests.
        }



        [TestMethod]
        public void MemberUriName01()
        {
            var t = typeof(Class1).Assembly.GetTypes().Where(x => x.Name.Contains("Class3")).First();
            var itemProp = t.GetProperties().Where(x => x.Name == "Item").First();

            var uriName = itemProp.ToUriName();
            Assert.AreEqual("TestLibrary.SomeNamespace.Class3`2.Item", uriName);
        }




        [TestMethod]
        public void MethodDisplayName01()
        {
            var t = typeof(Class2);
            var names = from x in t.GetMethods() select x.ToDisplayName();

            var expectedNames = new string[]
            {
                "void Foo<TIn>(TIn inParam)",
                "TOut Foo<TOut>()"
            };

            foreach(var en in expectedNames)
            {
                Assert.IsTrue(names.Contains(en), $"There must be a name with the display name '{en}'");
            }
        }



        [TestMethod]
        public void ToMethod01()
        {
            var cref = new CRef("M:TestLibrary.SomeNamespace.Class1.#ctor");
            Assert.IsTrue(cref.IsMethod);
            Assert.IsTrue(cref.IsConstructor);

            var method = cref.ToMethod();
            Assert.IsNotNull(method);
            Assert.AreEqual(".ctor", method.Name);
        }

        [TestMethod]
        public void ToMethod02()
        {
            var cref = new CRef("M:TestLibrary.SomeNamespace.Class1.Foo``2(``0)");
            var method = cref.ToMethod();
            Assert.IsNotNull(method);
            Assert.AreEqual("Foo", method.Name);
        }

        [TestMethod]
        public void ToMethod03()
        {
            var cref = new CRef("M:TestLibrary.SomeNamespace.Class1.Foo``4(``0,``1,``2)");
            var method = cref.ToMethod();
            Assert.IsNotNull(method);
            Assert.AreEqual("Foo", method.Name);
        }



        [TestMethod]
        public void ToType01()
        {
            var cref = new CRef("T:TestLibrary.SomeNamespace.Class1");
            Assert.IsTrue(cref.IsType);
            Assert.AreEqual("TestLibrary.SomeNamespace.Class1", cref.Value);

            var t = cref.ToType();
            Assert.AreEqual(typeof(Class1), t);
        }
    }
}
