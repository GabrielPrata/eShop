using GeekShop.IdentityServer.Configuration;
using GeekShop.IdentityServer.Model;
using GeekShop.IdentityServer.Model.Context;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GeekShop.IdentityServer.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly MySQLContext _context;
        private readonly UserManager<ApplicationUser> _user;
        private readonly RoleManager<IdentityRole> _role;

        public DbInitializer(MySQLContext context, UserManager<ApplicationUser> user, RoleManager<IdentityRole> role)
        {
            _context = context;
            _user = user;
            _role = role;
        }

        public void Initialize()
        {
            //Verifica se há um usuário administrador, para popular ou nao o banco de dados
            if (_role.FindByNameAsync(IdentityConfiguration.Admin).Result != null) return;

            //Gravando as roles (tabela aspnetroles)
            _role.CreateAsync(new IdentityRole(IdentityConfiguration.Admin)).GetAwaiter().GetResult();
            _role.CreateAsync(new IdentityRole(IdentityConfiguration.Client)).GetAwaiter().GetResult();


            //Gravando o usuário
            ApplicationUser admin = new ApplicationUser()
            {
                UserName = "admin",
                Email = "admin@admin.com",
                EmailConfirmed = true,
                PhoneNumber = "+55 (19) 12345-6789",
                FirstName = "Admin",
                LastName = "da Silva"
            };

            //Ligando as roles com o user
            _user.CreateAsync(admin, "Senha123!").GetAwaiter().GetResult();
            _user.AddToRoleAsync(admin, IdentityConfiguration.Admin).GetAwaiter().GetResult();

            //Adicionando as claims
            var adminClaims = _user.AddClaimsAsync(admin, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{admin.FirstName} {admin.LastName}"),
                new Claim(JwtClaimTypes.GivenName, admin.FirstName),
                new Claim(JwtClaimTypes.FamilyName, admin.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.Admin),
            }).Result;

            //Admin ja feito, agora vou fazer o user(client)
            ApplicationUser client = new ApplicationUser()
            {
                UserName = "client",
                Email = "client@client.com",
                EmailConfirmed = true,
                PhoneNumber = "+55 (19) 12345-6789",
                FirstName = "Client",
                LastName = "da Silva"
            };

            //Ligando as roles com o user
            _user.CreateAsync(client, "Senha123!").GetAwaiter().GetResult();
            _user.AddToRoleAsync(client, IdentityConfiguration.Client).GetAwaiter().GetResult();

            //Adicionando as claims
            var clientClaims = _user.AddClaimsAsync(client, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{client.FirstName} {client.LastName}"),
                new Claim(JwtClaimTypes.GivenName, client.FirstName),
                new Claim(JwtClaimTypes.FamilyName, client.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.Client),
            }).Result;
        }
    }
}
