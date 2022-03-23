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
                new Client
                {
                    ClientId = "movieClient", // unique name of the client
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {MovieApiName}
                }
            };
        }
    }

    public static IEnumerable<ApiResource> ApiResources => new List<ApiResource>();

    public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>();

    public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
    {
        new ApiScope(MovieApiName, "Movie API")
    };

    public static List<TestUser> TestUsers => new List<TestUser>();
}