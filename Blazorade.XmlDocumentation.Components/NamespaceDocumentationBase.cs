using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazorade.XmlDocumentation.Components
{
    /// <summary>
    /// A base class for components that handle a given namespace.
    /// </summary>
    public abstract class NamespaceDocumentationBase : DocumentationComponentBase
    {

        /// <summary>
        /// The namespace to process in the derived component.
        /// </summary>
        [Parameter]
        public string Namespace { get; set; }

    }
}
