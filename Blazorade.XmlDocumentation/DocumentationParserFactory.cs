using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Blazorade.XmlDocumentation
{
    /// <summary>
    /// Manages multiple <see cref="DocumentationParser"/> instances.
    /// </summary>
    public class DocumentationParserFactory
    {
        /// <summary>
        /// Creates an instance of the class.
        /// </summary>
        public DocumentationParserFactory() { }



        private Dictionary<string, DocumentationParser> Parsers = new Dictionary<string, DocumentationParser>();

        /// <summary>
        /// Adds a parser to the factory with the given <paramref name="key"/> using the given <paramref name="xml"/> document.
        /// </summary>
        /// <param name="key">The key to add the parser with. If a parser with the given key exists, it will be overwritten.</param>
        /// <param name="xml">The XML document to use to create a <see cref="DocumentationParser"/> object to add.</param>
        /// <returns>Returns the added parser.</returns>
        public DocumentationParser AddParser(string key, XmlDocument xml)
        {
            var parser = new DocumentationParser(xml);
            this.AddParser(key, parser);
            return parser;
        }

        /// <summary>
        /// Adds a parser to the factory with the given <paramref name="key"/> using the given <paramref name="xml"/> string.
        /// </summary>
        /// <param name="key">The key to add the parser with. If a parser with the given key exists, it will be overwritten.</param>
        /// <param name="xml">The XML string to use to create a <see cref="DocumentationParser"/> object to add.</param>
        /// <returns>Returns the added parser.</returns>
        public DocumentationParser AddParser(string key, string xml)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xml);
            return this.AddParser(key, xDoc);
        }

        /// <summary>
        /// Adds a parser representing the given assembly to the factory.
        /// </summary>
        /// <remarks>
        /// This method tries to find an embedded resource from the assembly with the name of the assembly file name with an <c>.xml</c>
        /// extension, and assumes that if such a resource is found, that resource contains the XML documentation for the assembly.
        /// </remarks>
        /// <param name="key">The key to add the parser with. If a parser with the given key exists, it will be overwritten.</param>
        /// <param name="asm">The assembly to search for embedded XML documents in.</param>
        /// <returns>Returns the added parser.</returns>
        public DocumentationParser AddParser(string key, Assembly asm)
        {
            var parser = new DocumentationParser(asm);
            this.AddParser(key, parser);
            return parser;
        }

        /// <summary>
        /// Adds a parser to the factory.
        /// </summary>
        /// <param name="key">The key of the parser. If a parser with the given key exists, it will be overwritten.</param>
        /// <param name="parser">The parser to add.</param>
        /// <returns>Returns the added parser.</returns>
        public DocumentationParser AddParser(string key, DocumentationParser parser)
        {
            this.Parsers[key] = parser ?? throw new ArgumentNullException(nameof(parser));
            return parser;
        }

        /// <summary>
        /// Returns a collection of keys representing parsers loaded into the current factory.
        /// </summary>
        public IEnumerable<string> GetParserKeys()
        {
            return this.Parsers.Keys;
        }

        /// <summary>
        /// Returns the parser with the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key of the parser to return.</param>
        public DocumentationParser GetParser(string key)
        {
            if (this.Parsers.ContainsKey(key))
            {
                return this.Parsers[key];
            }

            return null;
        }

        /// <summary>
        /// Returns the parser with the given key.
        /// </summary>
        /// <remarks>
        /// If a parser with the given key does not exist, one will be created using the given <paramref name="xml"/> string
        /// and added to the factory for further use.
        /// </remarks>
        /// <param name="key">The key representing the parser to return.</param>
        /// <param name="xml">Optional. The XML string to create a parser from in case a parser does not exist with the specified <paramref name="key"/>.</param>
        public DocumentationParser GetParser(string key, string xml = null)
        {
            if(!this.Parsers.ContainsKey(key))
            {
                this.AddParser(key, xml);
            }

            return this.Parsers[key];
        }

        /// <summary>
        /// Returns the parser with the given key.
        /// </summary>
        /// <remarks>
        /// If a parser with the given key does not exist, one will be created using the given <paramref name="xml"/> document
        /// and added to the factory for further use.
        /// </remarks>
        /// <param name="key">The key representing the parser to return.</param>
        /// <param name="xml">Optional. The XML document to create a parser from in case a parser does not exist with the specified <paramref name="key"/>.</param>
        public DocumentationParser GetParser(string key, XmlDocument xml = null)
        {
            if(!this.Parsers.ContainsKey(key))
            {
                this.AddParser(key, xml);
            }

            return this.Parsers[key];
        }

        /// <summary>
        /// Finds the parser that can resolve the type specified in <paramref name="typeName"/>.
        /// </summary>
        /// <param name="typeName">The full name of the type to find a parser for.</param>
        /// <returns>
        /// The method returns a <see cref="Tuple{T1, T2, T3}"/> where <c>Item1</c> contains
        /// the key for the parser, <c>Item2</c> contains the actual parser, and <c>Item3</c> contains the type
        /// that was resolved by the parser.
        /// </returns>
        public Tuple<string, DocumentationParser, Type> GetParserByType(string typeName)
        {
            foreach(var key in this.Parsers.Keys)
            {
                var p = this.GetParser(key);
                var t = p.GetType(typeName);
                if(null != t)
                {
                    return new Tuple<string, DocumentationParser, Type>(key, p, t);
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the parser that can resolve the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to use when searching for a parser.</param>
        /// <returns>
        /// The method returns a <see cref="Tuple{T1, T2}"/> where <c>Item1</c> contains the key for the parser
        /// and <c>Item2</c> contains the actual parser.
        /// </returns>
        public Tuple<string, DocumentationParser> GetParserByType(Type type)
        {
            var result = this.GetParserByType(type.FullName);
            if(null != result)
            {
                return new Tuple<string, DocumentationParser>(result.Item1, result.Item2);
            }

            return null;
        }

        /// <summary>
        /// Returns the <see cref="Type"/> that represents the given <paramref name="typeName"/>. 
        /// </summary>
        /// <remarks>
        /// If one is not directly resolved from that name, then each of the assemblies loaded into the current 
        /// <see cref="AppDomain"/> are enumerated and used to complete the given <paramref name="typeName"/>.
        /// </remarks>
        /// <param name="typeName">The full name of the type to return.</param>
        public Type GetType(string typeName)
        {
            Type t = null;
            t = Type.GetType(typeName);
            if(null == t)
            {
                foreach(var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    var fullName = $"{typeName}, {asm.GetName().Name}";
                    t = Type.GetType(fullName);
                    if(null != t)
                    {
                        break;
                    }
                }
            }

            return t;
        }
    }
}
