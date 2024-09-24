using Duende.IdentityServer.Services;
using GeekShop.IdentityServer.Configuration;
using GeekShop.IdentityServer.Initializer;
using GeekShop.IdentityServer.Model;
using GeekShop.IdentityServer.Model.Context;
using GeekShop.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GeekShop.IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connection = builder.Configuration["MySQLConnection:MySQLConnectionString"];

            builder.Services.AddDbContext<MySQLContext>(options => options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 25))));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<MySQLContext>()
                .AddDefaultTokenProviders();

            var builderIdentity = builder.Services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.EmitStaticAudienceClaim = true;
            }).AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
                .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
                .AddInMemoryClients(IdentityConfiguration.Clients)
                .AddAspNetIdentity<ApplicationUser>();

            //Uso para popular o banco
            builder.Services.AddScoped<IDbInitializer, DbInitializer>();
            builder.Services.AddScoped<IProfileService, ProfileService>();

            builderIdentity.AddDeveloperSigningCredential();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();
            var scope = app.Services.CreateScope();

            var dbInitializer = scope.ServiceProvider.GetService<IDbInitializer>();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthorization();


            dbInitializer.Initialize();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
