using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime;
using System.Text;
using System.Linq;
using System.Security.Cryptography;

namespace Blazorade.XmlDocumentation
{

    /// <summary>
    /// Represents a <c>cref</c> attribute value in an XML documentation node.
    /// </summary>
    public class CRef
    {
        /// <summary>
        /// Creates an instance using the given <paramref name="rawValue"/>.
        /// </summary>
        /// <param name="rawValue">The raw value of the <c>cref</c> attribute. The raw value always contains the member type prefix.</param>
        public CRef(string rawValue)
        {
            if(!(rawValue?.Length > 0))
            {
                throw new ArgumentNullException(nameof(rawValue));
            }

            this.RawValue = rawValue;

            if(this.RawValue.Substring(1, 1) != ":")
            {
                throw new ArgumentException("The given value does not appear to be a valid CREF string.", nameof(rawValue));
            }

            var typeString = this.RawValue.Substring(0, 1).ToUpper();
            switch(typeString)
            {
                case "N": this.CRefType = CRefType.Namespace; break;
                case "T": this.CRefType = CRefType.Type; break;
                case "F": this.CRefType = CRefType.Field; break;
                case "P": this.CRefType = CRefType.Property; break;
                case "M": this.CRefType = CRefType.Method; break;
                case "E": this.CRefType = CRefType.Event; break;
                default:
                    throw new ArgumentException($"The given value specifies a type ('{typeString}') which is not supported.", nameof(rawValue));
            }

            this.Value = this.RawValue.Substring(2);
        }

        /// <summary>
        /// Converts <paramref name="cref"/> to its string representation.
        /// </summary>
        public static implicit operator string(CRef cref)
        {
            return cref.ToString();
        }


        /// <summary>
        /// Returns <c>true</c> if the current <see cref="CRef"/> is a namespace.
        /// </summary>
        public bool IsNamespace { get { return this.CRefType == CRefType.Namespace; } }

        /// <summary>
        /// Returns <c>true</c> if the current <see cref="CRef"/> is a type.
        /// </summary>
        public bool IsType { get { return this.CRefType == CRefType.Type; } }

        /// <summary>
        /// Returns <c>true</c> if the current <see cref="CRef"/> is a field.
        /// </summary>
        public bool IsField { get { return this.CRefType == CRefType.Field; } }

        /// <summary>
        /// Returns <c>true</c> if the current <see cref="CRef"/> is a property.
        /// </summary>
        public bool IsProperty { get { return this.CRefType == CRefType.Property; } }

        /// <summary>
        /// Returns <c>true</c> if the current <see cref="CRef"/> is a method.
        /// </summary>
        public bool IsMethod { get { return this.CRefType == CRefType.Method; } }

        /// <summary>
        /// Returns <c>true</c> if the current <see cref="CRef"/> is a constructor.
        /// </summary>
        public bool IsConstructor { get { return this.IsMethod && this.Value.Contains(".#ctor"); } }

        /// <summary>
        /// Returns <c>true</c> if the current <see cref="CRef"/> is an event.
        /// </summary>
        public bool IsEvent { get { return this.CRefType == CRefType.Event; } }

        /// <summary>
        /// Returns the raw value the <see cref="CRef"/> was created from.
        /// </summary>
        public string RawValue { get; private set; }

        /// <summary>
        /// Returns the type the <see cref="CRef"/> represents.
        /// </summary>
        public CRefType CRefType { get; private set; }

        /// <summary>
        /// Returns the trimmed value.
        /// </summary>
        public string Value { get; private set; }



        /// <summary>
        /// Returns the member defined by the current <see cref="CRef"/> instance.
        /// </summary>
        public MemberInfo ToMember()
        {
            MemberInfo mi = null;

            switch(this.CRefType)
            {
                case CRefType.Method:
                    mi = this.ToMethod();
                    break;

                case CRefType.Type:
                    mi = this.ToType();
                    break;
            }

            return mi;
        }

        /// <summary>
        /// Returns the property defined by the current <see cref="CRef"/> instance.
        /// </summary>
        public PropertyInfo ToProperty()
        {
            if (!this.IsProperty) throw new InvalidOperationException("The current object does not represent a property.");
            return null;
        }

        /// <summary>
        /// Returns the method defined by the current <see cref="CRef"/> instance.
        /// </summary>
        /// <returns>Returns the method or <c>null</c> if <see cref="IsMethod"/> returns <c>false</c>.</returns>
        public MethodBase ToMethod()
        {
            if (!this.IsMethod) throw new InvalidOperationException("The current object does not represent a method.");
            return this.Value.ToMethod();
        }

        /// <summary>
        /// Returns the <see cref="Type"/> that the current <see cref="CRef"/> instance points to.
        /// </summary>
        /// <remarks>
        /// Can only be called if <see cref="IsType"/> returns <c>true</c>.
        /// </remarks>
        /// <exception cref="InvalidOperationException">The exception that is thrown if <see cref="IsType"/> returns <c>false</c>.</exception>
        public Type ToType()
        {
            if(!this.IsType) throw new InvalidOperationException("The current object does not represent a Type.");
            return this.Value.ToType();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.RawValue ?? base.ToString();
        }

    }

    /// <summary>
    /// Defines different CREF types.
    /// </summary>
    public enum CRefType
    {
        /// <summary>
        /// Namespace (N).
        /// </summary>
        Namespace,

        /// <summary>
        /// Type (T). Class, interface, struct, enum or delegate.
        /// </summary>
        Type,

        /// <summary>
        /// Field (F). Includes also enumeration members, which are classified as fields.
        /// </summary>
        Field,

        /// <summary>
        /// A property (P). Includes indexers and other indexed properties.
        /// </summary>
        Property,

        /// <summary>
        /// Method (M). Includes constructors, operators etc.
        /// </summary>
        Method,

        /// <summary>
        /// Event (E).
        /// </summary>
        Event
    }
}
