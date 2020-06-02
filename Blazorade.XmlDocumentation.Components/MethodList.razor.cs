using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Blazorade.XmlDocumentation.Components
{
    /// <summary>
    /// A component that lists methods from the type specified in the <see cref="TypeDocumentationBase.TypeName"/> parameter.
    /// </summary>
    partial class MethodList
    {

        /// <summary>
        /// A filter expression that includes only constructor methods.
        /// </summary>
        public static readonly Func<MethodDocumentation, bool> ConstructorFilter = (m) => m.Member.IsConstructor;

        /// <summary>
        /// A filter expression that includes all other methods except for constructors.
        /// </summary>
        public static readonly Func<MethodDocumentation, bool> NonConstructorFilter = (m) => !ConstructorFilter(m);



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
        /// If set to <c>null</c>, all methods are shown.
        /// </remarks>
        [Parameter]
        public Func<MethodDocumentation, bool> Filter { get; set; }



        /// <summary>
        /// A collection of methods to list.
        /// </summary>
        protected IEnumerable<MethodDocumentation> Methods { get; set; }

        /// <summary>
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            this.Methods = from x in this.Parser.GetMethods(this.Documentation)
                           where (null == this.Filter || this.Filter(x))
                           select x;
        }
    }
}
