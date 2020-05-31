using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Blazorade.XmlDocumentation
{
    /// <summary>
    /// Represents the documentation for a <see cref="Type"/> object.
    /// </summary>
    public class TypeDocumentation : MemberDocumentation<Type>
    {
        /// <inheritdoc/>
        public TypeDocumentation(XmlNode documentation, Type documentedType) : base(documentation, documentedType) { }

        /// <inheritdoc/>
        public override Type DocumentedMember
        {
            get { return base.DocumentedMember; }
            protected set
            {
                base.DocumentedMember = value;
                this.Namespace = this.DocumentedMember.Namespace;
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
            return $"Type: {this.DocumentedMember.FullName}";
        }

    }
}
