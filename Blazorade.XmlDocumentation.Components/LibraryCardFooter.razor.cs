using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazorade.XmlDocumentation.Components
{
    /// <summary>
    /// Represents the footer of a <see cref="LibraryCard"/> component.
    /// </summary>
    partial class LibraryCardFooter
    {

        /// <summary>
        /// The URL for the <c>Read more</c> link.
        /// </summary>
        /// <remarks>
        /// <para>If the URL is not specified, no link is rendered.</para>
        /// <para>Absolute URLs are considered as external links and are opened in a new browser tab. Relative URLs are opened in the same browser tab.</para>
        /// </remarks>
        [Parameter]
        public Uri ReadMoreUrl { get; set; }

        /// <summary>
        /// The label for the <c>Read more</c> link.
        /// </summary>
        [Parameter]
        public string ReadMoreLabel { get; set; }

    }
}
