using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MacaMvc.IHttpHelpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

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
            // Add framework services.!!!!ALWAYS ADD AT THE FUCKING TOP !!!!!!
            services.AddMvc();

            //new policy makes [Authorize] availible by claims
            services.AddAuthorization((options) => {
                options.AddPolicy("CanOrderFrame", policybuilder =>
                {               
                    policybuilder.RequireAuthenticatedUser();
                    policybuilder.RequireClaim("role", "PayingUser");

                });
            });


            // register an IHttpContextAccessor so we can access the current
            // HttpContext in services by injecting it
            //---we use to pull out the token
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // register an IPayloadHttpClient-- make sure interface is implemented
            services.AddScoped<IHttpPayloadClient, HttpPayloadClient>();







            services.AddAuthentication(options =>
            {
           
                //this works
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";

            })

                 //add cookies added the option to denied path
                 .AddCookie("Cookies", (options) => {
                     options.AccessDeniedPath = "/Authorization/AccessDenied";
                 })

                //auth scheme goes here and oicc
                .AddOpenIdConnect("oidc", options =>
                    {
                        options.SignInScheme = "Cookies";
                        options.Authority = "https://localhost:44365/";
                        options.RequireHttpsMetadata = true;
                        options.ClientId = "MacaMVC";

                        options.Scope.Add("openid");
                        options.Scope.Add("profile");
                        options.Scope.Add("address");
                        options.Scope.Add("roles");//--- add roles scope
                        options.Scope.Add("apiclient");//---add api client fuckkkkkkkkkk
                       

                        options.ResponseType = "code id_token";                      
                        options.SaveTokens = true;
                        options.ClientSecret = "secret";
                        options.GetClaimsFromUserInfoEndpoint = true;//makes sure we get aditional userInfo

                        //events not working
                        options.Events = new OpenIdConnectEvents()
                        {

                            OnTicketReceived = ticketReceivedContext =>
                            {
                                return Task.CompletedTask;
                            },

                            OnTokenValidated = tokenValidatedContext =>
                            {
                                var identity = tokenValidatedContext.Principal.Identity
                                    as ClaimsIdentity;

                                var targetClaims = identity.Claims.Where(z => new[] { "sub","role" }.Contains(z.Type));

                            var newClaimsIdentity = new ClaimsIdentity(
                              targetClaims,
                              identity.AuthenticationType,
                              "given_name",
                              "role");

                                tokenValidatedContext.Principal =
                                    new ClaimsPrincipal(newClaimsIdentity);

                                return Task.CompletedTask;
                            },

                            OnUserInformationReceived = userInformationReceivedContext =>
                            {
                               userInformationReceivedContext.User.Remove("address");
                                return Task.FromResult(0);
                            }

                        };

                
                    });

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

            //clears default claim type
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultInboundClaimFilter.Clear();
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

//----------------------Notes-------------------------------------    
// notes:       
//in service
//----Extras:
//signout call redirect:@defaultz
// options.SignedOutCallbackPath = new Microsoft
//.AspNetCore.Http.PathString("https://localhost:44321/signout-callback-oidc");

// options.CallbackPath = new Microsoft // - maybe use for android
//.AspNetCore.Http.PathString(...)// can make new path to login instead of default


// // template from :
// https://docs.microsoft.com/en-us/aspnet/core/migration/1x-to-2x/identity-2x
//-----------------------------------------------------------------
//notes end


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
