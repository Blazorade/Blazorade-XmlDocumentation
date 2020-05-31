using System;
using System.Collections.Generic;
using System.Text;

namespace Blazorade.XmlDocumentation.Components.Xml
{
    /// <summary>
    /// Use this component with any XML documentation node that contains a <c>cref</c> attribute. This component will make the member reference
    /// in that <c>cref</c> attribute into a link that points to the documentation for the specified member.
    /// </summary>
    /// <remarks>
    /// If the <c>cref</c> attribute points to a type or member that is not handled by any of the <see cref="DocumentationParser"/> instances
    /// loaded into the current <see cref="DocumentationParserFactory"/> service, then the link will point to Google to search for the member.
    /// </remarks>
    partial class CrefAnchorNode
    {
    }
}
