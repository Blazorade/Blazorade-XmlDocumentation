using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Linq;

namespace Blazorade.XmlDocumentation
{
    /// <summary>
    /// Represents the base class for all kinds of member documentation nodes.
    /// </summary>
    public abstract class MemberDocumentation : DocumentationNodeBase
    {
        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="documentation">The XML node representing the documentation for the member.</param>
        /// <param name="documentedMember">The member that <paramref name="documentation"/> represents.</param>
        protected MemberDocumentation(XmlNode documentation, MemberInfo documentedMember) : base(documentation)
        {
            this.Member = documentedMember ?? throw new ArgumentNullException(nameof(documentedMember));
        }



        private MemberInfo _Member;
        /// <summary>
        /// Returns the reflection member that the documenation represents.
        /// </summary>
        public virtual MemberInfo Member
        {
            get { return _Member; }
            protected set
            {
                _Member = value;
                this.Name = _Member?.Name;
            }
        }

        /// <summary>
        /// Returns the name of the member.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Returns the <c>summary</c> node from the documentation.
        /// </summary>
        public XmlNode Summary
        {
            get { return this.ChildNodes("summary").FirstOrDefault(); }
        }

        /// <summary>
        /// Returns the <c>remarks</c> node from the documentation.
        /// </summary>
        public XmlNode Remarks
        {
            get { return this.ChildNodes("remarks").FirstOrDefault(); }
        }

        /// <summary>
        /// Returns the <c>exception</c> nodes from the documentation.
        /// </summary>
        public IEnumerable<XmlNode> Exceptions
        {
            get { return this.ChildNodes("exception"); }
        }

        /// <summary>
        /// Returns the <c>example</c> nodes from the documentation.
        /// </summary>
        public IEnumerable<XmlNode> Examples
        {
            get { return this.ChildNodes("example"); }
        }



        /// <summary>
        /// Returns the string representation of the documentation.
        /// </summary>
        public override string ToString()
        {
            if(null != this.Member?.DeclaringType)
            {
                return $"{this.Member.MemberType}: {this.Member.DeclaringType.FullName}.{this.Member.Name}";
            }

            return base.ToString();
        }

    }

    public abstract class MemberDocumentation<TMember> : MemberDocumentation where TMember : MemberInfo
    {
        protected MemberDocumentation(XmlNode documentation, TMember member) : base(documentation, member)
        {
        }

        public new TMember Member { get => (TMember)base.Member; protected set => base.Member = value; }
    }
}
