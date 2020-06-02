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
    public class MethodDocumentation : MemberDocumentation
    {
        /// <inheritdoc/>
        public MethodDocumentation(XmlNode documentation, MethodBase member) : base(documentation, member) { }


        /// <inheritdoc/>
        public new MethodBase Member
        {
            get => (MethodBase)base.Member;
            set => base.Member = value;
        }

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
            foreach(var p in this.Member.GetParameters())
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
