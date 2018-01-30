﻿using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MacaMvc.IHttpHelpers
{
    public class HttpPayloadClient : IHttpPayloadClient
    {
        //new interface for accessor
        private readonly IHttpContextAccessor _httpContextAccessor;
       
        //make a private var 
        private HttpClient _httpClient = new HttpClient();


        //ctor
        public HttpPayloadClient(IHttpContextAccessor httpContextAccessor)
        {
            //inject var Ihttpacessor
            _httpContextAccessor = httpContextAccessor;
        }


        //interfrace method
        public async Task<HttpClient> GetClient()
        {
          //  string accessToken = await GetValidAccessToken();

            //if (!string.IsNullOrEmpty(accessToken))
            //{
            //    _httpClient.SetBearerToken(accessToken);
            //}

            _httpClient.BaseAddress = new Uri("https://localhost:44370/");//notsure but this is api client
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return _httpClient;

        }

        //--------------2 helper methods getvalidtoken() and RenewTokens() -----------------------------


        ////1. get valid accesstoken Token
        //private async Task<string> GetValidAccessToken()
        //{
        //    var currentContext = _httpContextAccessor.HttpContext;
        //    var expiresAtToken = await currentContext.GetTokenAsync("expires_at");
        //    var expiresAt = string.IsNullOrWhiteSpace(expiresAtToken) ? DateTime.MinValue : DateTime.Parse(expiresAtToken).AddSeconds(-60).ToUniversalTime();
        //    string accessToken = await (expiresAt < DateTime.UtcNow ?
        //        RenewTokens() : currentContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken));
        //    return accessToken;
        //}


        ////2 renew token
        //private async Task<string> RenewTokens()
        //{
        //    // get the current HttpContext to access the tokens
        //    var currentContext = _httpContextAccessor.HttpContext;

        //    // get the metadata
        //    var discoveryClient = new DiscoveryClient("https://localhost:44365/");  //====phf
        //    var metaDataResponse = await discoveryClient.GetAsync();

        //    // create a new token client to get new tokens
        //    var tokenClient = new TokenClient(metaDataResponse.TokenEndpoint,
        //        "MacaMVC", "secret");

        //    // get the saved refresh token
        //    var currentRefreshToken = await currentContext
        //        .GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

        //    // refresh the tokens
        //    var tokenResult = await tokenClient.RequestRefreshTokenAsync(currentRefreshToken);

        //    if (!tokenResult.IsError)
        //    {
        //        // get current tokens
        //        var old_id_token = await currentContext.GetTokenAsync("id_token");
        //        var new_access_token = tokenResult.AccessToken;
        //        var new_refresh_token = tokenResult.RefreshToken;

        //        // get new tokens and expiration time
        //        var tokens = new List<AuthenticationToken>();
        //        tokens.Add(new AuthenticationToken { Name = OpenIdConnectParameterNames.IdToken, Value = old_id_token });
        //        tokens.Add(new AuthenticationToken { Name = OpenIdConnectParameterNames.AccessToken, Value = new_access_token });
        //        tokens.Add(new AuthenticationToken { Name = OpenIdConnectParameterNames.RefreshToken, Value = new_refresh_token });

        //        var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResult.ExpiresIn);
        //        tokens.Add(new AuthenticationToken { Name = "expires_at", Value = expiresAt.ToString("o", CultureInfo.InvariantCulture) });

        //        // store tokens and sign in with renewed tokens
        //        var info = await currentContext.AuthenticateAsync("Cookies");
        //        info.Properties.StoreTokens(tokens);
        //        await currentContext.SignInAsync("Cookies", info.Principal, info.Properties);

        //        // return the new access token 
        //        return tokenResult.AccessToken;
        //    }
        //    else
        //    {
        //        throw new Exception("Problem encountered while refreshing tokens.",
        //            tokenResult.Exception);
        //    }
        //}



    }
}
