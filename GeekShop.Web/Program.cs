using GeekShop.Web.Services;
using GeekShop.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;

namespace GeekShop.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";

            })
                .AddCookie("Cookies", c => c.ExpireTimeSpan = TimeSpan.FromMinutes(10))
                .AddOpenIdConnect(
                "oidc", options =>
                {
                    options.Authority = builder.Configuration["ServiceUrls:IdentityServer"];
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.ClientId = "geek_shop";
                    //Secret que foi usada para gerar o token
                    //Essa senha pode vir de um arquivo, como o appsettings, por exemplo
                    options.ClientSecret = "senha_super_segura_e_forte";
                    options.ResponseType = "code";
                    options.ClaimActions.MapJsonKey("role", "role", "role");
                    options.ClaimActions.MapJsonKey("sub", "sub", "sub");
                    options.TokenValidationParameters.NameClaimType = "name";
                    options.TokenValidationParameters.RoleClaimType = "role";
                    options.Scope.Add("geek_shop");
                    options.SaveTokens = true;
                }
                );
            builder.Services.AddHttpClient<IProductService, ProductService>(c => c.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"]));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}