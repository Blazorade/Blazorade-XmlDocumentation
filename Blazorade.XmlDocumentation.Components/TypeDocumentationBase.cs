using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazorade.XmlDocumentation.Components
{
    /// <summary>
    /// A base class for all components handling the type specified in <see cref="TypeName"/>.
    /// </summary>
    public class TypeDocumentationBase : DocumentationComponentBase
    {

        /// <summary>
        /// The full name of the type handled in the current component.
        /// </summary>
        [Parameter]
        public string TypeName { get; set; }

        /// <summary>
        /// The documentation for the type specified in <see cref="TypeName"/>.
        /// </summary>
        protected TypeDocumentation Documentation { get; set; }


        /// <summary>
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            this.Documentation = this.Parser.GetDocumentation(this.TypeName);
        }
    }
}
