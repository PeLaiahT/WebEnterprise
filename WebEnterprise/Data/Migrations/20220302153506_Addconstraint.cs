using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebEnterprise.Data.Migrations
{
    public partial class Addconstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdeaId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_IdeaId",
                table: "Comments",
                column: "IdeaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Ideas_IdeaId",
                table: "Comments",
                column: "IdeaId",
                principalTable: "Ideas",
                principalColumn: "IdeaID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Ideas_IdeaId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_IdeaId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "IdeaId",
                table: "Comments");
        }
    }
}
