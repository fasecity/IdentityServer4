using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MacaMvc
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
         
            services.AddAuthentication(options =>
            {
                //this works
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies")      //auth scheme goes here add cookies and oicc
                .AddOpenIdConnect("oidc", options =>
                    {
                        options.SignInScheme = "Cookies";
                        options.Authority = "https://localhost:44365/";
                        options.RequireHttpsMetadata = true;
                        options.ClientId = "MacaMVC";
                        options.Scope.Add("openid");
                        options.Scope.Add("profile");
                        options.ResponseType = "code id_token";                      
                        options.SaveTokens = true;
                        options.ClientSecret = "secret";
                        options.GetClaimsFromUserInfoEndpoint = true;//makes sure we get aditional userInfo


                        //signout call redirect:@defaultz
                        // options.SignedOutCallbackPath = new Microsoft
                        //.AspNetCore.Http.PathString("https://localhost:44321/signout-callback-oidc");

                        // options.CallbackPath = new Microsoft // - maybe use for android
                        //.AspNetCore.Http.PathString(...)// can make new path to login instead of default
                       
                        // template from :
                        // https://docs.microsoft.com/en-us/aspnet/core/migration/1x-to-2x/identity-2x


                    });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            
            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

//----------------------Better Modern example using FB-------------------------------------         
//  services.AddIdentity<ApplicationUser, IdentityRole>()   
//.AddEntityFrameworkStores<ApplicationDbContext>();

// If you want to tweak Identity cookies, they're no longer part of IdentityOptions.
//services.ConfigureApplicationCookie(options => options.LoginPath = "/Account/LogIn");
//services.AddAuthentication()
//        .AddFacebook(options =>
//        {
//            options.AppId = Configuration["auth:facebook:appid"];
//            options.AppSecret = Configuration["auth:facebook:appsecret"];
//        });
//---------------------------------------------------------------------------------


//--------------------Auth Cookies Obsolete Garbage------------------------------//
//add auth before mvc  shit soo many depriCATES
//app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
//{
//    AuthenticationScheme = "oidc",
//    Authority = "https://localhost:44365/",
//    RequireHttpsMetadata = true,
//    ClientId = "ClientID at IDP server ex ImageGalary Client",
//    Scope = { "openid", "profile" },
//    ResponseType = "code id_token",
//    SignInScheme = "Cookies",//---make Cookies becasuse abouve is cookies
//    SaveTokens = true

//    //--If you wanna changepath from localhost/signin-odic
//    ///CallbackPath= new PathString("...")
//});


//---------------------------------------------------------------//
