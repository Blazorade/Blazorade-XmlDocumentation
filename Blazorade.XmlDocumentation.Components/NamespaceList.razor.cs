using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace Blazorade.XmlDocumentation.Components
{
    /// <summary>
    /// Lists all namespaces from the underlying <see cref="DocumentationComponentBase.Parser"/>.
    /// </summary>
    partial class NamespaceList
    {

        /// <summary>
        /// A filter expression that is used to filter the namespaces shown in the list.
        /// </summary>
        /// <remarks>
        /// By default, all namespaces are shown.
        /// </remarks>
        [Parameter]
        public Func<string, bool> Filter { get; set; }

        /// <summary>
        /// The namespaces to show in the list.
        /// </summary>
        protected IEnumerable<string> Namespaces { get; set; }

        /// <summary>
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            this.Namespaces = from x in this.Parser?.GetNamespaces() ?? new List<string>()
                              where (null == this.Filter || this.Filter(x))
                              orderby x 
                              select x;
        }
    }
}
