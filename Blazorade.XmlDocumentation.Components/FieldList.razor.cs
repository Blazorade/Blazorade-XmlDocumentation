using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Blazorade.XmlDocumentation.Components
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>
    /// <para>
    /// This example shows you a simple way of using the <c>FieldList</c> component.
    /// </para>
    /// <code>
    /// <FieldList LibraryKey="..." TypeName="...">
    ///     <HeadingTemplate>
    ///         <Heading Level="2">Fields</Heading>
    ///     </HeadingTemplate>
    /// </FieldList>
    /// </code>
    /// </example>
    partial class FieldList
    {

        /// <summary>
        /// Allows you to specify how the heading for the list is rendered.
        /// </summary>
        /// <remarks>
        /// This tempate is only rendered if there are any fields to show in the list.
        /// </remarks>
        [Parameter]
        public RenderFragment HeadingTemplate { get; set; }

        /// <summary>
        /// A filter expression that is used to filter the fields shown in the list.
        /// </summary>
        /// <remarks>
        /// If not specified, all fields are listed.
        /// </remarks>
        [Parameter]
        public Func<FieldDocumentation, bool> Filter { get; set; }


        /// <summary>
        /// The filtered list of fields to show in the component.
        /// </summary>
        protected IEnumerable<FieldDocumentation> Fields { get; set; }


        /// <summary>
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            this.Fields = from x in this.Parser.GetFields(this.Documentation)
                          where (null == this.Filter || this.Filter(x))
                          orderby x.Name
                          select x;

        }
    }
}
