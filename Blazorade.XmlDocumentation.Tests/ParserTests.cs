using Blazorade.Bootstrap.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TestLibrary;
using TestLibrary.SomeNamespace;

namespace Blazorade.XmlDocumentation.Tests
{
    [TestClass]
    public class ParserTests
    {

        [TestInitialize]
        public void TestInit()
        {
            var f = Shared.GetFactory(); // Just make sure that all assemblies are properly loaded for each test.
        }



        [TestMethod]
        public void AddParser01()
        {
            var t = typeof(Alert);
            DocumentationParserFactory f = new DocumentationParserFactory();
            var p = f.AddParser(ParserKeys.Bootstrap, t.Assembly);
            var doc = p.GetDocumentation(t);

            Assert.IsNotNull(doc);
            Assert.AreEqual(t, doc.Member);
        }



        [TestMethod]
        public void CreateParser01()
        {
            var parser = new DocumentationParser(Properties.Resources.Blazorade_Core);
            Assert.AreEqual("Blazorade.Core", parser.AssemblyName);
        }

        [TestMethod]
        public void CreateParser02()
        {
            var parser = new DocumentationParser(Properties.Resources.Blazorade_Bootstrap_Components);
            Assert.AreEqual("Blazorade.Bootstrap.Components", parser.AssemblyName);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void CreateParser03()
        {
            // This is not a valid XML docs file.
            var parser = new DocumentationParser("<root />");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void CreateParser04()
        {
            // This is not a valid XML docs file. It misses the doc/assembly/name element, which is required.
            var parser = new DocumentationParser("<doc><assembly></assembly></doc>");
        }



        [TestMethod]
        public void Cref01()
        {
            var cref = new CRef("M:TestLibrary.SomeNamespace.Class6`1.#ctor(`0)");
            var m = cref.ToMethod();
            Assert.IsNotNull(m);
        }

        [TestMethod]
        public void Cref02()
        {
            var crefs = new string[]
            {
                "M:TestLibrary.SomeNamespace.Class1.Foo``4(``0,``1,``2)",
                "M:TestLibrary.SomeNamespace.Class6`1.#ctor(`0)",
            };

            foreach(var c in crefs)
            {
                var cref = new CRef(c);
                var m = cref.ToMethod();
                Assert.IsNotNull(m, $"The string '{c}' must be a valid method reference.");
            }
        }

        [TestMethod]
        public void Cref03()
        {
            var cref = new CRef("M:TestLibrary.SomeNamespace.Class7`2.#ctor(`0,`1)");
            var m = cref.ToMethod();
            Assert.IsNotNull(m);
            var paramArr = m.GetParameters();
            Assert.AreEqual(2, paramArr.Length);
        }

        [TestMethod]
        public void Cref04()
        {
            var cref = new CRef("M:TestLibrary.SomeNamespace.Class3`2.#ctor(System.Collections.Generic.IDictionary{`0,`1})");
            Assert.IsTrue(cref.IsMethod);
            var m = cref.ToMethod();
            Assert.IsNotNull(m);
        }



        [TestMethod]
        public void GetFields01()
        {
            var p = Shared.GetFactory().GetParser(ParserKeys.Bootstrap);
            var doc = p.GetDocumentation(typeof(Backdrop));
            var fields = p.GetFields(doc);

            Assert.AreNotEqual(0, fields.Count());
            Assert.IsNotNull(fields.FirstOrDefault(x => x.Name == nameof(Backdrop.Default)));
            Assert.IsNotNull(fields.FirstOrDefault(x => x.Name == nameof(Backdrop.Hidden)));
            Assert.IsNotNull(fields.FirstOrDefault(x => x.Name == nameof(Backdrop.Static)));
        }

        [TestMethod]
        public void GetFields02()
        {

        }



        [TestMethod]
        public void GetMembers01()
        {
            var name = "TestLibrary.SomeNamespace.Class1.Foo";
            var p = Shared.GetFactory().GetParser(ParserKeys.TestLib);
            var members = p.GetMembers(name).ToList();
            Assert.AreNotEqual(0, members.Count());
            foreach(var m in members)
            {
                Assert.AreEqual("Foo", m.Name);
            }
        }

        [TestMethod]
        public void GetMembers02()
        {
            var t = typeof(Class1);
            var p = Shared.GetFactory().GetParser(ParserKeys.TestLib);
            var doc = p.GetDocumentation(t);
            
            var members = t.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            
            foreach(var m in members)
            {
                var uriName = m.ToFullName();
                var memberDocs = p.GetMembers(uriName).ToList();
                Assert.AreNotEqual(0, memberDocs.Count(), $"The name '{uriName}' must return at least one member.");
            }
        }

        [TestMethod]
        public void GetMembers03()
        {
            var t = typeof(Class5);
            var p = Shared.GetFactory().GetParserByType("");// .GetParser(ParserKeys.TestLib);
            var expectedNames = new string[] {
                "TestLibrary.SomeNamespace.Class5..ctor",
                "TestLibrary.SomeNamespace.Class5.Item",
                "TestLibrary.SomeNamespace.Class5.ReadWriteProperty",
                "TestLibrary.SomeNamespace.Class5.ReadOnlyProperty",
                "TestLibrary.SomeNamespace.Class5.WriteOnlyProperty",
                "TestLibrary.SomeNamespace.Class5.ImplementedProperty"
            };

            var members = t.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            Assert.AreNotEqual(0, members.Count());

            foreach(var m in members)
            {
                var name = m.ToFullName();
                Assert.IsTrue(expectedNames.Contains(name), $"The name '{name}' must be contained in the collection of expected values.");
            }
        }

        [TestMethod]
        public void GetMembers04()
        {
            var t = typeof(Class1);
            var p = Shared.GetFactory().GetParserByType(t).Item2;
            Assert.IsNotNull(p);

            var members = p.GetMembers("TestLibrary.SomeNamespace.Class1..ctor").ToList();
            Assert.AreNotEqual(0, members.Count());
        }

        [TestMethod]
        public void GetMembers05()
        {
            var expected = new string[]
            {
                "void Foo()",
                "void Foo(string what)",
                "TOut Foo<TOut>()",
                "void Foo<TWhat>(TWhat what)",
                "void Foo<T1, T2>(T1 in1, T2 in2)",
                "TOut Foo<TIn, TOut>(TIn input)",
                "TOut Foo<T1, T2, T3, TOut>(T1 in1, T2 in2, T3 in3)"
            };

            var p = Shared.GetFactory().GetParser(ParserKeys.TestLib);
            var members = p.GetMembers("TestLibrary.SomeNamespace.Class1.Foo");
            foreach(var m in members)
            {
                var name = m.Member.ToDisplayName();
                Assert.IsTrue(expected.Contains(name), $"The display name '{name}' must exist in the expected collection.");
            }
        }

        [TestMethod]
        public void GetMembers06()
        {
            var expected = new string[]
            {
                ".ctor()",
                ".ctor(string input)"
            };

            var p = Shared.GetFactory().GetParser(ParserKeys.TestLib);
            var members = p.GetMembers("TestLibrary.SomeNamespace.Class1..ctor");
            foreach(var m in members)
            {
                var name = m.Member.ToDisplayName();
                Assert.IsTrue(expected.Contains(name), $"The display name '{name}' must exist in the expected collection.");
            }
        }

        [TestMethod]
        public void GetMembers07()
        {
            var expected = new string[]
            {
                "TItem Item[TKey key]",
                "TItem Item[int index]"
            };

            var t = typeof(Class1).Assembly.GetTypes().First(x => x.Name == "Class3`2");
            var p = Shared.GetFactory().GetParserByType(t).Item2;
            var members = p.GetMembers("TestLibrary.SomeNamespace.Class3`2.Item");
            foreach (var m in members)
            {
                var name = m.Member.ToDisplayName();
                Assert.IsTrue(expected.Contains(name), $"The display name '{name}' must exist in the expected collection.");
            }
        }



        [TestMethod]
        public void GetMethods01()
        {
            var p = Shared.GetFactory().GetParser(ParserKeys.Bootstrap);
            var doc = p.GetDocumentation(typeof(Modal));
            var methods = p.GetMethods(doc);
            Assert.AreNotEqual(0, methods.Count());
        }

        [TestMethod]
        public void GetMethods02()
        {
            var p = Shared.GetFactory().GetParser(ParserKeys.XmlDocs);
            var doc = p.GetDocumentation(typeof(DocumentationParser));
            var methods = p.GetMethods(doc);
            Assert.AreNotEqual(0, methods.Count());

            var methodsWithParams = from x in methods where x.GetParameters().Count() > 0 select x;
            Assert.AreNotEqual(0, methodsWithParams.Count());

            foreach(var m in methodsWithParams)
            {
                var pArr = m.GetParameters();
                Assert.AreNotEqual(0, pArr.Count());
            }
        }

        [TestMethod]
        public void GetMethods03()
        {
            var p = Shared.GetFactory().GetParser(ParserKeys.XmlDocs);
            var doc = p.GetDocumentation(typeof(DocumentationParser));

            List<string> names = new List<string>();
            foreach(var m in p.GetMethods(doc))
            {
                Assert.IsFalse(names.Contains(m.Member.ToDisplayName()));
                names.Add(m.Member.ToDisplayName());
            }
        }

        [TestMethod]
        public void GetMethods04()
        {
            var p = Shared.GetFactory().GetParser(ParserKeys.XmlDocs);
            var doc = p.GetDocumentation(typeof(DocumentationExtensions));

            var names = new List<string>();
            foreach(var m in p.GetMethods(doc))
            {
                Assert.IsFalse(names.Contains(m.Member.ToDisplayName()));
                names.Add(m.Member.ToDisplayName());
            }
        }

        [TestMethod]
        public void GetMethods05()
        {
            var expectedList = new List<string>
            {
                "void Foo()",
                "void Foo(string what)",
                "TOut Foo<TOut>()",
                "void Foo<TWhat>(TWhat what)",
                "void Foo<T1, T2>(T1 in1, T2 in2)",
                "TOut Foo<TIn, TOut>(TIn input)",
            };

            var p = Shared.GetFactory().GetParser(ParserKeys.TestLib);
            var methods = p.GetMethods(typeof(TestLibrary.SomeNamespace.Class1)).ToList();
            var displayNames = from x in methods select x.Member.ToDisplayName();

            foreach(var itm in expectedList)
            {
                Assert.IsTrue(displayNames.Contains(itm), $"There must be a method with the display name '{itm}'.");
            }
        }

        [TestMethod]
        public void GetMethods06()
        {
            var list = new List<string>
            {
                "void Foo<TIn>(TIn inParam)",
                "TOut Foo<TOut>()"
            };

            var p = Shared.GetFactory().GetParser(ParserKeys.TestLib);
            var methods = p.GetMethods(typeof(Class2)).ToList();
            var names = from x in methods select x.Member.ToDisplayName();

            foreach(var itm in list)
            {
                Assert.IsTrue(names.Contains(itm), $"There must be a method with the display name '{itm}'.");
            }
        }

        [TestMethod]
        public void GetMethods07()
        {
            var t = typeof(Class3<string, object>);
            var p = Shared.GetFactory().GetParser(ParserKeys.TestLib);
            var methods = p.GetMethods(t).ToList();
        }

        [TestMethod]
        public void GetMethods08()
        {
            var expected = new string[]
            {
                ".ctor()",
                ".ctor(IDictionary<TKey, TItem> source)"
            };
            var t = typeof(Class3<,>);
            var p = Shared.GetFactory().GetParserByType(t).Item2;

            var names = p.GetMembers("TestLibrary.SomeNamespace.Class3`2..ctor").Select(x => x.Member.ToDisplayName()).ToList();

            foreach(var name in expected)
            {
                Assert.IsTrue(names.Contains(name), $"The name '{name}' must exist in the generated constructor display names.");
            }
        }

        [TestMethod]
        public void GetMethods09()
        {
            var t = typeof(Class6<>);
            var p = Shared.GetFactory().GetParserByType(t).Item2;

            var constructors = from x in p.GetMethods(t) where x.Member.IsConstructor select x;
            Assert.AreNotEqual(0, constructors.Count());
        }



        [TestMethod]
        public void GetNamespaces01()
        {
            var parser = new DocumentationParser(Properties.Resources.Blazorade_Bootstrap_Components);
            var nsList = parser.GetNamespaces();
            Assert.AreEqual(6, nsList.Count());
        }




        [TestMethod]
        public void GetTypes01()
        {
            var ns = typeof(Alert).Namespace;
            var parser = new DocumentationParser(Properties.Resources.Blazorade_Bootstrap_Components);
            var types = parser.GetTypes(ns);

            Assert.AreNotEqual(0, types.Count());
            foreach(var t in types)
            {
                Assert.AreEqual(ns, t.Namespace);
            }
        }

        [TestMethod]
        public void GetTypes02()
        {
            var parser = new DocumentationParser(Properties.Resources.Blazorade_Bootstrap_Components);
            var baseType = parser.GetDocumentation(typeof(BootstrapComponentBase));
            Assert.IsNotNull(baseType);
        }

        [TestMethod]
        public void GetTypes03()
        {
            var parser = Shared.GetFactory().GetParser(ParserKeys.Bootstrap);
            var t = parser.GetType(typeof(Alert).FullName);
            Assert.IsNotNull(t);
            Assert.AreEqual(typeof(Alert), t);
        }

        [TestMethod]
        public void GetTypes04()
        {
            var p = Shared.GetFactory().GetParser(ParserKeys.TestLib);
            var doc = p.GetDocumentation(typeof(Class1));
            Assert.IsNotNull(doc);
        }




        [TestMethod]
        public void GetTypeDisplayName01()
        {
            var t = typeof(List<string>);
            var name = t.ToDisplayName();
            Assert.AreEqual("List<string>", name);
        }

        [TestMethod]
        public void GetTypeDisplayName02()
        {
            var t = typeof(IDictionary<string, object>);
            Assert.IsNotNull(t);
            var name = t.ToDisplayName();
            Assert.AreEqual("IDictionary<string, object>", name);
        }




        [TestMethod]
        public void GetProperties01()
        {
            var parser = new DocumentationParser(Properties.Resources.Blazorade_Bootstrap_Components);
            var type = parser.GetDocumentation(typeof(BootstrapComponentBase));
            var props = parser.GetProperties(type);

            Assert.AreNotEqual(0, props.Count());
        }

        [TestMethod]
        public void GetProperties02()
        {
            var f = Shared.GetFactory();
            var p = f.GetParser(ParserKeys.Bootstrap);
            var type = p.GetDocumentation(typeof(Card));
            var props = p.GetProperties(type);

            Assert.AreNotEqual(0, props.Count());
        }

        [TestMethod]
        public void GetProperties03()
        {
            var t = typeof(MemberDocumentation);
            var p = Shared.GetFactory().GetParser(ParserKeys.XmlDocs);
            var properties = p.GetProperties(t).ToList();
            Assert.IsNotNull(properties.FirstOrDefault(x => x.Name == nameof(MemberDocumentation.Member)));
        }

        [TestMethod]
        public void GetProperties04()
        {
            var t = typeof(Class4<object>).Assembly.GetTypes().Where(x => x.Name == "Class4`1").FirstOrDefault();
            var p = Shared.GetFactory().GetParser(ParserKeys.TestLib);
            var props = p.GetProperties(t).ToList();
            Assert.AreNotEqual(0, props.Count());

            var prop = props.FirstOrDefault(x => x.Name == "Item");
            Assert.IsNotNull(prop);
        }



        [TestMethod]
        public void ReadAssemblyInfo01()
        {
            var f = new DocumentationParserFactory();
            f.AddParser(ParserKeys.Bootstrap, typeof(BootstrapComponentBase).Assembly);
            var p = f.GetParser(ParserKeys.Bootstrap);
            Assert.IsNotNull(p.AssemblyName);
            Assert.IsNotNull(p.AssemblyVersion);
            Assert.IsNotNull(p.AssemblyProductName);
            Assert.IsNotNull(p.AssemblyDescription);
        }



        [TestMethod]
        public void ReadDocs01()
        {
            var p = Shared.GetFactory().GetParser(ParserKeys.Bootstrap);
            var doc = p.GetDocumentation(typeof(AlertLink));
            Assert.IsNotNull(doc);
        }

        [TestMethod]
        public void ReadDocs02()
        {
            var p = Shared.GetFactory().GetParser(ParserKeys.XmlDocsComponenents);
            var doc = p.GetDocumentation(typeof(Components.Xml.CNode));
            Assert.IsNotNull(doc);
        }

        [TestMethod]
        public void ReadDocs03()
        {
            var p = Shared.GetFactory().GetParser(ParserKeys.TestLib);
            var t = typeof(Class3<string, object>);
            var doc = p.GetDocumentation(t);

            Assert.IsNotNull(doc);
        }

    }
}
