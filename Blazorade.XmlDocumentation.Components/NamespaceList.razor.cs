using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Blazorade.XmlDocumentation.Components
{
    /// <summary>
    /// Lists all namespaces from the underlying <see cref="DocumentationComponentBase.Parser"/>.
    /// </summary>
    partial class NamespaceList
    {

        /// <summary>
        /// The namespaces to show in the list.
        /// </summary>
        protected IEnumerable<string> Namespaces { get; set; }

        /// <summary>
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            this.Namespaces = from x in this.Parser?.GetNamespaces() ?? new List<string>() orderby x select x;
        }
    }
}
