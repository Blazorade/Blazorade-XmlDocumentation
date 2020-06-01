using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Blazorade.XmlDocumentation.Components
{
    /// <summary>
    /// A component that lists properties.
    /// </summary>
    partial class PropertyList
    {

        /// <summary>
        /// Allows you to provide a heading for the list.
        /// </summary>
        /// <remarks>
        /// This template is rendered only if the list has properties to show.
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
        public Func<PropertyDocumentation, bool> Filter { get; set; }



        /// <summary>
        /// A collection of properties to list.
        /// </summary>
        protected IEnumerable<PropertyDocumentation> Properties { get; set; }


        /// <summary>
        /// </summary>
        protected async override Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();


            this.Properties = from x in this.Parser.GetProperties(this.Documentation)
                              where (null == this.Filter || this.Filter(x))
                              orderby x.Name
                              select x;

        }
    }
}
