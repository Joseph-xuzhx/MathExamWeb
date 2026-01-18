using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MathExamWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddQuestionTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "question_templates",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    subject = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    type_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    template_code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    template_content = table.Column<string>(type: "text", nullable: false),
                    parameters_config = table.Column<string>(type: "jsonb", nullable: false),
                    answer_expression = table.Column<string>(type: "text", nullable: false),
                    difficulty = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_question_templates", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_question_templates_active",
                table: "question_templates",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "idx_question_templates_code",
                table: "question_templates",
                column: "template_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_question_templates_subject",
                table: "question_templates",
                column: "subject");

            migrationBuilder.CreateIndex(
                name: "idx_question_templates_type",
                table: "question_templates",
                column: "type_name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "question_templates");
        }
    }
}
