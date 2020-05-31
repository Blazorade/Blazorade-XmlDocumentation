using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Blazorade.XmlDocumentation
{
    /// <summary>
    /// Represents documentation for a method, encapsulating a <see cref="MethodBase"/> type.
    /// </summary>
    /// <remarks>
    /// This documentation type is used for both methods and constructors.
    /// </remarks>
    public class MethodDocumentation : MemberDocumentation<MethodBase>
    {
        /// <inheritdoc/>
        public MethodDocumentation(XmlNode documentation, MethodBase documentedMember) : base(documentation, documentedMember) { }



        /// <inheritdoc/>
        public override MethodBase DocumentedMember
        { 
            get => base.DocumentedMember;
            protected set
            {
                base.DocumentedMember = value;
                
            }
        }

        /// <summary>
        /// Returns the display name for the method.
        /// </summary>
        /// <remarks>
        /// The display name is unique within its declaring type, because the display name contains
        /// also the parameter types.
        /// </remarks>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Returns the <c>param</c> nodes from the documentation.
        /// </summary>
        public IEnumerable<XmlNode> Parameters
        {
            get { return this.ChildNodes("param"); }
        }

        /// <summary>
        /// The <c>returns</c> node from the documentation.
        /// </summary>
        public XmlNode Returns
        {
            get { return this.ChildNodes("returns").FirstOrDefault(); }
        }



        /// <summary>
        /// Returns the parameters for the current method.
        /// </summary>
        public IEnumerable<ParameterDocumentation> GetParameters()
        {
            foreach(var p in this.DocumentedMember.GetParameters())
            {
                var node = this.Parameters.FirstOrDefault(x => x.Attributes["name"].Value == p.Name);
                if(null != node)
                {
                    yield return new ParameterDocumentation(node, p);
                }
            }
            yield break;
        }

    }
}
