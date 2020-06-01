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
    partial class TypeList
    {

        /// <summary>
        /// Allows you to provide a heading for the list.
        /// </summary>
        /// <remarks>
        /// This template is rendered only if the list has types to show.
        /// </remarks>
        [Parameter]
        public RenderFragment HeadingTemplate { get; set; }

        /// <summary>
        /// Specifies a filter to apply to the types shown in the list.
        /// </summary>
        /// <remarks>
        /// <see cref="Namespace"/> is always used as filter, regardless of the other properties.
        /// </remarks>
        [Parameter]
        public Func<TypeDocumentation, bool> Filter { get; set; }

        /// <summary>
        /// The namespace whose types to show in the list.
        /// </summary>
        [Parameter]
        public string Namespace { get; set; }


        /// <summary>
        /// The types to show in the list.
        /// </summary>
        protected IEnumerable<TypeDocumentation> Types { get; set; }

        /// <summary>
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            this.Types = from x in this.Parser.GetTypes(this.Namespace)
                         where (null == this.Filter || this.Filter(x))
                         orderby x.DocumentedMember.FullName
                         select x;
        }
    }
}
