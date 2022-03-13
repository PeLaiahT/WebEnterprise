using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebEnterprise.Migrations
{
    public partial class fixfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileDetails_File_FileID",
                table: "FileDetails");

            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.RenameColumn(
                name: "FileID",
                table: "FileDetails",
                newName: "FileModelFileID");

            migrationBuilder.RenameIndex(
                name: "IX_FileDetails_FileID",
                table: "FileDetails",
                newName: "IX_FileDetails_FileModelFileID");

            migrationBuilder.CreateTable(
                name: "FileModel",
                columns: table => new
                {
                    FileID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdeaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileModel", x => x.FileID);
                    table.ForeignKey(
                        name: "FK_FileModel_Ideas_IdeaID",
                        column: x => x.IdeaID,
                        principalTable: "Ideas",
                        principalColumn: "IdeaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "aa7bf875-b1b3-45ef-bc37-5f768ec9af08", "AQAAAAEAACcQAAAAEJ9gthQ88DRTvZM0h98hiFXWAmcGWKrgFx1u+af9DErdeFY+yC8qezSMGgQyMYe+sQ==", "0217cb80-89d1-455a-a0ad-c959a51f80a6" });

            migrationBuilder.CreateIndex(
                name: "IX_FileModel_IdeaID",
                table: "FileModel",
                column: "IdeaID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FileDetails_FileModel_FileModelFileID",
                table: "FileDetails",
                column: "FileModelFileID",
                principalTable: "FileModel",
                principalColumn: "FileID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileDetails_FileModel_FileModelFileID",
                table: "FileDetails");

            migrationBuilder.DropTable(
                name: "FileModel");

            migrationBuilder.RenameColumn(
                name: "FileModelFileID",
                table: "FileDetails",
                newName: "FileID");

            migrationBuilder.RenameIndex(
                name: "IX_FileDetails_FileModelFileID",
                table: "FileDetails",
                newName: "IX_FileDetails_FileID");

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

            migrationBuilder.AddForeignKey(
                name: "FK_FileDetails_File_FileID",
                table: "FileDetails",
                column: "FileID",
                principalTable: "File",
                principalColumn: "FileID");
        }
    }
}
