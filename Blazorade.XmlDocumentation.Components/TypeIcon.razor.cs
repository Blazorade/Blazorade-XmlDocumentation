using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.XmlDocumentation.Components
{
    partial class TypeIcon
    {

        /// <summary>
        /// The type for which to show the icon.
        /// </summary>
        [Parameter]
        public Type Type { get; set; }

        /// <summary>
        /// The URL of the icon.
        /// </summary>
        protected string IconUrl { get; set; }


        /// <summary>
       /// </summary>
        protected async override Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            if(null != this.Type)
            {
                if(this.Type.IsClass)
                {
                    this.IconUrl = "/_content/Blazorade.XmlDocumentation.Components/Class_16x.svg";
                }
                else if(this.Type.IsInterface)
                {
                    this.IconUrl = "/_content/Blazorade.XmlDocumentation.Components/Interface_16x.svg";
                }
                else if (this.Type.IsEnum)
                {
                    this.IconUrl = "/_content/Blazorade.XmlDocumentation.Components/Enumerator_16x.svg";
                }
                else if (this.Type.IsValueType)
                {
                    this.IconUrl = "/_content/Blazorade.XmlDocumentation.Components/Structure_16x.svg";
                }
                else if(this.Type.IsPointer)
                {
                    this.IconUrl = "/_content/Blazorade.XmlDocumentation.Components/Delegate_16x.svg";
                }
            }
        }
    }
}
