using Blazorade.XmlDocumentation;
using Blazorade.XmlDocumentation.Components;
using Blazorade.XmlDocumentation.Components.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddBlazoradeXmlDocumentation(this IServiceCollection services)
        {
            return services.AddBlazoradeXmlDocumentation((options) =>
            {
                
            });
        }

        public static IServiceCollection AddBlazoradeXmlDocumentation(this IServiceCollection services, Action<XmlDocumentationOptions> configureOptions)
        {
            var options = new XmlDocumentationOptions();
            configureOptions?.Invoke(options);

            var factory = new DocumentationParserFactory();
            foreach (var key in options?.Parsers?.Keys ?? new string[0])
            {
                factory.AddParser(key, options.Parsers[key]);
            }
            services.AddSingleton(factory);

            services.AddSingleton<DocumentationUriBuilder>(options.UriBuilder ?? new DocumentationUriBuilder());

            return services;
        }
    }
}
