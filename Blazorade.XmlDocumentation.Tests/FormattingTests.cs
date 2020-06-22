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
using System.Xml;

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

            var uriName = itemProp.ToFullName();
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
        public void Split01()
        {
            var input = "System.Collections.Generic.IDictionary{`0,`1},System.String";
            var arr = input.SplitTypeDefinitions().ToList();
            Assert.AreEqual(2, arr.Count());
            Assert.AreEqual("System.Collections.Generic.IDictionary{`0,`1}", arr.ElementAt(0));
            Assert.AreEqual("System.String", arr.ElementAt(1));
        }

        [TestMethod]
        public void Split02()
        {
            var input = "System.String,System.Collections.Generic.IDictionary{`0,`1}";
            var arr = input.SplitTypeDefinitions();
            Assert.AreEqual(2, arr.Count());
            Assert.AreEqual("System.String", arr.ElementAt(0));
            Assert.AreEqual("System.Collections.Generic.IDictionary{`0,`1}", arr.ElementAt(1));
        }



        [TestMethod]
        public void FindMethod01()
        {
            var name = "Foo";
            var t = typeof(Class3<,>);
            var pt = typeof(IDictionary<,>);
            var expected = t.GetMethods().First(x => x.Name == name);
            var actual = t.FindMethod(name, new Type[] { pt });

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FindMethod02()
        {
            var name = ".ctor";
            var t = typeof(Class1);
            var m1 = t.FindMethod(name, new Type[0]);
            var m2 = t.FindMethod(name, new Type[0], false);
            var m3 = t.FindMethod(name, new Type[0], true);

            Assert.IsNotNull(m1);
            Assert.IsNotNull(m2);
            Assert.AreEqual(m1, m2);
            Assert.IsNull(m3);
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
        public void ToMethod04()
        {
            var m = "TestLibrary.SomeNamespace.Class1.#ctor".ToMethod();
            Assert.IsNotNull(m);
        }

        [TestMethod]
        public void ToMethod05()
        {
            var m = "TestLibrary.SomeNamespace.Class1.Foo".ToMethod();
            Assert.IsNotNull(m);
        }

        [TestMethod]
        public void ToMethod06()
        {
            var m = "TestLibrary.SomeNamespace.Class1.Foo(System.String)".ToMethod();
            Assert.IsNotNull(m);
        }

        [TestMethod]
        public void ToMethod07()
        {
            var m = "TestLibrary.SomeNamespace.Class3`2.#ctor(System.Collections.Generic.IDictionary{`0,`1})".ToMethod();
            Assert.IsNotNull(m);
        }

        [TestMethod]
        public void ToMethod08()
        {
            var m = "TestLibrary.SomeNamespace.Class7`2.#ctor(`0,`1)".ToMethod();
            Assert.IsNotNull(m);
        }

        [TestMethod]
        public void ToMethod09()
        {
            var m = "TestLibrary.SomeNamespace.Class1.Foo``1".ToMethod();
            Assert.IsNotNull(m);
        }

        [TestMethod]
        public void ToMethod10()
        {
            var m = "TestLibrary.SomeNamespace.Class1.Foo``2(``0,``1)".ToMethod();
            Assert.IsNotNull(m);
        }

        [TestMethod]
        public void ToMethod11()
        {
            var m = "TestLibrary.SomeNamespace.Class1.Foo``4(``0,``1,``2)".ToMethod();
            Assert.IsNotNull(m);
        }

        [TestMethod]
        public void ToMethod12()
        {
            var m = "TestLibrary.SomeNamespace.Class3`2.#ctor(System.Collections.Generic.IDictionary{`0,`1})".ToMethod();
            Assert.IsNotNull(m);
        }

        [TestMethod]
        public void ToMethod13()
        {
            var m = "TestLibrary.SomeNamespace.Class3`2.Foo(System.Collections.Generic.IDictionary{`0,`1})".ToMethod();
            Assert.IsNotNull(m);
        }

        [TestMethod]
        public void ToMethod14()
        {
            var m1 = "TestLibrary.SomeNamespace.Class1.Foo``1".ToMethod();
            var m2 = "TestLibrary.SomeNamespace.Class1.Foo".ToMethod();

            Assert.AreNotEqual(m1, m2);
        }

        [TestMethod]
        public void ToMethod15()
        {
            var m = "TestLibrary.SomeNamespace.Class1.Foo``1(``0)".ToMethod();
            Assert.IsNotNull(m);
        }

        [TestMethod]
        public void ToMethod99()
        {
            var p = Shared.GetFactory().GetParser(ParserKeys.TestLib);
            foreach(XmlNode node in p.Document.DocumentElement.SelectNodes("members/member[starts-with(@name, 'M:')]"))
            {
                var methodString = node.Attributes["name"].Value.Substring(2); // Skipping M: from the start.
                var method = methodString.ToMethod();
                Assert.IsNotNull(method, $"The method definition '{methodString}' must produce a method instance.");
            }
        }



        [TestMethod]
        public void ToProperty01()
        {
            var p = "TestLibrary.SomeNamespace.Class1.Prop1".ToProperty();
            Assert.IsNotNull(p);
        }

        [TestMethod]
        public void ToProperty02()
        {
            var p = "TestLibrary.SomeNamespace.Class3`2.Item(`0)".ToProperty();
            Assert.IsNotNull(p);
        }

        [TestMethod]
        public void ToProperty99()
        {
            var p = Shared.GetFactory().GetParser(ParserKeys.TestLib);
            foreach (XmlNode node in p.Document.DocumentElement.SelectNodes("members/member[starts-with(@name, 'P:')]"))
            {
                var propertyString = node.Attributes["name"].Value.Substring(2); // Skipping P: from the start.
                var property = propertyString.ToProperty();
                Assert.IsNotNull(property, $"The method definition '{propertyString}' must produce a property instance.");
            }
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

        [TestMethod]
        public void ToType02()
        {
            var t = "System.String".ToType();
            Assert.AreEqual(typeof(string), t);
        }

        [TestMethod]
        public void ToType03()
        {
            var t = "System.Collections.Generic.IDictionary{System.Int32,System.Boolean}".ToType();
            Assert.AreEqual(typeof(IDictionary<int, bool>), t);
        }

        [TestMethod]
        public void ToType04()
        {
            var t = "System.Collections.Generic.IDictionary{`0,`1}".ToType();
            Assert.AreEqual(typeof(IDictionary<,>), t);
        }

    }
}
