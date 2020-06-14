using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blazorade.XmlDocumentation.Components
{
    /// <summary>
    /// Displays a list of events.
    /// </summary>
    partial class EventList
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
        /// A filter that is used to filter the events shown in the list.
        /// </summary>
        /// <remarks>
        /// If set to <c>null</c>, all events are shown.
        /// </remarks>
        [Parameter]
        public Func<EventDocumentation, bool> Filter { get; set; }



        /// <summary>
        /// A collection of events to show in the list.
        /// </summary>
        protected IEnumerable<EventDocumentation> Events { get; set; }


        /// <summary>
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            this.Events = from x in this.Parser.GetEvents(this.Documentation)
                          where (null == this.Filter || this.Filter(x))
                          select x;
        }

    }
}
