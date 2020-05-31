using Blazorade.XmlDocumentation.Components.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.XmlDocumentation.Components
{
    /// <summary>
    /// A card component that displays basic information about a library represented by an <see cref="DocumentationParser"/> instance
    /// loaded into a <see cref="DocumentationParserFactory"/> service.
    /// </summary>
    partial class LibraryCard
    {

        /// <summary>
        /// The URL to an image to show in the card.
        /// </summary>
        /// <remarks>
        /// If this parameter is not set, no image is shown in the card.
        /// </remarks>
        [Parameter]
        public string ImageUrl { get; set; }

        /// <summary>
        /// The label for the <c>Read more</c> link.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If this parameter is not set, the default <c>Read more...</c> is used.
        /// </para>
        /// <para>
        /// The URL for the <c>Read more</c> link is resolved by using the <see cref="DocumentationComponentBase.UriBuilder"/> instance
        /// using the <see cref="DocumentationComponentBase.LibraryKey"/> when calling the <see cref="DocumentationUriBuilder.GetAssemblyUri(string)"/> method.
        /// </para>
        /// </remarks>
        [Parameter]
        public string ReadMoreLabel { get; set; }


        /// <summary>
        /// The URL for the read more link.
        /// </summary>
        protected Uri ReadMoreUrl { get; set; }

        /// <summary>
        /// </summary>
        protected async override Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            this.ReadMoreUrl = this.UriBuilder.GetAssemblyUri(this.LibraryKey);
            this.ReadMoreLabel = this.ReadMoreLabel ?? "Read more...";
        }
    }
}
