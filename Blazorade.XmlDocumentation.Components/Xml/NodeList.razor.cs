using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Blazorade.XmlDocumentation.Components.Xml
{
    /// <summary>
    /// Represents a set of XML nodes in XML documentation. Renders each node appropriately.
    /// </summary>
    /// <remarks>
    /// Rendering is implemented based on the <see cref="XmlNode.NodeType"/> of each node loaded into the <see cref="Nodes"/> property.
    /// </remarks>
    partial class NodeList
    {
        /// <summary>
        /// The collection of nodes to render.
        /// </summary>
        [Parameter]
        public XmlNodeList Nodes { get; set; }
    }
}
