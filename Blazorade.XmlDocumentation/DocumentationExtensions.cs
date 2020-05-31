using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public static string GetDisplayName(this MethodBase method)
        {
            return null;
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
