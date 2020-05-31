using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Blazorade.XmlDocumentation
{
    /// <summary>
    /// Represents documentation for a method parameter.
    /// </summary>
    public class ParameterDocumentation : DocumentationNodeBase
    {
        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="documentation">The XML documentation node for the parameter.</param>
        /// <param name="parameterInfo">Information about the reflected parameter.</param>
        public ParameterDocumentation(XmlNode documentation, ParameterInfo parameterInfo) : base(documentation)
        {
            this.DocumentedParameter = parameterInfo ?? throw new ArgumentNullException(nameof(parameterInfo));
        }

        private ParameterInfo _DocumentedParameter;
        /// <summary>
        /// Returns the parameteter info for the parameter.
        /// </summary>
        public ParameterInfo DocumentedParameter
        {
            get { return _DocumentedParameter; }
            private set
            {
                _DocumentedParameter = value;
                this.Name = _DocumentedParameter?.Name;
            }
        }

        /// <summary>
        /// Returns the name of the parameter.
        /// </summary>
        public string Name { get; private set; }



        /// <summary>
        /// Returns the string representation of the parameter.
        /// </summary>
        public override string ToString()
        {
            return this.DocumentedParameter?.ToString() ?? base.ToString();
        }

    }
}
