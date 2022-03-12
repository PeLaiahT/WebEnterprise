using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebEnterprise.Migrations
{
    public partial class _5h11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomUserDTO",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomUserDTO", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cbc00fe9-e4e1-4d57-b7b9-09fff98c45e7", "AQAAAAEAACcQAAAAENmjW1OK0GK/ARLY33IHO1FmpDxoHM6OEFV1DlNdW7HHK/h5NqHleqCedz3i3+XyAw==", "07614bf8-7694-4f22-b823-e5472f02609e" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomUserDTO");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3b802f04-c166-4831-90d5-a922484b38c9", "AQAAAAEAACcQAAAAENZEHYmNtBaSRttEuV215kNnHRvTJA71ACgMiGA/0xoQIFUWl9jlpnf/bCkojgIJXw==", "be3e542b-82c0-42e5-9811-45de86d970b6" });
        }
    }
}
