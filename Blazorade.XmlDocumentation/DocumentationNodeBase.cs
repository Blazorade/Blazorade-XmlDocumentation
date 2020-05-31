using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;

namespace Blazorade.XmlDocumentation
{
    public abstract class DocumentationNodeBase
    {
        protected DocumentationNodeBase(XmlNode documentation)
        {
            this.DocumentationNode = documentation ?? throw new ArgumentNullException(nameof(documentation));
        }



        /// <summary>
        /// Returns the XML node representing the documentation.
        /// </summary>
        public XmlNode DocumentationNode { get; private set; }



        /// <summary>
        /// Returns the child nodes with the given name.
        /// </summary>
        /// <param name="name">The name of the child nodes to return.</param>
        protected IEnumerable<XmlNode> ChildNodes(string name)
        {
            return from XmlNode x in this.DocumentationNode.SelectNodes(name) select x;
        }

    }
}
