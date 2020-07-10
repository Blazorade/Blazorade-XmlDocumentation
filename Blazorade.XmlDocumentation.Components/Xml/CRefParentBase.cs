using Blazorade.XmlDocumentation.Components.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Text;

namespace Blazorade.XmlDocumentation.Components.Xml
{
    /// <summary>
    /// Base class for nodes that contain a <c>cref</c> attribute.
    /// </summary>
    /// <remarks>
    /// <code></code>
    /// </remarks>
    public abstract class CRefParentBase : NodeComponentBase
    {

        /// <summary>
        /// The URI builder used by the component.
        /// </summary>
        [Inject]
        protected DocumentationUriBuilder UriBuilder { get; set; }

        /// <summary>
        /// A documentation parser factory that the component uses.
        /// </summary>
        [Inject]
        protected DocumentationParserFactory Factory { get; set; }

        /// <summary>
        /// The URI that the <c>cref</c> attribute points to.
        /// </summary>
        protected Uri LinkUrl { get; set; }

        /// <summary>
        /// The text to show in the link.
        /// </summary>
        protected string LinkText { get; set; }

        /// <summary>
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            var crefValue = this.Node?.Attributes["cref"]?.Value;
            if(null != crefValue)
            {
                CRef cref = new CRef(crefValue);
                switch(cref.CRefType)
                {
                    case CRefType.Type:
                        var t = cref.ToType();
                        this.LinkText = t.Name;
                        var result = this.Factory.GetParserByType(t);
                        this.LinkUrl = this.UriBuilder.GetTypeUri(result?.Item1, t);
                        break;
                }
            }
        }

    }
}
