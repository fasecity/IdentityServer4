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
        //1
        public static List<TestUser> GetUsers()
        {
            //returns users
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
                    }

                },//user ends

                 new TestUser
                {
                    SubjectId="1002",
                    Username="Claire",
                    Password="password",

                    Claims= new List<Claim>
                    {
                        new Claim("given_name","Claire"),
                        new Claim("family_name","Underwood"),

                    }

                }//user ends
            };//list ends

        }

        //2 not sure scope           
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            //returns this 
            //maps to subjectId ensures its ok
            return new List<IdentityResource> {

                new IdentityResources.OpenId(),

                new IdentityResources.Profile()

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
                    AllowedScopes =//gets scopes like profile data ect
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },

                    ClientSecrets =//needed for a client
                    {
                        new Secret("secret".Sha256())
                    }

                    
                    
                }
            };
        }

    }
}
