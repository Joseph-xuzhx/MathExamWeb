using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MathExamWeb.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "questions",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    text = table.Column<string>(type: "text", nullable: false),
                    correct_answer = table.Column<string>(type: "text", nullable: false),
                    options = table.Column<string>(type: "text", nullable: false),
                    explanation = table.Column<string>(type: "text", nullable: false),
                    IsFractionAnswer = table.Column<bool>(type: "boolean", nullable: false),
                    difficulty = table.Column<string>(type: "text", nullable: false),
                    category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    subject = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Tags = table.Column<List<string>>(type: "text[]", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_questions", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_questions_category",
                table: "questions",
                column: "category");

            migrationBuilder.CreateIndex(
                name: "idx_questions_created_at",
                table: "questions",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "idx_questions_difficulty",
                table: "questions",
                column: "difficulty");

            migrationBuilder.CreateIndex(
                name: "idx_questions_subject",
                table: "questions",
                column: "subject");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "questions");
        }
    }
}
