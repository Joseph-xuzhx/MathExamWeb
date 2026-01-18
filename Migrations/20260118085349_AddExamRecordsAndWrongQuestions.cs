using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MathExamWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddExamRecordsAndWrongQuestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tags",
                table: "questions");

            migrationBuilder.CreateTable(
                name: "exam_records",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    subject = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    score = table.Column<int>(type: "integer", nullable: false),
                    total_questions = table.Column<int>(type: "integer", nullable: false),
                    correct_count = table.Column<int>(type: "integer", nullable: false),
                    accuracy_rate = table.Column<double>(type: "double precision", nullable: false),
                    difficulty = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    answers_json = table.Column<string>(type: "jsonb", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_records", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "wrong_questions",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    question_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    subject = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    question_text = table.Column<string>(type: "text", nullable: false),
                    correct_answer = table.Column<string>(type: "text", nullable: false),
                    user_answer = table.Column<string>(type: "text", nullable: false),
                    explanation = table.Column<string>(type: "text", nullable: false),
                    category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    question_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    difficulty = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    question_json = table.Column<string>(type: "jsonb", nullable: false),
                    wrong_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    wrong_count = table.Column<int>(type: "integer", nullable: false),
                    is_mastered = table.Column<bool>(type: "boolean", nullable: false),
                    mastered_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wrong_questions", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_exam_records_created_at",
                table: "exam_records",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "idx_exam_records_end_time",
                table: "exam_records",
                column: "end_time");

            migrationBuilder.CreateIndex(
                name: "idx_exam_records_subject",
                table: "exam_records",
                column: "subject");

            migrationBuilder.CreateIndex(
                name: "idx_wrong_questions_is_mastered",
                table: "wrong_questions",
                column: "is_mastered");

            migrationBuilder.CreateIndex(
                name: "idx_wrong_questions_question_id",
                table: "wrong_questions",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "idx_wrong_questions_subject",
                table: "wrong_questions",
                column: "subject");

            migrationBuilder.CreateIndex(
                name: "idx_wrong_questions_subject_text",
                table: "wrong_questions",
                columns: new[] { "subject", "question_text" });

            migrationBuilder.CreateIndex(
                name: "idx_wrong_questions_wrong_at",
                table: "wrong_questions",
                column: "wrong_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exam_records");

            migrationBuilder.DropTable(
                name: "wrong_questions");

            migrationBuilder.AddColumn<List<string>>(
                name: "Tags",
                table: "questions",
                type: "text[]",
                nullable: false);
        }
    }
}
