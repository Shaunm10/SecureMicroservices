using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServer;

public class Config
{
    public static IEnumerable<Client> Clients => new List<Client>();

    public static IEnumerable<ApiResource> ApiResources => new List<ApiResource>();

    public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>();

    public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>();

    public static List<TestUser> TestUsers => new List<TestUser>();
}