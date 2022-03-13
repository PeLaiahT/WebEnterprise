using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebEnterprise.Migrations
{
    public partial class File : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Documment",
                table: "Ideas");

            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    FileID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdeaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.FileID);
                    table.ForeignKey(
                        name: "FK_File_Ideas_IdeaID",
                        column: x => x.IdeaID,
                        principalTable: "Ideas",
                        principalColumn: "IdeaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileDetails",
                columns: table => new
                {
                    FileDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileDetails", x => x.FileDetailID);
                    table.ForeignKey(
                        name: "FK_FileDetails_File_FileID",
                        column: x => x.FileID,
                        principalTable: "File",
                        principalColumn: "FileID");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4c8c6789-a2a8-4c86-8249-f868997a706e", "AQAAAAEAACcQAAAAEDNFsimFG7CMKkoI7hBL5znfyYzTg69UavDLbiazI1gGjw0cA1hmwdF3YH099b1Tzw==", "fd93b0f2-666f-4187-bc5b-677077e471aa" });

            migrationBuilder.CreateIndex(
                name: "IX_File_IdeaID",
                table: "File",
                column: "IdeaID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileDetails_FileID",
                table: "FileDetails",
                column: "FileID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileDetails");

            migrationBuilder.DropTable(
                name: "File");

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
                values: new object[] { "3b802f04-c166-4831-90d5-a922484b38c9", "AQAAAAEAACcQAAAAENZEHYmNtBaSRttEuV215kNnHRvTJA71ACgMiGA/0xoQIFUWl9jlpnf/bCkojgIJXw==", "be3e542b-82c0-42e5-9811-45de86d970b6" });
        }
    }
}
