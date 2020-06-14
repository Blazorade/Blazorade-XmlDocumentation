using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Blazorade.XmlDocumentation
{
    /// <summary>
    /// Extension methods for working with XML documentation files.
    /// </summary>
    public static class DocumentationExtensions
    {

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
                if(rt != typeof(void))
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
            return $"{property?.PropertyType?.ToDisplayName()} {property?.Name}";
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
            if (type.IsGenericType)
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
        /// Returns the name of the member that is used in URIs.
        /// </summary>
        /// <remarks>
        /// The URI name of a member includeds the full name of the declaring type with the actual name of the member without
        /// any parameters or generic arguments.
        /// </remarks>
        /// <param name="member">The member whose URI name to return.</param>
        public static string ToUriName(this MemberInfo member)
        {
            var name = $"{member.DeclaringType.FullName}.{member.ToDisplayName()}";

            return name;
        }

    }
}
