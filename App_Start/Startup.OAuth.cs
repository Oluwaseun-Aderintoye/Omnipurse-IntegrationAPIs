using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace IntegrationAPIs.App_Start
{
    public class Startup
    {
        public void ConfigureOAuth(IAppBuilder app)
        {
            var issuer = ConfigurationManager.AppSettings["issuer"];
            var secret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["secret"]);

            //app.CreatePerOwinContext(() => new OmnipurseContext());
            //app.CreatePerOwinContext(() => new ApiUserManager());

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                AllowedAudiences = new[] { "Any" },
                IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                {
                    new SymmetricKeyIssuerSecurityTokenProvider(issuer, secret)
                }
            });

            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/oauth2/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                //Provider = new CustomOAuthProvider(),
                //AccessTokenFormat = new CustomJwtFormat(issuer)
            });

            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/oauth/general"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                //Provider = new CustomOAuthProvider(),
                //AccessTokenFormat = new CustomJwtFormat(issuer)
            });

            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/oauth/hyadmin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(120),
                //Provider = new CustomOAuthProvider(),
                //AccessTokenFormat = new CustomJwtFormat(issuer)
            });

            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/provider/authentication"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(2),
                //Provider = new CustomOAuthProvider(),
                //AccessTokenFormat = new CustomJwtFormat(issuer)
            });

        }
    }
}