using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lexora.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VocabularyEntry",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Word = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Definition = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Example = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VocabularyEntry", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VocabularyEntry_Definition",
                table: "VocabularyEntry",
                column: "Definition");

            migrationBuilder.CreateIndex(
                name: "IX_VocabularyEntry_Word",
                table: "VocabularyEntry",
                column: "Word");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VocabularyEntry");
        }
    }
}
