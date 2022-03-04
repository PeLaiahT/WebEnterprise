using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebEnterprise.Data.Migrations
{
    public partial class AddCatgoy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Ideas_IdeaId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ideas",
                table: "Ideas");

            migrationBuilder.RenameTable(
                name: "Ideas",
                newName: "Idea");

            migrationBuilder.RenameColumn(
                name: "IdeaId",
                table: "Comments",
                newName: "IdeaID");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_IdeaId",
                table: "Comments",
                newName: "IX_Comments_IdeaID");

            migrationBuilder.AddColumn<int>(
                name: "IdCategory",
                table: "Idea",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "categoryIdCategory",
                table: "Idea",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Idea",
                table: "Idea",
                column: "IdeaID");

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    IdCategory = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Desciption = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.IdCategory);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Idea_categoryIdCategory",
                table: "Idea",
                column: "categoryIdCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Idea_IdeaID",
                table: "Comments",
                column: "IdeaID",
                principalTable: "Idea",
                principalColumn: "IdeaID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Idea_Category_categoryIdCategory",
                table: "Idea",
                column: "categoryIdCategory",
                principalTable: "Category",
                principalColumn: "IdCategory",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Idea_IdeaID",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Idea_Category_categoryIdCategory",
                table: "Idea");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Idea",
                table: "Idea");

            migrationBuilder.DropIndex(
                name: "IX_Idea_categoryIdCategory",
                table: "Idea");

            migrationBuilder.DropColumn(
                name: "IdCategory",
                table: "Idea");

            migrationBuilder.DropColumn(
                name: "categoryIdCategory",
                table: "Idea");

            migrationBuilder.RenameTable(
                name: "Idea",
                newName: "Ideas");

            migrationBuilder.RenameColumn(
                name: "IdeaID",
                table: "Comments",
                newName: "IdeaId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_IdeaID",
                table: "Comments",
                newName: "IX_Comments_IdeaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ideas",
                table: "Ideas",
                column: "IdeaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Ideas_IdeaId",
                table: "Comments",
                column: "IdeaId",
                principalTable: "Ideas",
                principalColumn: "IdeaID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
