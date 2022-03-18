using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebEnterprise.Migrations
{
    public partial class all : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9a626ca0-720e-4509-9c10-521014e61747", "AQAAAAEAACcQAAAAEE6bKcLPDsRlLDTg2Rri75diY5VcU3b3Mic69UXHjashyOc8Emo3riEnB9YFGTBfug==", "9482df7c-6dc0-4c53-ad93-cabfea2439a0" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1de09f47-c092-4911-befd-79a079e0d49b", "AQAAAAEAACcQAAAAEPDM+XtSCDA+xERdtRE9MnmIBJPWSs3lpeWTdXmjjgOALGUse+GNQtzp/o23U9/kyQ==", "6c4b3f6e-5c60-4ea4-b119-81594fecc2b7" });
        }
    }
}
