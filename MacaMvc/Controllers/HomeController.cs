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

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
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
