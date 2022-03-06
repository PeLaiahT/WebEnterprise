using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebEnterprise.Migrations
{
    public partial class SchoolDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "file",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "1", 0, "bf0758e6-f81c-4c37-8e23-73cf8115165a", "IdentityUser", "admin@gmail.com", true, true, null, null, "admin", "AQAAAAEAACcQAAAAEJ938vEK8pE2hjEw1PZmyTD3bNYbx2ptLK9Zx+HRISORCezf6AO8ZBj+hIs8q2XJPg==", null, false, "8d94dc93-7cab-4b8e-8d1a-a68fd02c4d50", false, "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "file",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e7b94cc1-fde4-4a6e-82de-821bdfba6f00", "AQAAAAEAACcQAAAAENsY+S2hvCwNNZMGaXuFLS8CMkeVcyqQTAc9h2CUWFnmNygn4queCBpjfMEnqyjB4Q==", "b0aa7a41-30e4-456a-9f2d-8b4d2b5d7f56" });
        }
    }
}
