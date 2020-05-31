using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using Blazorade.XmlDocumentation.Components.Services;
using Blazorade.Bootstrap.Components.Model;
using System.Linq;

namespace Blazorade.XmlDocumentation.Components
{

    /// <summary>
    /// A breadcrumb that helps navigating between documentation pages.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This breadcrumb component shows the following structure based on the parameters set on the component.
    /// </para>
    /// <para>
    /// Home / Library (assembly) / Namespace / Type
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <term>Home</term>
    /// <description>The link to the home page. If <see cref="HomeUrl"/> is set to <c>null</c>, the home link is not shown in the breadcrumb.</description>
    /// </item>
    /// <item>
    /// <term>Library</term>
    /// <description>
    /// The assembly or library being documented. Points to the URI returned by the <see cref="DocumentationUriBuilder.GetAssemblyUri(string)"/> 
    /// where the parameter is the key set on the <see cref="DocumentationComponentBase.LibraryKey"/> on this breadcrumb.
    /// </description>
    /// </item>
    /// <item>
    /// <term>Namespace</term>
    /// <description>
    /// The <see cref="Namespace"/> or the namespace of the type specified in <see cref="TypeName"/>.
    /// </description>
    /// </item>
    /// <item>
    /// <term>Type</term>
    /// <description>
    /// The type specified in <see cref="TypeName"/>.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    partial class DocumentationBreadcrumb : DocumentationComponentBase
    {
        /// <summary>
        /// </summary>
        public DocumentationBreadcrumb()
        {
            this.HomeLabel = "Home";
            this.HomeUrl = "/";
        }



        /// <summary>
        /// The label that is used to point to the home page.
        /// </summary>
        [Parameter]
        public string HomeLabel { get; set; }

        /// <summary>
        /// The URL to the home page. If set to <c>null</c>, the home link is not shown.
        /// </summary>
        [Parameter]
        public string HomeUrl { get; set; }

        /// <summary>
        /// The namespace to use when building the breadcrumb.
        /// </summary>
        /// <remarks>
        /// This parameter is ignored if <see cref="TypeName"/> is specified. The namespace shown is defined by that type name.
        /// </remarks>
        [Parameter]
        public string Namespace { get; set; }

        /// <summary>
        /// The type name of the type to use when building the breadcrumb.
        /// </summary>
        [Parameter]
        public string TypeName { get; set; }


        private List<Link> Items = new List<Link>();

        /// <summary>
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            this.Items.Clear();

            if(this.HomeUrl?.Length > 0)
            {
                this.Items.Add(new Link { Text = this.HomeLabel, Url = this.HomeUrl });
            }

            if(this.LibraryKey?.Length > 0)
            {
                this.Items.Add(new Link { Text = this.Parser.AssemblyProductName, Url = this.UriBuilder.GetAssemblyUri(this.LibraryKey)?.ToString() });
            }

            if (this.TypeName?.Length > 0)
            {
                var t = this.Parser.GetType(this.TypeName);
                if(null != t)
                {
                    this.Items.Add(new Link { Text = t.Namespace, Url = this.UriBuilder.GetNamespaceUri(this.LibraryKey, t.Namespace)?.ToString() });
                    this.Items.Add(new Link { Text = t.ToDisplayName(), Url = this.UriBuilder.GetTypeUri(this.LibraryKey, t)?.ToString() });
                }
            }
            else if (this.Namespace?.Length > 0)
            {
                this.Items.Add(new Link { Text = this.Namespace, Url = this.UriBuilder.GetNamespaceUri(this.LibraryKey, this.Namespace)?.ToString() });
            }

            this.Items.Last().IsActive = true;
            this.Items.Last().Url = null;
        }
    }
}
