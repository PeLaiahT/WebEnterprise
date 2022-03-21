using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebEnterprise.Migrations
{
    public partial class Run2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    LikeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdeaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.LikeId);
                    table.ForeignKey(
                        name: "FK_Likes_Ideas_IdeaID",
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
                values: new object[] { "4c9999a1-e155-43fb-8519-e538528e7b83", "AQAAAAEAACcQAAAAECMHrz3QNQ8qnvfSFgMNg2tcJ6WuV5NVIiFeM9Q8XvAwbpDUAP0TCVhe++hYPirB0w==", "dc871a2f-b03d-4f59-8d33-3a9fc0daa592" });

            migrationBuilder.CreateIndex(
                name: "IX_Likes_IdeaID",
                table: "Likes",
                column: "IdeaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "98e7a144-8d75-409c-9b56-7e9d9cb9b185", "AQAAAAEAACcQAAAAENAPAy6zPWV/Or1x0briZi/3hza1R66ScCdtAvMEpmXRyoXsl3tIfilo6yslsKMD4w==", "9d62f0cb-89b8-4f41-904e-12948516d193" });
        }
    }
}
