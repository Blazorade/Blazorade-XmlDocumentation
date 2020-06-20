using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Blazorade.XmlDocumentation
{
    /// <summary>
    /// Extension methods for working with XML documentation files.
    /// </summary>
    public static class DocumentationExtensions
    {

        private static readonly IDictionary<Type, string> TypeAliases = new Dictionary<Type, string>
        {
            { typeof(byte), "byte" },
            { typeof(sbyte), "sbyte" },
            { typeof(short), "short" },
            { typeof(ushort), "ushort" },
            { typeof(int), "int" },
            { typeof(uint), "uint" },
            { typeof(long), "long" },
            { typeof(ulong), "ulong" },
            { typeof(float), "float" },
            { typeof(double), "double" },
            { typeof(decimal), "decimal" },
            { typeof(object), "object" },
            { typeof(bool), "bool" },
            { typeof(char), "char" },
            { typeof(string), "string" },
            { typeof(void), "void" },
            { typeof(byte?), "byte?" },
            { typeof(sbyte?), "sbyte?" },
            { typeof(short?), "short?" },
            { typeof(ushort?), "ushort?" },
            { typeof(int?), "int?" },
            { typeof(uint?), "uint?" },
            { typeof(long?), "long?" },
            { typeof(ulong?), "ulong?" },
            { typeof(float?), "float?" },
            { typeof(double?), "double?" },
            { typeof(decimal?), "decimal?" },
            { typeof(bool?), "bool?" },
            { typeof(char?), "char?" }
        };

        /// <summary>
        /// Returns only the types where <see cref="Type.IsGenericParameter"/> returns <c>true</c>.
        /// </summary>
        public static IEnumerable<Type> GenericParameters(this IEnumerable<Type> types)
        {
            return from x in types where x.IsGenericParameter select x;
        }

        /// <summary>
        /// Returns the description for the assembly.
        /// </summary>
        /// <remarks>
        /// The description is read from the <see cref="AssemblyDescriptionAttribute"/> attribute of the assembly.
        /// </remarks>
        /// <param name="asm"></param>
        public static string GetDescription(this Assembly asm)
        {
            var attr = asm?.GetCustomAttribute<AssemblyDescriptionAttribute>();
            return attr?.Description;
        }

        /// <summary>
        /// Returns the informational version for the assembly.
        /// </summary>
        /// <remarks>
        /// The version info is read from the <see cref="AssemblyInformationalVersionAttribute"/> attribute of the assembly.
        /// </remarks>
        /// <param name="asm"></param>
        /// <returns></returns>
        public static string GetInformationalVersion(this Assembly asm)
        {
            var attr = asm?.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            return attr?.InformationalVersion;
        }

        /// <summary>
        /// Returns the product name for the assembly.
        /// </summary>
        /// <remarks>
        /// The product name is read from the <see cref="AssemblyProductAttribute"/> attribute of the assembly.
        /// </remarks>
        public static string GetProductName(this Assembly asm)
        {
            var attr = asm?.GetCustomAttribute<AssemblyProductAttribute>();
            return attr?.Product;
        }

        /// <summary>
        /// Searches the current app domain for a type that matches <paramref name="typeName"/> and returns it.
        /// </summary>
        /// <param name="domain">The current app domain.</param>
        /// <param name="typeName">The type to attempt to resolve into a <see cref="Type"/> objcet.</param>
        /// <returns>Returns the resolved <see cref="Type"/> or <c>null</c>if no type was found.</returns>
        public static Type GetType(this AppDomain domain, string typeName)
        {
            return domain.GetAssemblies().GetType(typeName);
        }

        /// <summary>
        /// Enumerates all of the given <paramref name="assemblies"/> until a type if found with the given <paramref name="typeName"/>.
        /// </summary>
        /// <param name="assemblies">The collection of assemblies to look in when searching for the given tyhpe.</param>
        /// <param name="typeName">The full name of the type to look for.</param>
        /// <returns>
        /// Returns the <see cref="Type"/> found, or <c>null</c> if nothing was found.
        /// </returns>
        public static Type GetType(this IEnumerable<Assembly> assemblies, string typeName)
        {
            Type t = null;

            foreach(var asm in assemblies)
            {
                t = asm.GetType(typeName);
                if (null != t) break;
            }

            return t;
        }

        /// <summary>
        /// Assumes that <paramref name="input"/> is a string containing a comma-separated
        /// list of type definitions that can also be generic type definitions.
        /// </summary>
        public static IEnumerable<string> SplitTypeDefinitions(this string input)
        {
            if(!string.IsNullOrEmpty(input))
            {
                var rx = new Regex("[^,^{^}]+\\{[^}]+\\}|[^,]+");
                foreach(Match m in rx.Matches(input))
                {
                    yield return m.Value;
                }
            }

            yield break;
        }

        /// <summary>
        /// Returns the display name for the given <paramref name="method"/>.
        /// </summary>
        /// <remarks>
        /// A method display name is unique within its declaring type.
        /// </remarks>
        /// <param name="method">The method for which to return the display name.</param>
        public static string ToDisplayName(this MethodBase method)
        {
            var sb = new StringBuilder();

            if (method is MethodInfo)
            {
                var rt = ((MethodInfo)method).ReturnType;
                if(null != rt)
                {
                    sb.Append(rt.ToDisplayName());
                    sb.Append(" ");
                }
            }

            sb.Append(method.Name);

            var gArgs = method.IsGenericMethod ? method.GetGenericArguments() : new Type[0];
            if(gArgs?.Length > 0)
            {
                sb.Append("<");
                sb.Append(string.Join(", ", from x in gArgs select x.Name));
                sb.Append(">");
            }

            sb.Append("(");

            var parameters = method.GetParameters();
            var pArr = from x in parameters select x.ToDisplayName();

            sb.Append(string.Join(", ", pArr.ToArray()));

            sb.Append(")");

            return sb.ToString();
        }

        /// <summary>
        /// Returns the display name for the parameter.
        /// </summary>
        /// <param name="parameter">The parameter to return the display name for.</param>
        /// <returns></returns>
        public static string ToDisplayName(this ParameterInfo parameter)
        {
            string extensionPrefix = "";

            if(parameter.Position == 0 && parameter.Member.IsDefined(typeof(ExtensionAttribute)))
            {
                extensionPrefix = "this ";
            }

            return $"{extensionPrefix}{parameter.ParameterType.ToDisplayName()} {parameter.Name}";
        }

        /// <summary>
        /// Returns the display name for the given <paramref name="property"/>.
        /// </summary>
        /// <param name="property">The property whose display name to return.</param>
        public static string ToDisplayName(this PropertyInfo property)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(property.PropertyType.ToDisplayName());
            builder.Append(" ");
            builder.Append(property.Name);

            var indexParams = property.GetIndexParameters();
            if(indexParams.Length > 0)
            {
                builder.Append("[");
                builder.Append(string.Join(", ", from x in indexParams select x.ToDisplayName()));
                builder.Append("]");
            }

            return builder.ToString();
        }

        /// <summary>
        /// Returns the display name for the given type.
        /// </summary>
        /// <remarks>
        /// This method also supports breaking up generic types including the display names of the generic type arguments too.
        /// </remarks>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string ToDisplayName(this Type type)
        {
            string displayName = null;
            if(TypeAliases.ContainsKey(type))
            {
                displayName = TypeAliases[type];
            }
            else if (type.IsGenericType)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(type.Name.Substring(0, type.Name.LastIndexOf('`')));
                builder.Append("<");

                if(type.GenericTypeArguments?.Length > 0)
                {
                    builder.Append(string.Join(", ", from x in type.GenericTypeArguments select x.ToDisplayName()));
                }
                else
                {
                    var gDef = type.GetGenericTypeDefinition();
                    var args = gDef.GetGenericArguments();

                    builder.Append(string.Join(", ", from x in args select x.ToDisplayName()));
                }

                builder.Append(">");
                displayName = builder.ToString();
            }
            else
            {
                displayName = type.Name;
            }

            return displayName;
        }

        /// <summary>
        /// Returns the display name for <paramref name="field"/>.
        /// </summary>
        /// <param name="field">The field to return the display name for.</param>
        public static string ToDisplayName(this FieldInfo field)
        {
            return $"{field?.FieldType?.ToDisplayName()} {field?.Name}";
        }

        /// <summary>
        /// Returns the display name for <paramref name="member"/>.
        /// </summary>
        /// <param name="member">The member whose display name to reutrn.</param>
        public static string ToDisplayName(this MemberInfo member)
        {
            if(member is MethodBase)
            {
                return ((MethodBase)member).ToDisplayName();
            }
            else if(member is FieldInfo)
            {
                return ((FieldInfo)member).ToDisplayName();
            }
            else if(member is PropertyInfo)
            {
                return ((PropertyInfo)member).ToDisplayName();
            }
            else if(member is EventInfo)
            {
                return ((EventInfo)member).ToDisplayName();
            }


            var name = member?.Name?.Replace('#', '.');

            if (name.Contains('('))
            {
                name = name.Substring(0, name.IndexOf('('));
            }

            if (name.Contains('`') && name.LastIndexOf('.') < name.LastIndexOf('`'))
            {
                name = name.Substring(0, name.IndexOf('`'));
            }

            var prefixes = new string[] { "get_", "set_", "add_", "remove_" };
            var prefix = from x in prefixes where name.StartsWith(x) select x;
            if (prefix.Count() > 0) name = name.Substring(prefix.First().Length);

            return name;
        }

        /// <summary>
        /// Returns the display name for <paramref name="member"/>.
        /// </summary>
        /// <param name="member">The event member to return the name for.</param>
        /// <returns></returns>
        public static string ToDisplayName(this EventInfo member)
        {
            return $"{member.EventHandlerType.ToDisplayName()} {member.Name}";
        }

        /// <summary>
        /// Returns the method that <paramref name="methodString"/> represents.
        /// </summary>
        public static MethodBase ToMethod(this string methodString)
        {
            if (string.IsNullOrEmpty(methodString)) throw new ArgumentNullException(nameof(methodString));
            if (!(methodString.IndexOf('.') > 0)) throw new ArgumentException("The given string does not appear to be a full method definition.");

            MethodBase method = null;
            int genericParamCount = 0;
            List<Type> paramTypes = new List<Type>();

            if (methodString.Contains('('))
            {
                var paramsString = methodString.Substring(methodString.IndexOf('(') + 1, methodString.IndexOf(')') - methodString.IndexOf('(') - 1);
                var arr = paramsString.SplitTypeDefinitions();
                foreach(var s in arr)
                {
                    var pt = s.ToType();
                    if(null != pt)
                    {
                        if (pt.IsGenericParameter) genericParamCount++;
                        paramTypes.Add(pt);
                    }
                }
                methodString = methodString.Substring(0, methodString.IndexOf('('));
            }

            var methodName = methodString.Substring(methodString.LastIndexOf('.') + 1).Replace('#', '.');
            if(methodName.Contains("``"))
            {
                genericParamCount = int.Parse(methodName.Substring(methodName.LastIndexOf('`') + 1));
                methodName = methodName.Substring(0, methodName.IndexOf("``"));
            }

            var declaringTypeString = methodString.Substring(0, methodString.LastIndexOf('.'));
            var declaringType = declaringTypeString.ToType();

            if(declaringType.IsGenericType)
            {
                var genArgs = declaringType.GetGenericArguments();
                for(int i = 0; i < paramTypes.Count; i++)
                {
                    var p = paramTypes[i];
                    if(p.IsGenericParameter)
                    {
                        p = genArgs[p.GenericParameterPosition];
                        paramTypes[i] = p;
                    }
                }
            }

            if(methodName == ".ctor")
            {
                var constructors = declaringType.GetConstructors();
                foreach(var c in constructors)
                {
                    var pArr = c.GetParameters();
                }
                method = declaringType.GetConstructor(paramTypes.ToArray());
            }
            else
            {
                method = declaringType.GetMethod(methodName, genericParamCount, paramTypes.ToArray());
            }

            return method;
        }

        /// <summary>
        /// Returns the <see cref="Type"/> that the current string represents.
        /// </summary>
        public static Type ToType(this string typeString)
        {
            Type pt = null;
            if (typeString.StartsWith("`"))
            {
                var pString = typeString.Substring(typeString.LastIndexOf('`') + 1);
                int position = int.Parse(pString);
                pt = Type.MakeGenericMethodParameter(position);
            }
            else if (typeString.Contains('{'))
            {
                StringBuilder builder = new StringBuilder();

                var paramsString = typeString.Substring(typeString.IndexOf('{') + 1, typeString.IndexOf('}') - typeString.IndexOf('{') - 1);
                typeString = typeString.Substring(0, typeString.IndexOf('{'));

                var arr = paramsString.SplitTypeDefinitions();
                builder
                    .Append(typeString)
                    .Append("`")
                    .Append(arr.Count());

                var typeList = new List<Type>();
                foreach (var s in arr)
                {
                    var t = s.ToType();
                    if (null != t) typeList.Add(t);
                }

                var firstParam = typeList.FirstOrDefault();
                if(null != firstParam && !firstParam.IsGenericParameter)
                {
                    var isFirst = true;
                    builder.Append("[");
                    foreach(var p in typeList)
                    {
                        if (!isFirst) builder.Append(",");
                        builder.Append("[").Append(p.FullName).Append("]");
                        isFirst = false;
                    }

                    builder.Append("]");
                }
                
                pt = AppDomain.CurrentDomain.GetType(builder.ToString());
            }
            else
            {
                pt = AppDomain.CurrentDomain.GetType(typeString);
            }

            return pt;
        }


        private static readonly IEnumerable<string> MemberNamePrefixes = new string[]
        {
            "add_",
            "remove_",
            "get_",
            "set_"
        };

        /// <summary>
        /// Returns the full name of the member.
        /// </summary>
        /// <remarks>
        /// The full name includes the full name of the declaring type with the actual name of the member without
        /// any parameters or generic arguments.
        /// </remarks>
        /// <param name="member">The member whose full name to return.</param>
        public static string ToFullName(this MemberInfo member)
        {
            var name = member.Name;
            var prefix = MemberNamePrefixes.FirstOrDefault(x => name.StartsWith(x));
            if(null != prefix)
            {
                name = name.Substring(prefix.Length);
            }

            var fullName = $"{member.DeclaringType.FullName}.{name}";

            return fullName;
        }

    }
}
