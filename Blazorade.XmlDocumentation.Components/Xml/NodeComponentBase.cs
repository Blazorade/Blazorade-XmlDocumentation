using Blazorade.Bootstrap.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Blazorade.XmlDocumentation.Components.Xml
{
    /// <summary>
    /// The base class for components that render XML documentation nodes.
    /// </summary>
    public abstract class NodeComponentBase : BootstrapComponentBase
    {
        /// <summary>
        /// The node to render in the component.
        /// </summary>
        /// <remarks>
        /// Derived components must handle <c>null</c> values for this parameter.
        /// </remarks>
        [Parameter]
        public XmlNode Node { get; set; }
    }
}
