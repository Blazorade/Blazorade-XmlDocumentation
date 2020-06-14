using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Blazorade.XmlDocumentation.Components.Services
{
    /// <summary>
    /// An implementation that is used by components to build URIs with. URIs built with this
    /// implementation are used to point to various sections within the documentation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The URIs produced can be anything, both relative or absolute. By convention, components that
    /// use URIs produced by an implementation will treat absolute URIs as external links that are
    /// opened by default in a new browser tab.
    /// </para>
    /// <para>
    /// This can be configured by setting the <see cref="OpenExternalsInNewTab"/> property to <c>false</c>,
    /// which will result in external links being opened in the same browser tab.
    /// </para>
    /// </remarks>
    public class DocumentationUriBuilder
    {
        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        public DocumentationUriBuilder()
        {
            this.OpenExternalsInNewTab = true;
        }

        /// <summary>
        /// Specifies whether external links should be opened in a new browser tab.
        /// </summary>
        /// <remarks>
        /// Absolute URIs are considered as external links are are opened in a new browser tab by default.
        /// </remarks>
        public virtual bool OpenExternalsInNewTab { get; set; }

        /// <summary>
        /// Returns the URI that points to the page that documents the assembly represented by the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key that represents the documentation for an assembly that is loaded into the current <see cref="DocumentationParserFactory"/>.</param>
        public virtual Uri GetAssemblyUri(string key)
        {
            if(key?.Length > 0)
            {
                return new Uri($"/{key}", UriKind.Relative);
            }

            return null;
        }

        /// <summary>
        /// Returns the URI for the given member.
        /// </summary>
        /// <param name="key">The key that specifies library the member originates from. If <c>null</c>, the given member is considered an external member.</param>
        /// <param name="member">The member to return the URI for.</param>
        public virtual Uri GetMemberUri(string key, MemberInfo member)
        {
            var name = member?.ToFullName();

            if(key?.Length > 0 && name?.Length > 0)
            {
                return new Uri($"/{key}/m/{name}", UriKind.Relative);
            }
            else if(name?.Length > 0)
            {
                return new Uri($"https://www.google.com/search?q={name}", UriKind.Absolute);
            }

            return null;
        }

        /// <summary>
        /// Returns the URI for the given namespace.
        /// </summary>
        /// <remarks>
        /// If <paramref name="key"/> is <c>null</c>, it means that the given <paramref name="namespace"/> is not from any of the parsers loaded into
        /// <see cref="DocumentationParserFactory"/>, which indicates that the <paramref name="namespace"/> is not documented by the running application.
        /// </remarks>
        /// <param name="key">The key that specifies the parser in <see cref="DocumentationParserFactory"/> that produced the given <paramref name="namespace"/>.</param>
        /// <param name="namespace">The namespace to create the URI for.</param>
        public virtual Uri GetNamespaceUri(string key, string @namespace)
        {
            if(key?.Length > 0 && @namespace?.Length > 0)
            {
                return new Uri($"/{key}/ns/{@namespace}", UriKind.Relative);
            }
            else if(@namespace?.Length > 0)
            {
                return new Uri($"https://www.google.com/search?q={@namespace}+namespace", UriKind.Absolute);
            }

            return null;
        }

        /// <summary>
        /// Returns the URI for the given type.
        /// </summary>
        /// <remarks>
        /// If <paramref name="key"/> is <c>null</c>, it means that the given <paramref name="type"/> is not from any of the parsers loaded into
        /// <see cref="DocumentationParserFactory"/>, which indicates that the <paramref name="type"/> is not documented by the running application.
        /// </remarks>
        /// <param name="key">The key that specifies the parser in <see cref="DocumentationParserFactory"/> that produced the given <paramref name="type"/>.</param>
        /// <param name="type">The type for which to return the URI.</param>
        public virtual Uri GetTypeUri(string key, Type type)
        {
            if (key?.Length > 0 && null != type)
            {
                return new Uri($"/{key}/t/{type.FullName}", UriKind.Relative);
            }
            else if(null != type)
            {
                return new Uri($"https://www.google.com/search?q={type.Namespace}.{type.Name}", UriKind.Absolute);
            }

            return null;
        }

    }
}
