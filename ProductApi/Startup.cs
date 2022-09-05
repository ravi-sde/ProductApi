using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductApi.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private static readonly string CustomApiScheme = "CustomApiScheme";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<ProductDbContext>(item => item.UseSqlServer(Configuration.GetConnectionString("myconn")));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = AzureADDefaults.CookieScheme;
                options.DefaultChallengeScheme = CustomApiScheme;
                options.DefaultSignInScheme = AzureADDefaults.CookieScheme;
            })
           .AddAzureAD(options =>
           {
               Configuration.Bind("AzureAd", options);
           })
           .AddScheme<AuthenticationSchemeOptions, CustomApiAuthenticationHandler>(CustomApiScheme, options => { });

            services.Configure<CookieAuthenticationOptions>(AzureADDefaults.CookieScheme, options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.MaxAge = new TimeSpan(7, 0, 0, 0);
            });

            services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, options =>
            {
                options.Authority = options.Authority + "/v2.0/";         // Microsoft identity platform
                options.TokenValidationParameters.ValidateIssuer = false; // accept several tenants (here simplified)
            });

            //Allowing the Cors Policy , in order to call the web API from different domain 
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });

            });
            //Allow Cors Policy end 

            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c=>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Products API V1");
                });
                    
            }

            //Using the Cors Policy 
            app.UseCors("CorsPolicy");
            //Cors Policy use end 
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
