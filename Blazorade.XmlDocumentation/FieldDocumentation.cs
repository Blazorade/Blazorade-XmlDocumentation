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
    public class FieldDocumentation : MemberDocumentation
    {
        /// <inheritdoc/>
        public FieldDocumentation(XmlNode documentation, FieldInfo member) : base(documentation, member)
        {
        }


        /// <inheritdoc/>
        public new FieldInfo Member { get => (FieldInfo)base.Member; protected set => base.Member = value; }


        /// <summary>
        /// Returns the string representation of the object.
        /// </summary>
        public override string ToString()
        {
            return $"Field: {this.Member.DeclaringType.FullName}.{this.Member.Name}";
        }

    }
}
