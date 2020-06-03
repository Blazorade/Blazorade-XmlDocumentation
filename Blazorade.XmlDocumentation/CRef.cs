using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime;
using System.Text;
using System.Linq;

namespace Blazorade.XmlDocumentation
{

    public class CRef
    {

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

        public static implicit operator string(CRef cref)
        {
            return cref.ToString();
        }


        public bool IsNamespace { get { return this.CRefType == CRefType.Namespace; } }

        public bool IsType { get { return this.CRefType == CRefType.Type; } }

        public bool IsField { get { return this.CRefType == CRefType.Field; } }

        public bool IsProperty { get { return this.CRefType == CRefType.Property; } }

        public bool IsMethod { get { return this.CRefType == CRefType.Method; } }

        public bool IsConstructor { get { return this.IsMethod && this.Value.Contains(".#ctor"); } }

        public bool IsEvent { get { return this.CRefType == CRefType.Event; } }

        public string RawValue { get; private set; }

        public CRefType CRefType { get; private set; }

        public string Value { get; private set; }



        public MethodBase ToMethod()
        {
            MethodBase m = null;
            List<Type> paramTypes = new List<Type>();
            List<string> paramTypeNames = new List<string>();
            int genericParamsCount = 0;

            var baseName = this.Value;
            if (baseName.Contains("("))
            {
                var paramsPart = baseName.Substring(baseName.IndexOf('(') + 1, baseName.IndexOf(')') - baseName.IndexOf('(') - 1);
                baseName = baseName.Substring(0, baseName.IndexOf('('));

                paramTypeNames = new List<string>(paramsPart.Split(','));

                foreach(var n in paramTypeNames)
                {
                    Type pt = null;
                    if(n.StartsWith("``"))
                    {
                        int position = int.Parse(n.Substring(2));
                        pt = Type.MakeGenericMethodParameter(position);
                    }
                    else
                    {
                        pt = AppDomain.CurrentDomain.GetAssemblies().GetType(n);
                    }

                    if(null != pt)
                    {
                        paramTypes.Add(pt);
                    }
                }
            }

            if(baseName.Contains("``"))
            {
                var countString = baseName.Substring(baseName.IndexOf("``") + 2);
                genericParamsCount = int.Parse(countString);
                baseName = baseName.Substring(0, baseName.IndexOf("``"));
            }

            var methodName = baseName.Substring(baseName.LastIndexOf('.') + 1).Replace('#', '.');

            var parentName = baseName.Substring(0, baseName.LastIndexOf('.'));
            var parentCref = new CRef($"T:{parentName}");
            var parentType = parentCref.ToType();

            if(methodName == ".ctor")
            {
                m = parentType.GetConstructor(paramTypes.ToArray());
            }
            else
            {
                m = parentType.GetMethod(methodName, genericParamsCount, paramTypes.ToArray());
            }
            
            return m;
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
            Type t = null;
            if(!this.IsType) throw new InvalidOperationException("The current object does not represent a Type.");

            t = Type.GetType(this.Value);
            if(null == t)
            {
                t = AppDomain.CurrentDomain.GetAssemblies().GetType(this.Value);
            }

            return t;
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
