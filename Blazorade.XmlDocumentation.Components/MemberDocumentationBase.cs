using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Blazorade.XmlDocumentation.Components
{
    /// <summary>
    /// A base class for creating documentation for different types of members, such as:
    /// <list type="bullet">
    /// <item><c>Method</c></item>
    /// <item><c>Property</c></item>
    /// <item><c>Field</c></item>
    /// <item><c>Event</c></item>
    /// </list>
    /// </summary>
    public class MemberDocumentationBase : DocumentationComponentBase
    {

        /// <summary>
        /// The name of the member.
        /// </summary>
        [Parameter]
        public string MemberName { get; set; }


        /// <summary>
        /// The documentation associated with the member specified in <see cref="MemberName"/>.
        /// </summary>
        protected IEnumerable<MemberDocumentation> Documentation { get; set; }



        /// <summary>
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            
        }
    }
}
