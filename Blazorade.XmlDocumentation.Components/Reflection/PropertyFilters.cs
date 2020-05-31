using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace Blazorade.XmlDocumentation.Components.Reflection
{
    /// <summary>
    /// Filters used for filtering properties.
    /// </summary>
    public static class PropertyFilters
    {
        /// <summary>
        /// A filter that includes only parameter properties.
        /// </summary>
        /// <remarks>
        /// A parameter property is a property that is decorated with the <see cref="ParameterAttribute"/> attribute.
        /// </remarks>
        public static readonly Func<PropertyDocumentation, bool> ParameterFilter = (pd) =>
        {
            var paramAttribute = pd.DocumentedMember.GetCustomAttributes(false)
                .Where(x => x is Attribute)
                .Select((o) => (Attribute)o)
                .FirstOrDefault(x => x.GetType() == typeof(ParameterAttribute));

            return null != paramAttribute;
        };

        /// <summary>
        /// A filter that includes properties that are not parameter properties.
        /// </summary>
        public static readonly Func<PropertyDocumentation, bool> NonParametersFilter = (pd) =>
        {
            return !ParameterFilter(pd);
        };

    }
}
