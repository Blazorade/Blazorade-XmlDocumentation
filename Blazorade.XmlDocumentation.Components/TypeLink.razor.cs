using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using Blazorade.XmlDocumentation.Components.Services;
using System.Threading.Tasks;

namespace Blazorade.XmlDocumentation.Components
{
    /// <summary>
    /// Creates a link that points to the type specified in <see cref="Type"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This component requires that the <see cref="DocumentationUriBuilder"/> (or a derived implementation) has been
    /// added to the services collection of your application.
    /// </para>
    /// <para>
    /// By convention, links to relative URIs are opened in the same browser tag. Absolute URIs are considered as external links and opened in a new browser tab.
    /// </para>
    /// </remarks>
    partial class TypeLink
    {

        /// <summary>
        /// The type to create a link to.
        /// </summary>
        [Parameter]
        public Type Type { get; set; }

        /// <summary>
        /// The URL to show in the link.
        /// </summary>
        protected Uri Url { get; set; }

        /// <summary>
        /// The text to show in the link.
        /// </summary>
        protected string Text { get; set; }

        /// <summary>
        /// </summary>
        protected async override Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            if(null != this.Type)
            {
                string key = this.Factory.GetParserByTypeName(this.Type.FullName)?.Item1;
                this.Url = this.UriBuilder.GetTypeUri(key, this.Type);
                
                if(this.Type.GenericTypeArguments?.Length > 0)
                {
                    this.Text = this.Type.Name.Substring(0, this.Type.Name.IndexOf('`'));
                }
                else
                {
                    this.Text = this.Type.Name;
                }
            }
        }
    }
}
