using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthIDPserver
{
    public static class Config
    {
        //1 this is a test class
        public static List<TestUser> GetUsers()
        {
            //returns users
            //add role based schema ex.free user
            return new List<TestUser>
            {
               
                new TestUser
                {
                    SubjectId="1001",
                    Username="Frank",
                    Password="password",
                    
                    Claims= new List<Claim>
                    {
                        new Claim("given_name","Frank"),
                        new Claim("family_name","Underwood"),
                        new Claim("address","1 addy rd unit 233"),                
                        new Claim("role", "FreeUser")


                    }

                },//user ends

                //add role based schema ex.free user
                 new TestUser
                {
                    SubjectId="1002",
                    Username="Claire",
                    Password="password",

                    Claims= new List<Claim>
                    {
                        new Claim("given_name","Claire"),
                        new Claim("family_name","Underwood"),
                        new Claim("address","1 fuy guy ave"),
                        new Claim("role", "PayingUser")


                    }

                }//user ends
            };//list ends

        }

        //2 not sure scope
        //returns this 
        //maps to subjectId ensures its ok
        //roles isnt a standard scope add in ctor of IR: 
        //name of reasorce //display name// List of custom claim type role   
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            
            return new List<IdentityResource> {

                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResource("roles","Roles",new List<string>(){"role"})
              

            };
        }


        //adding API RESOURCE --------------------------------------//
        /// <summary>
        /// this reps resource scopes
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[] {
                new ApiResource("apiclient", "api client", new[] { "role" })
                {
                    // ApiSecrets = new[] { new Secret("apisecret".Sha256()) } }

                    // ApiSecrets ={ new Secret("apisecret".Sha256()) } 
                }
            };
        }



        //3 clients
        public static IEnumerable<Client> GetClients()
        {
            //ret clients are actual clientside apps not people
            
            return new List<Client>
            {
                new Client
                {
                    ClientName="MVC client",
                    ClientId="MacaMVC",
                    AllowedGrantTypes=GrantTypes.Hybrid,//use implict or other in future
                    RedirectUris= new List<string>()
                    {
                        //THIS IS REDIRECT URL:from the ssl debug from maca mvc
                        "https://localhost:44321/signin-oidc"
                    },
                    AllowedScopes =//gets scopes like profile data ect/2/adding roles scope
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                       "roles",
                       "apiclient" //rem to add api res to startup



                    },

                    ClientSecrets =//needed for a client
                    {
                        new Secret("secret".Sha256())
                    },

                    //signout call redirect:@defaultz-- 
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:44321/signout-callback-oidc"
                    },

                       //make sure this is true or you 
                       //-you know waste time getting the claims
                      AlwaysIncludeUserClaimsInIdToken = true 
               

                    
                    
                }

               

            };
        }

    }
}
