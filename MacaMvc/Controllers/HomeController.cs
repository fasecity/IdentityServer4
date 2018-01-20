using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MacaMvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using IdentityModel.Client;

namespace MacaMvc.Controllers
{
    [Authorize]//kicks in auth frm middleware
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> About()
        {
            //get saved token
            var xtoken = await AuthenticationHttpContextExtensions
                .GetTokenAsync(HttpContext,OpenIdConnectParameterNames.IdToken);

            //write it out
            Debug.WriteLine("Iden token " + xtoken);


            //writeout Claims
            foreach (var claim in User.Claims)
            {
                Debug.WriteLine("claim type: " + claim.Type + "-- Claim Value " + claim.Value);
            }
            ViewData["Message"] = xtoken;

            return View();
        }

        /// <summary>
        ///  INSTALL IDENTITYMODEL NUGET PACKAGE
        ///  this is a helper class.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Contact()
        {
            //when getting info pass in authority if IDP
            var discoveryClient = new DiscoveryClient("https://localhost:44365/");
          
            //gets meta data 
            var metaDataResponse = await discoveryClient.GetAsync();

            //get userInfo endpoint  pass in meta data
            var userInfoClient = new UserInfoClient(metaDataResponse.UserInfoEndpoint);

            //need an access token to call this endpoint above
            var accessToken = await AuthenticationHttpContextExtensions
                .GetTokenAsync(HttpContext, OpenIdConnectParameterNames.AccessToken);

            //call endpoint pass in access token
            var response = await userInfoClient.GetAsync(accessToken);

            //throw an error exeption if any errors:
            if (response.IsError)
            {
                throw new Exception("error on user endpoint",response.Exception);
               
            }

            // response object has a list of claims that are returned from the user info endpoint
            //match scopes that are in the access token-- if null its gonna be null
            var address = response.Claims.FirstOrDefault(x => x.Type == "address")?.Value;

            //return view and pass in new orderview frame model with address in ctor
            return View(new OrderViewFrameModel(address));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// THIS LOGS OUT OF BOTH IPD AND mvc app
        /// </summary>
        /// <returns></returns>
        public async Task Logout()
        {
            //only logs out of client not IDP
            await AuthenticationHttpContextExtensions.SignOutAsync(HttpContext, "Cookies");
            await AuthenticationHttpContextExtensions.SignOutAsync(HttpContext, "oidc");


        }

    }
}
