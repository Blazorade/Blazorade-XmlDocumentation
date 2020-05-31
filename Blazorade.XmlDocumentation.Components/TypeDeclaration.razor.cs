using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.XmlDocumentation.Components
{
    /// <summary>
    /// This component outputs just the declaration of the type specified in the <see cref="Type"/> parameter.
    /// </summary>
    partial class TypeDeclaration
    {

        /// <summary>
        /// Outputs everything in lower case if set to <c>true</c>.
        /// </summary>
        [Parameter]
        public bool LowerCase { get; set; }

        /// <summary>
        /// The type for which to produce the declaration.
        /// </summary>
        [Parameter]
        public Type Type { get; set; }


        /// <summary>
        /// The type declaration to output in the component.
        /// </summary>
        protected string Declaration { get; set; }


        /// <summary>
        /// </summary>
        protected async override Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            if(null != this.Type)
            {
                if (this.Type.IsClass)
                {
                    this.Declaration = "Class";
                }
                else if (this.Type.IsEnum)
                {
                    this.Declaration = "Enumeration";
                }
                else if (this.Type.IsValueType)
                {
                    this.Declaration = "Struct";
                }
                else if(this.Type.IsInterface)
                {
                    this.Declaration = "Interface";
                }
                else if(this.Type.IsPointer)
                {
                    this.Declaration = "Delegate";
                }
            }

            if (this.LowerCase)
            {
                this.Declaration = this.Declaration?.ToLower();
            }
        }
    }
}
