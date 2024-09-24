using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace GeekShop.IdentityServer.Configuration
{
    public static class IdentityConfiguration
    {
        //Duas constantes para definir o tipo de usuário que estará usando a aplicação
        public const string Admin = "Admin";
        public const string Client = "Client";

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
            };

        //Api Scope = Identificadores ou recursos que um client pode acessar.
        //Nesse caso é o GeekShopWeb que irá acessar o IdentityServer para realizar as autenticações.
        //Existem dois tipos de escopo no IdentityServer, sendo eles: IdentityScope e ResourceScope.
        //IdentityScope: Contém um objeto com as informações do perfil, como: Nome, Sobrenome, Nome de Usuário, email e etc.
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("geek_shop", "GeekShop Server"),
                new ApiScope("read", "Read Data"),
                new ApiScope("write", "Write Data"),
                new ApiScope("delete", "Delete Data"),
            };

        //Criando o client.
        //Nesse caso o cliente é o GeekShopWeb
        //Client é o componente que solicita o token ao identity server, assim identificando o usuário e
        //permitindo ou não o acesso aos recursos.

        public static IEnumerable<Client> Clients => new List<Client>
        {
            //Client genérico
            new Client
            {
                ClientId = "client",

                //Posso deixar essa string no meu appsettings.json ou deixar hardcode que é o caso dessa vez.
                //Sempre usar secrets complexas.
                ClientSecrets = { new Secret("senha_super_segura_e_forte".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { "read", "write", "profile" }
            },

            //Client específico
             new Client
            {
                ClientId = "geek_shop",

                //Posso deixar essa string no meu appsettings.json ou deixar hardcode que é o caso dessa vez.
                //Sempre usar secrets complexas.
                ClientSecrets = { new Secret("senha_super_segura_e_forte".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = {"https://localhost:4430/signin-oidc"},
                PostLogoutRedirectUris = {"https://localhost:4430/signout-callback-oidc"},
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "geek_shop"
                }
            }
        };
    }
}
