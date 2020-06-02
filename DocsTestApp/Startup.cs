using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorade.Bootstrap.Components;
using Blazorade.XmlDocumentation;
using Blazorade.XmlDocumentation.Components;
using Blazorade.XmlDocumentation.Components.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DocsTestApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            var f = new DocumentationParserFactory();
            f.AddParser(LibKeys.LocalLib, this.GetType().Assembly);
            f.AddParser(LibKeys.TestLib, typeof(TestLibrary.Class1).Assembly);
            f.AddParser(LibKeys.Bootstrap, typeof(Blazorade.Bootstrap.Components._Imports).Assembly);
            f.AddParser(LibKeys.Core, typeof(Blazorade.Core._Imports).Assembly);
            f.AddParser(LibKeys.XmlDocs, typeof(DocumentationParser).Assembly);
            f.AddParser(LibKeys.XmlDocsComponents, typeof(Blazorade.XmlDocumentation.Components._Imports).Assembly);
            services.AddSingleton(f);

            services.AddSingleton<DocumentationUriBuilder>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
