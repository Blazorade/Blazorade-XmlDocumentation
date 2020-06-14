using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;

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
            this.AssemblyName = assemblyNameNode.InnerText;
            var asm = Assembly.Load(this.AssemblyName);
            if(null == asm)
            {
                throw new Exception($"The assembly with the name '{this.AssemblyName}' could not be loaded. The parser will most likely not be able to function properly.");
            }

            var membersNode = this.Document.SelectSingleNode("doc/members");
            if(null == membersNode)
            {
                throw new ArgumentException($"The provided XML document does not appear to be a valid XML documentation document.");
            }


            this.ParseAssemblyInformation();
        }

        /// <summary>
        /// Creates an instance of the class from the given XML string.
        /// </summary>
        /// <param name="xml">The XML string representing the XML documentation document to parse.</param>
        public DocumentationParser(string xml) : this(LoadDocument(xml)) { }

        /// <summary>
        /// Creates a new parser instance from the given assembly.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This constructor assumes that the assembly has the XML documentation file embedded as a resource in the assembly.
        /// </para>
        /// <para>
        /// If the assembly is stored in the file <c>My.Library.dll</c>, then the XML documentation file should be embedded
        /// as <c>My.Library.xml</c>.
        /// </para>
        /// </remarks>
        /// <param name="asm">The assembly to load the documentation from.</param>
        public DocumentationParser(Assembly asm) : this(LoadDocument(asm)) { }


        private static XmlDocument LoadDocument(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc;
        }

        private static XmlDocument LoadDocument(Assembly asm)
        {
            var name = $"{asm.ManifestModule.Name.Substring(0, asm.ManifestModule.Name.LastIndexOf('.'))}.xml";
            var resourceName = asm.GetManifestResourceNames().FirstOrDefault(x => x.EndsWith(name));
            if (resourceName?.Length > 0)
            {
                XmlDocument doc = new XmlDocument();
                using (var reader = new StreamReader(asm.GetManifestResourceStream(resourceName)))
                {
                    doc.LoadXml(reader.ReadToEnd());
                }

                return doc;
            }

            return null;
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
            string fullName = type.FullName;
            if (type.IsGenericType)
            {
                fullName = type.GetGenericTypeDefinition().FullName;
            }
            var node = this.Document.DocumentElement.SelectSingleNode($"members/member[@name = 'T:{fullName}']");
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

        /// <summary>
        /// Returns the events for the given type.
        /// </summary>
        /// <param name="type">The type to return the events for.</param>
        public IEnumerable<EventDocumentation> GetEvents(TypeDocumentation type)
        {
            var nodes = this.Document.DocumentElement.SelectNodes($"members/member[starts-with(@name, 'E:{type.Member.FullName}.')]");
            foreach(XmlNode node in nodes)
            {
                var nameAttribute = node.Attributes["name"].Value;
                var name = nameAttribute.Substring(nameAttribute.LastIndexOf('.') + 1);
                var eInfo = type.Member.GetEvent(name);
                if(null != eInfo)
                {
                    yield return new EventDocumentation(node, eInfo);
                }
            }
            yield break;
        }

        /// <summary>
        /// Returns the events for the given type.
        /// </summary>
        /// <param name="type">The type to return the events for.</param>
        public IEnumerable<EventDocumentation> GetEvents(Type type)
        {
            var doc = this.GetDocumentation(type);
            return this.GetEvents(doc);
        }


        /// <summary>
        /// Returns the fields for the given type.
        /// </summary>
        /// <param name="type">The type for which to return the fields.</param>
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
        /// Returns the members that match the given name.
        /// </summary>
        /// <remarks>
        /// In case <paramref name="memberName"/> represents a method, this method can return several items representing overloaded methods.
        /// Other member types return only one item.
        /// </remarks>
        /// <param name="memberName">The full name of the member to return.</param>
        /// <returns></returns>
        public IEnumerable<MemberDocumentation> GetMembers(string memberName)
        {
            string typeName, name;
            if(memberName.Contains(".."))
            {
                typeName = memberName.Substring(0, memberName.IndexOf(".."));
                name = memberName.Substring(memberName.LastIndexOf('.'));
            }
            else
            {
                typeName = memberName.Substring(0, memberName.LastIndexOf('.'));
                name = memberName.Substring(memberName.LastIndexOf('.') + 1);
            }

            var typeDoc = this.GetDocumentation(typeName);

            foreach(var m in from x in this.GetMembers(typeDoc) where x.Name == name select x)
            {
                yield return m;
            }
        }

        /// <summary>
        /// Returns the member documentation for the given type.
        /// </summary>
        /// <param name="type">The type to return the members for.</param>
        public IEnumerable<MemberDocumentation> GetMembers(TypeDocumentation type)
        {
            foreach(var m in this.GetFields(type))
            {
                yield return m;
            }
            foreach(var m in this.GetProperties(type))
            {
                yield return m;
            }
            foreach(var m in this.GetMethods(type))
            {
                yield return m;
            }
            foreach(var m in this.GetEvents(type))
            {
                yield return m;
            }
        }

        /// <summary>
        /// Returns the member documentation for the given type.
        /// </summary>
        /// <param name="type">The type to return the members for.</param>
        public IEnumerable<MemberDocumentation> GetMembers(Type type)
        {
            var doc = this.GetDocumentation(type);
            return this.GetMembers(doc);
        }

        /// <summary>
        /// Returns the methods for the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type for which to return the methods.</param>
        public IEnumerable<MethodDocumentation> GetMethods(TypeDocumentation type)
        {
            if (null == type) throw new ArgumentNullException(nameof(type));

            var nodes = this.Document.DocumentElement.SelectNodes($"members/member[starts-with(@name, 'M:{type.Member.FullName}.')]");
            foreach(XmlNode node in nodes)
            {
                MethodBase method = null;
                var cref = new CRef(node.Attributes["name"].Value);
                if (cref.IsMethod)
                {
                    method = cref.ToMethod();
                }

                if(null != method)
                {
                    yield return new MethodDocumentation(node, method);
                }
            }
            yield break;
        }

        /// <summary>
        /// Returns the method documentation for the methods of <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type for which to return the method documentations.</param>
        public IEnumerable<MethodDocumentation> GetMethods(Type type)
        {
            var doc = this.GetDocumentation(type);
            return this.GetMethods(doc);
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
            var fullName = type.Member.FullName;
            var nodes = this.Document.DocumentElement.SelectNodes($"members/member[starts-with(@name, 'P:{fullName}.')]");
            foreach(XmlNode node in nodes)
            {
                var nameAttribute = node.Attributes["name"].Value;
                
                var name = nameAttribute;
                if (name.Contains('(')) name = name.Substring(0, name.IndexOf('('));
                name = name.Substring(name.LastIndexOf('.') + 1);

                var prop = type.Member.GetProperties().Where(x => x.Name == name).Where(x => x.DeclaringType == type.Member).FirstOrDefault();

                if (null != prop)
                {
                    yield return new PropertyDocumentation(node, prop);
                }
            }
            yield break;
        }

        /// <summary>
        /// Returns the properties defined on the given type.
        /// </summary>
        /// <param name="type">The type for which to return the property documentations.</param>
        public IEnumerable<PropertyDocumentation> GetProperties(Type type)
        {
            var doc = this.GetDocumentation(type);
            return this.GetProperties(doc);
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
