using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Blazorade.XmlDocumentation
{
    /// <summary>
    /// Represents documentation for a field.
    /// </summary>
    public class FieldDocumentation : MemberDocumentation<FieldInfo>
    {
        /// <inheritdoc/>
        public FieldDocumentation(XmlNode documentation, FieldInfo member) : base(documentation, member)
        {
        }


        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Field: {this.Member.ToDisplayName()}";
        }

    }
}
