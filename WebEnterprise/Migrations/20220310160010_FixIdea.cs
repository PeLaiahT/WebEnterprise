using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebEnterprise.Migrations
{
    public partial class FixIdea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Documment",
                table: "Ideas",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a348adac-05a9-4734-9ca5-e23ad019563b", "AQAAAAEAACcQAAAAEC///4aZs2gHokbn9eFLDn2yRex+zQ3rKSysZ/sDUoi5hUYnGbdYqosuhlWwWPuNyA==", "06928078-d455-4921-a389-80b11a1d866e" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Documment",
                table: "Ideas");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e147fa21-254d-4d03-93ee-45d5e57e9270", "AQAAAAEAACcQAAAAECLbCP6OPihRcPcMSrICalhLRmLq9NPGddyCIHwf3toaQK+skG6JCj08J6I34oFtew==", "eed0669a-25fd-44ee-aa9e-b71dac9a4393" });
        }
    }
}
