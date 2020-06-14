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
    /// <para>
    /// The type specified in <see cref="TypeName"/>.
    /// </para>
    /// <para>
    /// If you specify <see cref="TypeName"/> then you don't have to specify <see cref="Namespace"/>, because that is resolved
    /// from <see cref="TypeName"/>.
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>Member</term>
    /// <description>
    /// <para>
    /// The member specified in <see cref="MemberName"/>.
    /// </para>
    /// <para>
    /// If you specify <see cref="MemberName"/>, then you don't have to specify <see cref="TypeName"/> nor <see cref="Namespace"/>, because
    /// they are resolved from <see cref="MemberName"/>.
    /// </para>
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

        private string _MemberShortName;
        private string _MemberName;
        /// <summary>
        /// The member name of the member to use when building the breadcrumb.
        /// </summary>
        [Parameter]
        public string MemberName
        {
            get { return _MemberName; }
            set
            {
                _MemberShortName = null;
                _MemberName = value;
                if(_MemberName?.Length > 0)
                {
                    this.TypeName = _MemberName.Substring(0, _MemberName.LastIndexOf('.'));
                    _MemberShortName = _MemberName.Substring(_MemberName.LastIndexOf('.') + 1);
                    if (this.TypeName.EndsWith('.'))
                    {
                        this.TypeName = this.TypeName.Substring(0, this.TypeName.Length - 1);
                    }
                }
            }
        }

        /// <summary>
        /// The namespace to use when building the breadcrumb.
        /// </summary>
        /// <remarks>
        /// This parameter is ignored if <see cref="TypeName"/> is specified. The namespace shown is defined by that type name.
        /// </remarks>
        [Parameter]
        public string Namespace { get; set; }

        private Type _Type;
        private string _TypeName;
        /// <summary>
        /// The type name of the type to use when building the breadcrumb.
        /// </summary>
        [Parameter]
        public string TypeName
        {
            get { return _TypeName; }
            set
            {
                _Type = null;
                _TypeName = value;
                if(_TypeName?.Length > 0)
                {
                    _Type = this.Parser.GetType(_TypeName);
                    if(null != _Type)
                    {
                        this.Namespace = _Type.Namespace;
                    }
                }
            }
        }




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

            if(this.Namespace?.Length > 0)
            {
                this.Items.Add(new Link { Text = this.Namespace, Url = this.UriBuilder.GetNamespaceUri(this.LibraryKey, this.Namespace)?.ToString() });
            }

            if(null != _Type)
            {
                this.Items.Add(new Link { Text = _Type.ToDisplayName(), Url = this.UriBuilder.GetTypeUri(this.LibraryKey, _Type)?.ToString() });
            }

            if(this.MemberName?.Length > 0)
            {
                this.Items.Add(new Link { Text = _MemberShortName });
            }

            this.Items.Last().IsActive = true;
            this.Items.Last().Url = null;
        }
    }
}
