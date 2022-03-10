using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebEnterprise.Migrations
{
    public partial class FixCustomUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e147fa21-254d-4d03-93ee-45d5e57e9270", "AQAAAAEAACcQAAAAECLbCP6OPihRcPcMSrICalhLRmLq9NPGddyCIHwf3toaQK+skG6JCj08J6I34oFtew==", "eed0669a-25fd-44ee-aa9e-b71dac9a4393" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e74146a7-71e7-42bc-927e-57c034f4207a", "AQAAAAEAACcQAAAAELjkqbGaPBZUfhZJghfm/Z025Y0waHjtpwLp42Qr/7gxN52O+5eRGf0PdlCK7BG9zg==", "522244fc-61b8-4762-9665-2aae2f7f3706" });
        }
    }
}
