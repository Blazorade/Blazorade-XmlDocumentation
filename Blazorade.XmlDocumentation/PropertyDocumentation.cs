using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Blazorade.XmlDocumentation
{
    /// <summary>
    /// Represents documentation for a property, encapsulating a <see cref="PropertyInfo"/> type.
    /// </summary>
    public class PropertyDocumentation : MemberDocumentation<PropertyInfo>
    {
        /// <inheritdoc/>
        public PropertyDocumentation(XmlNode documentation, PropertyInfo member) : base(documentation, member)
        {
        }


        /// <summary>
        /// Returns the <c>value</c> node from the documentation.
        /// </summary>
        /// <remarks>
        /// For details on the <c>value</c> node, see https://docs.microsoft.com/dotnet/csharp/programming-guide/xmldoc/value.
        /// </remarks>
        public XmlNode Value
        {
            get { return this.DocumentationNode?.SelectSingleNode("value"); }
        }



        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Property: {this.Member.ToDisplayName()}";
        }
    }
}
