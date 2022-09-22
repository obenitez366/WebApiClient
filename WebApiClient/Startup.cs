using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace WebApiClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiClient", Version = "v1" });
            });
            //var clientCertificate =
            //new X509Certificate2("C:/PuntoaPunto-sandbox.pfx", "puntoapunto.2022");

            //services.AddHttpClient("ApiConti", c =>
            //{
            //}).ConfigurePrimaryHttpMessageHandler(() =>
            //{
            //    var handler = new HttpClientHandler();
            //    handler.ClientCertificates.Add(clientCertificate);
            //    return handler;
            //});

            //services.AddAuthentication(
            //   CertificateAuthenticationDefaults.AuthenticationScheme)
            //   .AddCertificate()
            //   // Adding an ICertificateValidationCache results in certificate auth caching the results.
            //   // The default implementation uses a memory cache.
            //   .AddCertificateCache();
            //services.AddAuthentication(
            //CertificateAuthenticationDefaults.AuthenticationScheme)
            //.AddCertificate(options =>
            //{
            //    options.Events = new CertificateAuthenticationEvents
            //    {
            //        OnCertificateValidated = context =>
            //        {
            //            var claims = new[]
            //            {
            //            new Claim(
            //                ClaimTypes.NameIdentifier,
            //                context.ClientCertificate.Subject,
            //                ClaimValueTypes.String,
            //                context.Options.ClaimsIssuer),
            //            new Claim(ClaimTypes.Name,
            //                context.ClientCertificate.Subject,
            //                ClaimValueTypes.String,
            //                context.Options.ClaimsIssuer)
            //            };

            //            context.Principal = new ClaimsPrincipal(
            //                new ClaimsIdentity(claims, context.Scheme.Name));
            //            context.Success();

            //            return Task.CompletedTask;
            //        }
            //    };
            //});

        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiClient v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //app.UseAuthentication();
        }
    }
}
