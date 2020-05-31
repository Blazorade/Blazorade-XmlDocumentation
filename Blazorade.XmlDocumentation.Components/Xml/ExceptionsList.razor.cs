using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;

namespace Blazorade.XmlDocumentation.Components.Xml
{
    /// <summary>
    /// Builds a list of exception documentation from the <see cref="SourceNodes"/> node list.
    /// </summary>
    partial class ExceptionsList
    {

        /// <summary>
        /// A collection of nodes to search for <c>&lt;exception /&gt;</c> nodes in.
        /// </summary>
        /// <remarks>
        /// The list specified does not have to be filtered to include only <c>exception</c> elements. The rendering logic will take care of that filtering.
        /// </remarks>
        [Parameter]
        public XmlNodeList SourceNodes { get; set; }


        /// <summary>
        /// The exception nodes to display in the list.
        /// </summary>
        protected IEnumerable<XmlNode> ExceptionNodes { get; set; }



        /// <summary>
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if(null != this.SourceNodes)
            {
                this.ExceptionNodes = from XmlNode x in this.SourceNodes select x;
            }
            else
            {
                this.ExceptionNodes = new XmlNode[0];
            }
            
        }
    }
}
