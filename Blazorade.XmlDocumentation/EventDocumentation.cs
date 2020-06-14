using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Blazorade.XmlDocumentation
{
    /// <summary>
    /// Represents documentation for an event.
    /// </summary>
    public class EventDocumentation : MemberDocumentation<EventInfo>
    {
        /// <inheritdoc/>
        public EventDocumentation(XmlNode documentation, EventInfo member) : base(documentation, member)
        {
        }


        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Event: {this.Member.ToDisplayName()}";
        }
    }
}
