using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace DataAccess.Config
{
    public static class Config
    {
        public static IEnumerable<ApiScope> ApiScopes =>
       new List<ApiScope>
       {
            new ApiScope("api1", "My API", new[]{"role,name"})
       };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
            new Client
            {
                ClientId = "client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = { "api1" }
            }
            };
    }
}
