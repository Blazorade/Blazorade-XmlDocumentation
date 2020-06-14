using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazorade.XmlDocumentation.Components
{
    partial class MemberDetailsView
    {

        [Parameter]
        public MemberDocumentation MemberDocumentation { get; set; }


        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            var name = this.MemberDocumentation.Member.ToDisplayName();
        }
    }
}
