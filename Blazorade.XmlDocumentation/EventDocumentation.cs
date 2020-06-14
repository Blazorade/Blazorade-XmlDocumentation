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
    public class EventDocumentation : MemberDocumentation
    {
        /// <inheritdoc/>
        public EventDocumentation(XmlNode documentation, EventInfo member) : base(documentation, member)
        {
        }


        /// <inheritdoc/>
        public new EventInfo Member
        {
            get => (EventInfo)base.Member;
            set => base.Member = value;
        }

    }
}
