using IntegrationAPIs;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace IntegrationAPIs
{
    using Newtonsoft.Json.Serialization;
    using Owin;
    using System.Linq;
    using System.Net.Http.Formatting;
    using System.Web.Http;
    using System.Web.Http.Cors;

    //userid.identity.name

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // OAuth setup if any
            //ConfigureOAuth(app);

            // Web API config
            var httpConfig = new HttpConfiguration();

            // CORS setup - allow Angular
            var cors = new EnableCorsAttribute("http://localhost:4200", "*", "*");
            httpConfig.EnableCors(cors);  // From Microsoft.AspNet.WebApi.Cors

            // Register routes, formatters, etc.
            ConfigureWebApi(httpConfig);

            // Enable CORS via OWIN
            //app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);  // optional if already handled above

            // Hook Web API into OWIN pipeline
            //app.UseWebApi(httpConfig);
        }

        private void ConfigureWebApi(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

    }
}