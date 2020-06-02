using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using System.Linq;
using System.Reflection;

namespace Blazorade.XmlDocumentation
{
    /// <summary>
    /// A utility class that helps you parse XML documentation files.
    /// </summary>
    public class DocumentationParser
    {

        #region Constructors

        /// <summary>
        /// Creates an instance of the class from the given XML document object.
        /// </summary>
        /// <param name="document">The XML document containing the documentation to parse.</param>
        public DocumentationParser(XmlDocument document)
        {
            this.Document = document ?? throw new ArgumentNullException(nameof(document));

            var assemblyNameNode = this.Document.SelectSingleNode("doc/assembly/name");
            if(null == assemblyNameNode)
            {
                throw new ArgumentException($"Cannot find assembly name in the provided XML document.");
            }

            var membersNode = this.Document.SelectSingleNode("doc/members");
            if(null == membersNode)
            {
                throw new ArgumentException($"The provided XML document does not appear to be a valid XML documentation document.");
            }

            this.AssemblyName = assemblyNameNode.InnerText;
            this.ParseAssemblyInformation();
        }

        /// <summary>
        /// Creates an instance of the class from the given string.
        /// </summary>
        /// <param name="xml">The XML string representing the XML documentation document to parse.</param>
        public DocumentationParser(string xml) : this(LoadDocument(xml)) { }

        private static XmlDocument LoadDocument(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc;
        }

        #endregion


        /// <summary>
        /// Returns the name of the assembly whose XML documentation is loaded into the parser.
        /// </summary>
        public string AssemblyName { get; private set; }

        /// <summary>
        /// The informational version of the assembly.
        /// </summary>
        public string AssemblyVersion { get; private set; }

        /// <summary>
        /// The product name defined in the assembly.
        /// </summary>
        public string AssemblyProductName { get; set; }

        /// <summary>
        /// The description of the assembly.
        /// </summary>
        public string AssemblyDescription { get; set; }



        /// <summary>
        /// The XML document object containing the XML documentation.
        /// </summary>
        public XmlDocument Document { get; private set; }



        /// <summary>
        /// Returns the documentation for the given type.
        /// </summary>
        /// <param name="type">The type to return the documentation for.</param>
        /// <returns>
        /// Returns an instance of the <see cref="TypeDocumentation"/> class representing the documentation. If
        /// documentation representing the given type is not found, <c>null</c> is returned.
        /// </returns>
        public TypeDocumentation GetDocumentation(Type type)
        {
            var node = this.Document.DocumentElement.SelectSingleNode($"members/member[@name = 'T:{type.FullName}']");
            if(null != node)
            {
                return new TypeDocumentation(node, type);
            }
            return null;
        }

        /// <summary>
        /// Returns the documentation for the the type specified in <paramref name="typeName"/>.
        /// </summary>
        /// <param name="typeName">The full name of the type whose documentation to return.</param>
        public TypeDocumentation GetDocumentation(string typeName)
        {
            return this.GetDocumentation(this.GetType(typeName));
        }

        public IEnumerable<FieldDocumentation> GetFields(TypeDocumentation type)
        {
            var nodes = this.Document.DocumentElement.SelectNodes($"members/member[starts-with(@name, 'F:{type.Member.FullName}.')]");
            foreach(XmlNode node in nodes)
            {
                var nameAttribute = node.Attributes["name"].Value;
                var name = nameAttribute.Substring(nameAttribute.LastIndexOf('.') + 1);
                var fld = type.Member.GetField(name);
                if(null != fld)
                {
                    yield return new FieldDocumentation(node, fld);
                }
            }

            yield break;
        }

        /// <summary>
        /// Returns the methods for the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type for which to return the methods.</param>
        public IEnumerable<MethodDocumentation> GetMethods(TypeDocumentation type)
        {
            var nodes = this.Document.DocumentElement.SelectNodes($"members/member[starts-with(@name, 'M:{type.Member.FullName}.')]");
            foreach(XmlNode node in nodes)
            {
                List<Type> paramTypes = new List<Type>();
                var nameAttribute = node.Attributes["name"].Value;
                if (nameAttribute.Contains("("))
                {
                    var factory = new DocumentationParserFactory();
                    var parameters = nameAttribute.Substring(nameAttribute.IndexOf('('));
                    nameAttribute = nameAttribute.Substring(0, nameAttribute.Length - parameters.Length);

                    parameters = parameters.Substring(1, parameters.Length - 2);
                    var pArr = parameters.Split(',');
                    foreach(var itm in pArr)
                    {
                        var pType = factory.GetType(itm.Trim());
                        if(null != pType)
                        {
                            paramTypes.Add(pType);
                        }
                    }
                }

                MethodBase method = null;
                var name = nameAttribute.Substring(nameAttribute.LastIndexOf('.') + 1);
                if (name == "#ctor")
                {
                    method = type.Member.GetConstructor(paramTypes.ToArray());
                }
                else
                {
                    method = type.Member.GetMethod(name, paramTypes.ToArray());
                }

                if(null != method)
                {
                    yield return new MethodDocumentation(node, method);
                }
            }
            yield break;
        }

        /// <summary>
        /// Returns a list of namespaces representing the types documented in the loaded XML documentation.
        /// </summary>
        public IList<string> GetNamespaces()
        {
            var list = new List<string>();
            var typeNodes = this.Document.DocumentElement.SelectNodes("members/member[starts-with(@name, 'T:')]");
            foreach(XmlNode node in typeNodes)
            {
                var name = node.Attributes["name"]?.Value?.Substring(2);
                var ns = name.Substring(0, name.LastIndexOf('.'));
                if(!list.Contains(ns))
                {
                    list.Add(ns);
                }
            }

            return list.OrderBy(x => x).ToList();
        }

        /// <summary>
        /// Returns the properties defined on the given type.
        /// </summary>
        /// <param name="type">The type for which to return the properties.</param>
        /// <returns></returns>
        public IEnumerable<PropertyDocumentation> GetProperties(TypeDocumentation type)
        {
            var nodes = this.Document.DocumentElement.SelectNodes($"members/member[starts-with(@name, 'P:{type.Member.FullName}.')]");
            foreach(XmlNode node in nodes)
            {
                var nameAttribute = node.Attributes["name"].Value;
                var name = nameAttribute.Substring(nameAttribute.LastIndexOf('.') + 1);
                var prop = type.Member.GetProperty(name);
                if(null != prop)
                {
                    yield return new PropertyDocumentation(node, prop);
                }
            }
            yield break;
        }

        /// <summary>
        /// Returns the type represented by <paramref name="typeName"/>. The assembly that the current
        /// parser instance represent is used when searching for the type.
        /// </summary>
        /// <param name="typeName">The full type name of the type to return.</param>
        public Type GetType(string typeName)
        {
            return Type.GetType($"{typeName}, {this.AssemblyName}");
        }

        /// <summary>
        /// Returns the types documented in the given namespace.
        /// </summary>
        /// <param name="ns">The namespace whose types to return.</param>
        public IEnumerable<TypeDocumentation> GetTypes(string ns)
        {
            var nodes = this.Document.DocumentElement.SelectNodes($"members/member[starts-with(@name, 'T:{ns}.')]");

            foreach(XmlNode node in nodes)
            {
                var typeName = $"{node.Attributes["name"].Value.Substring(2)}, {this.AssemblyName}";
                var t = Type.GetType(typeName);
                if(t?.Namespace == ns)
                {
                    yield return new TypeDocumentation(node, t);
                }
            }
        }



        private void ParseAssemblyInformation()
        {
            var asmList = AppDomain.CurrentDomain.GetAssemblies();

            var asm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == this.AssemblyName);
            this.AssemblyDescription = asm.GetDescription();
            this.AssemblyProductName = asm.GetProductName();
            this.AssemblyVersion = asm.GetInformationalVersion();
        }
    }
}
