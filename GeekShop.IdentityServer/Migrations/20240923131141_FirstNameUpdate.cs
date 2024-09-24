using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeekShop.IdentityServer.Migrations
{
    public partial class FirstNameUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UsertUserName",
                table: "AspNetUsers",
                newName: "FirstName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "AspNetUsers",
                newName: "UsertUserName");
        }
    }
}
