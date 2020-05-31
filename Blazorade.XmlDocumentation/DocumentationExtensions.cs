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
                sb.Append(rt.ToDisplayName());
                sb.Append(" ");
            }

            sb.Append(method.Name);
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

        public static string ToDisplayName(this PropertyInfo property)
        {
            var sb = new StringBuilder();
            sb.Append(property.PropertyType.ToDisplayName());
            sb.Append(" ");
            sb.Append(property.Name);

            return sb.ToString();
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

    }
}
