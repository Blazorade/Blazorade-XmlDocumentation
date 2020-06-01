using Blazorade.Bootstrap.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Blazorade.XmlDocumentation.Tests
{
    [TestClass]
    public class ParserTests
    {

        [TestMethod]
        public void AddParser01()
        {
            var t = typeof(Alert);
            DocumentationParserFactory f = new DocumentationParserFactory();
            var p = f.AddParser(ParserKeys.Bootstrap, t.Assembly);
            var doc = p.GetDocumentation(t);

            Assert.IsNotNull(doc);
            Assert.AreEqual(t, doc.DocumentedMember);
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
        public void GetFields01()
        {
            var p = this.GetFactory().GetParser(ParserKeys.Bootstrap);
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
        public void GetMethods01()
        {
            var p = this.GetFactory().GetParser(ParserKeys.Bootstrap);
            var doc = p.GetDocumentation(typeof(Modal));
            var methods = p.GetMethods(doc);
            Assert.AreNotEqual(0, methods.Count());
        }

        [TestMethod]
        public void GetMethods02()
        {
            var p = this.GetFactory().GetParser(ParserKeys.XmlDocs);
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
            var p = this.GetFactory().GetParser(ParserKeys.XmlDocs);
            var doc = p.GetDocumentation(typeof(DocumentationParser));

            List<string> names = new List<string>();
            foreach(var m in p.GetMethods(doc))
            {
                Assert.IsFalse(names.Contains(m.DocumentedMember.ToDisplayName()));
                names.Add(m.DocumentedMember.ToDisplayName());
            }
        }

        [TestMethod]
        public void GetMethods04()
        {
            var p = this.GetFactory().GetParser(ParserKeys.XmlDocs);
            var doc = p.GetDocumentation(typeof(DocumentationExtensions));

            var names = new List<string>();
            foreach(var m in p.GetMethods(doc))
            {
                Assert.IsFalse(names.Contains(m.DocumentedMember.ToDisplayName()));
                names.Add(m.DocumentedMember.ToDisplayName());
            }
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
            var parser = this.GetFactory().GetParser(ParserKeys.Bootstrap);
            var t = parser.GetType(typeof(Alert).FullName);
            Assert.IsNotNull(t);
            Assert.AreEqual(typeof(Alert), t);
        }



        [TestMethod]
        public void GetTypeDisplayName01()
        {
            var t = typeof(MemberDocumentation<PropertyInfo>);
            var name = t.ToDisplayName();
            Assert.AreEqual("MemberDocumentation<PropertyInfo>", name);
        }

        [TestMethod]
        public void GetTypeDisplayName02()
        {
            var t = typeof(DocumentationParser).Assembly.GetTypes().Where(x => x.Name.StartsWith("MemberDocumentation")).FirstOrDefault();
            Assert.IsNotNull(t);
            var name = t.ToDisplayName();
            Assert.AreEqual("MemberDocumentation<TMember>", name);
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
            var f = this.GetFactory();
            var p = f.GetParser(ParserKeys.Bootstrap);
            var type = p.GetDocumentation(typeof(Card));
            var props = p.GetProperties(type);

            Assert.AreNotEqual(0, props.Count());
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
            var p = this.GetFactory().GetParser(ParserKeys.Bootstrap);
            var doc = p.GetDocumentation(typeof(AlertLink));
            Assert.IsNotNull(doc);
        }

        [TestMethod]
        public void ReadDocs02()
        {
            var p = this.GetFactory().GetParser(ParserKeys.XmlDocsComponenents);
            var doc = p.GetDocumentation(typeof(Components.Xml.CNode));
            Assert.IsNotNull(doc);
        }


        private DocumentationParserFactory GetFactory()
        {
            var f = new DocumentationParserFactory();
            f.AddParser(ParserKeys.Bootstrap, typeof(Blazorade.Bootstrap.Components._Imports).Assembly);
            f.AddParser(ParserKeys.Core, typeof(Blazorade.Core._Imports).Assembly);
            f.AddParser(ParserKeys.XmlDocs, typeof(Blazorade.XmlDocumentation.DocumentationParser).Assembly);
            f.AddParser(ParserKeys.XmlDocsComponenents, typeof(Blazorade.XmlDocumentation.Components._Imports).Assembly);

            return f;
        }
    }
}
