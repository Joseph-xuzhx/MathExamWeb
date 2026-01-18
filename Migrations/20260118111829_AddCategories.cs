using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MathExamWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    subject = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    group_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_categories_active",
                table: "categories",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "idx_categories_sort_order",
                table: "categories",
                column: "sort_order");

            migrationBuilder.CreateIndex(
                name: "idx_categories_subject",
                table: "categories",
                column: "subject");

            migrationBuilder.CreateIndex(
                name: "idx_categories_subject_name",
                table: "categories",
                columns: new[] { "subject", "name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}
