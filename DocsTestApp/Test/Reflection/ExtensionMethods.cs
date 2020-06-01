using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml;

namespace DocsTestApp.Test.Reflection
{
    /// <summary>
    /// Extension methods, for your convenience.
    /// </summary>
    /// <remarks>
    /// These methods lack actual implementation, and have been added purely in order to have something to write <c>XML</c> documentation against.
    /// </remarks>
    public static class ExtensionMethods
    {

        /// <summary>
        /// Loads the embedded XML documentation file from the given assembly and returns it as an XML document.
        /// </summary>
        /// <param name="assembly">The assembly to load the XML documentation from.</param>
        /// <remarks>
        /// Using this method requires that the XML documentation file hsa been embedded into the assembly when it was complied.
        /// </remarks>
        public static XmlDocument LoadEmbeddedXmlDocs(this Assembly assembly)
        {
            return null;
        }
    }
}
