using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Translator.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "translations",
                columns: table => new
                {
                    translation_id = table.Column<Guid>(nullable: false, defaultValueSql: "newId()"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    translation_source = table.Column<string>(maxLength: 2048, nullable: false),
                    translation_sourcelang = table.Column<string>(maxLength: 255, nullable: false),
                    translation_destlang = table.Column<string>(maxLength: 255, nullable: false),
                    translation_result = table.Column<string>(maxLength: 2048, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_translations", x => x.translation_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "translations");
        }
    }
}
