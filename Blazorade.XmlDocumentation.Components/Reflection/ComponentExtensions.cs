using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Blazorade.XmlDocumentation.Components.Reflection
{
    /// <summary>
    /// Extension methods for components.
    /// </summary>
    public static class ComponentExtensions
    {
        /// <summary>
        /// Returns <c>true</c> if the given <see cref="TypeDocumentation"/> represents a component class.
        /// </summary>
        /// <param name="type">The type documentation to examine.</param>
        public static bool IsComponent(this Type type)
        {
            var itf = type.GetInterfaces().FirstOrDefault(x => x == typeof(IComponent));
            return null != itf && !type.IsAbstract && type.IsPublic;
        }

        /// <summary>
        /// Returns <c>true</c> if the given <see cref="PropertyDocumentation"/> represents a parameter property.
        /// </summary>
        /// <param name="property">The property documentation to examine.</param>
        public static bool IsParameter(this PropertyInfo property)
        {
            var paramAttribute = property.GetCustomAttributes(false)
                .Where(x => x is Attribute)
                .Select((o) => (Attribute)o)
                .FirstOrDefault(x => x.GetType() == typeof(ParameterAttribute));

            return null != paramAttribute;
        }

    }
}
