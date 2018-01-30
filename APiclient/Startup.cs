using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace APiclient
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

            //policy
            //new policy makes [Authorize] availible by claims
            services.AddAuthorization((options) =>
            {
                options.AddPolicy("Blaka", policybuilder =>
                {
                    policybuilder.RequireAuthenticatedUser();
                    policybuilder.RequireClaim("role", "PayingUser");

                });
            });

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
           .AddIdentityServerAuthentication(options =>
           {
               // base-address of your identityserver 
               //https://localhost:44365 is IDP  

               options.Authority = "https://localhost:44365/";
               options.RequireHttpsMetadata = true;
               options.ApiName = "apiclient";

           });



            services.AddMvc();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            ////err pg
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        // ensure generic 500 status code on fault.
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                    });
                });
            }




            app.UseAuthentication();
            app.UseStaticFiles();

            app.UseMvc();

        } 
    }
}

// base-address of your identityserver 
//https://localhost:44365 is IDP

//app.UseIdentityServer(,new IdentityServerAuthenticationOptions()


//    options =>
//    {
//        options.Authority = "https://localhost:44365/";

//        options.RequireHttpsMetadata = true;

//        options.ApiName = "apiclient";
//    }); 
