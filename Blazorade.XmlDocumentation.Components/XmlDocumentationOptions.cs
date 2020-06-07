using Blazorade.XmlDocumentation.Components.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazorade.XmlDocumentation.Components
{
    public class XmlDocumentationOptions
    {

        public XmlDocumentationOptions()
        {
            this.UriBuilder = new DocumentationUriBuilder();
            this.Parsers = new Dictionary<string, DocumentationParser>();
        }

        public DocumentationUriBuilder UriBuilder { get; set; }

        public IDictionary<string, DocumentationParser> Parsers { get; set; }

    }
}
