using Blazorade.Bootstrap.Components;
using Blazorade.XmlDocumentation.Components.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazorade.XmlDocumentation.Components
{
    /// <summary>
    /// The base class for all components used to generate documentation from .NET XML documentation.
    /// </summary>
    public abstract class DocumentationComponentBase : BootstrapComponentBase
    {

        /// <summary>
        /// The key that will be used to get the current documentation parser from
        /// <see cref="Factory"/>.
        /// </summary>
        [Parameter]
        public string LibraryKey { get; set; }


        /// <summary>
        /// The documentation parser factory that will be used to get documentation parsers with.
        /// </summary>
        /// <remarks>
        /// An instance of the <see cref="DocumentationParserFactory"/> must be added as a service
        /// to the services collection at startup. It will then be injected to this property
        /// automatically whenever needed.
        /// </remarks>
        [Inject]
        protected DocumentationParserFactory Factory { get; set; }

        private DocumentationParser _Parser;
        /// <summary>
        /// Returns the documentation parser to use in the component.
        /// </summary>
        protected DocumentationParser Parser
        {
            get
            {
                if(null == _Parser)
                {
                    _Parser = this.Factory.GetParser(this.LibraryKey);
                }
                return _Parser;
            }
        }

        /// <summary>
        /// The URI builder that is used by components to build URIs to various sections within the documentation.
        /// </summary>
        [Inject]
        protected DocumentationUriBuilder UriBuilder { get; set; }


        /// <summary>
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            // If the LibraryKey parameter has changed, then we need to get a different parser.
            _Parser = null;
        }
    }
}
