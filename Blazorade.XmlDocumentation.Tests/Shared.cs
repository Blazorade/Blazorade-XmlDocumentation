using System;
using System.Collections.Generic;
using System.Text;

namespace Blazorade.XmlDocumentation.Tests
{
    public static class Shared
    {

        public static DocumentationParserFactory GetFactory()
        {
            var f = new DocumentationParserFactory();
            f.AddParser(ParserKeys.Bootstrap, typeof(Blazorade.Bootstrap.Components._Imports).Assembly);
            f.AddParser(ParserKeys.Core, typeof(Blazorade.Core._Imports).Assembly);
            f.AddParser(ParserKeys.XmlDocs, typeof(Blazorade.XmlDocumentation.DocumentationParser).Assembly);
            f.AddParser(ParserKeys.XmlDocsComponenents, typeof(Blazorade.XmlDocumentation.Components._Imports).Assembly);
            f.AddParser(ParserKeys.TestLib, typeof(TestLibrary.SomeNamespace.Class1).Assembly);
            return f;
        }

    }
}
