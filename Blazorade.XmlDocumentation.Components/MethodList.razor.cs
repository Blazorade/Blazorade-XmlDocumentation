using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazorade.XmlDocumentation.Components
{
    /// <summary>
    /// A component that lists methods from the type specified in the <see cref="TypeDocumentationBase.TypeName"/> parameter.
    /// </summary>
    partial class MethodList
    {

        /// <summary>
        /// Allows you to provide a heading for the list.
        /// </summary>
        /// <remarks>
        /// This template is rendered only if the list has methods to show.
        /// </remarks>
        [Parameter]
        public RenderFragment HeadingTemplate { get; set; }

        /// <summary>
        /// A filter that is used to filter the properties shown in the list.
        /// </summary>
        /// <remarks>
        /// If set to <c>null</c>, all properties are shown.
        /// </remarks>
        [Parameter]
        public Func<MethodDocumentation, bool> MethodFilter { get; set; }



        /// <summary>
        /// A collection of methods to list.
        /// </summary>
        protected IEnumerable<MethodDocumentation> Methods { get; set; }

        /// <summary>
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            this.Methods = this.Parser.GetMethods(this.Documentation);
        }
    }
}
