﻿using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Jcvegan.Web.CustomPrincipal.Models;
using Microsoft.AspNet.Identity.Owin;

namespace Jcvegan.Web.CustomPrincipal {
    public class MvcApplication : System.Web.HttpApplication {
        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e) {
            HttpCookie authCookie = Request.Cookies["__CustomPrincipalSampleAuth"];
            if (authCookie != null) {
                var ticket = authCookie.Value;
                Extensions.Principal.CustomPrincipal customPrincipal = GetClaimsPrincipalFromCookie((ticket));
                HttpContext.Current.User = customPrincipal;
            }
        }

        private Extensions.Principal.CustomPrincipal GetClaimsPrincipalFromCookie(string ticket) {
            
            ticket = ticket.Replace('-', '+').Replace('_', '/');

            var padding = 3 - ((ticket.Length + 3) % 4);
            if (padding != 0)
                ticket = ticket + new string('=', padding);

            var bytes = Convert.FromBase64String(ticket);
            bytes = System.Web.Security.MachineKey.Unprotect(bytes,
                "Microsoft.Owin.Security.Cookies.CookieAuthenticationMiddleware",
                "ApplicationCookie", "v1");
            using (var memory = new MemoryStream(bytes))
            {
                using (var compression = new GZipStream(memory, CompressionMode.Decompress))
                {
                    using (var reader = new BinaryReader(compression))
                    {
                        reader.ReadInt32(); // Ignoring version here
                        string authenticationType = reader.ReadString();
                        reader.ReadString(); // Ignoring the default name claim type
                        reader.ReadString(); // Ignoring the default role claim type

                        int count = reader.ReadInt32(); // count of claims in the ticket

                        var claims = new Claim[count];
                        for (int index = 0; index != count; ++index)
                        {
                            string type = reader.ReadString();
                            type = type == "\0" ? ClaimTypes.Name : type;

                            string value = reader.ReadString();

                            string valueType = reader.ReadString();
                            valueType = valueType == "\0" ? "http://www.w3.org/2001/XMLSchema#string" : valueType;

                            string issuer = reader.ReadString();
                            issuer = issuer == "\0" ? "LOCAL AUTHORITY" : issuer;

                            string originalIssuer = reader.ReadString();
                            originalIssuer = originalIssuer == "\0" ? issuer : originalIssuer;

                            claims[index] = new Claim(type, value, valueType, issuer, originalIssuer);
                        }

                        var identity = new ClaimsIdentity(claims, authenticationType,
                            ClaimTypes.Name, ClaimTypes.Role);

                        var principal = new CustomPrincipal.Extensions.Principal.CustomPrincipal(identity,identity.Name);
                        System.Threading.Thread.CurrentPrincipal = principal;
                        //if (HttpContext.Current != null)
                        //    HttpContext.Current.User = principal;
                        return principal;
                    }
                }
            }
        }
    }
}