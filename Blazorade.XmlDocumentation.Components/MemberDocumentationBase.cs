using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// The full name of the member.
        /// </summary>
        [Parameter]
        public string MemberName { get; set; }



        /// <summary>
        /// The documentation associated with the member specified in <see cref="MemberName"/>.
        /// </summary>
        protected IEnumerable<MemberDocumentation> Documentation { get; set; }

        /// <summary>
        /// The display name of the member.
        /// </summary>
        protected string MemberDisplayName { get; set; }

        /// <summary>
        /// The type name of the member.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Can be one of the following.
        /// </para>
        /// <list type="bullet">
        /// <item>Field</item>
        /// <item>Property</item>
        /// <item>Method</item>
        /// <item>Event</item>
        /// </list>
        /// </remarks>
        protected string MemberTypeName { get; set; }

        /// <summary>
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            this.MemberDisplayName = this.MemberName.Substring(this.MemberName.LastIndexOf('.') + 1);
            this.Documentation = this.Parser.GetMembers(this.MemberName);
            var doc = this.Documentation.FirstOrDefault();
            if(null != doc)
            {
                this.MemberTypeName = doc.Member.MemberType.ToString();
                if(doc.Member.MemberType == MemberTypes.Constructor)
                {
                    this.MemberDisplayName = "." + this.MemberDisplayName;
                }
            }
        }
    }
}
