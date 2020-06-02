using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Blazorade.XmlDocumentation
{
    /// <summary>
    /// Represents the documentation for a <see cref="Type"/> object.
    /// </summary>
    public class TypeDocumentation : MemberDocumentation
    {
        /// <inheritdoc/>
        public TypeDocumentation(XmlNode documentation, Type member) : base(documentation, member) { }

        /// <inheritdoc/>
        public new Type Member
        {
            get { return (Type)base.Member; }
            protected set
            {
                base.Member = value;
                this.Namespace = this.Member.Namespace;
            }
        }

        /// <summary>
        /// Returns the namespace of the type.
        /// </summary>
        public string Namespace { get; private set; }



        /// <summary>
        /// Returns the string representation of the object.
        /// </summary>
        public override string ToString()
        {
            return $"Type: {this.Member.FullName}";
        }

    }
}
