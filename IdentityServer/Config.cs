using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServer;

public class Config
{
    private const string MovieApiName = "movieAPI";

    public static IEnumerable<Client> Clients
    {
        get
        {
            return new List<Client>
            {
                //new Client
                //{
                //    ClientId = "movieClient", // unique name of the client
                //    AllowedGrantTypes = GrantTypes.ClientCredentials,
                //    ClientSecrets =
                //    {
                //        new Secret("secret".Sha256())
                //    },
                //    AllowedScopes = { MovieApiName }
                //},
                new Client
                {
                    ClientId = "movies_mvc_client",         // must be unique
                    ClientName = "Movies MVC Web App",      // a description of whom the client is.
                    AllowedGrantTypes = GrantTypes.Hybrid,    // basically which flow we are using to get the token
                    RequirePkce = false,
                    AllowRememberConsent = false,
                    RedirectUris = new List<string>
                    {
                        "https://localhost:5002/signin-oidc" // this is the client app port
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "https://localhost:5002/signout-callback-oidc"
                    },
                    ClientSecrets = new List<Secret>
                    {
                        // used for client verification the client is whom they say they are.
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        // the scopes this client is allowed to have.
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        MovieApiName
                    }
                }
            };
        }
    }

    public static IEnumerable<ApiResource> ApiResources => new List<ApiResource>();

    public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        //new IdentityResource(MovieApiName,"Movie API")
    };

    public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
    {
        new ApiScope(MovieApiName, "Movie API"),
        //new ApiScope(IdentityServerConstants.StandardScopes.OpenId),
        //new ApiScope(IdentityServerConstants.StandardScopes.Profile)
    };

    public static List<TestUser> TestUsers => new List<TestUser>
    {
        new TestUser
        {
            SubjectId = "2d4ad762-e232-4538-9bd2-a063cab7186d",
            Username = "player1",
            Password = "player1",
            Claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.GivenName, "Player"),
                new Claim(JwtClaimTypes.FamilyName, "One")
            }
        }
    };
}