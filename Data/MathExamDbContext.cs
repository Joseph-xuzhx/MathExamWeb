using Microsoft.EntityFrameworkCore;
using MathExamWeb.Data.Models;

namespace MathExamWeb.Data;

/// <summary>
/// 数学考试系统数据库上下文
/// </summary>
public class MathExamDbContext : DbContext
{
    public MathExamDbContext(DbContextOptions<MathExamDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// 题目集合
    /// </summary>
    public DbSet<Question> Questions { get; set; }

    /// <summary>
    /// 考试记录集合
    /// </summary>
    public DbSet<ExamRecordEntity> ExamRecords { get; set; }

    /// <summary>
    /// 错题记录集合
    /// </summary>
    public DbSet<WrongQuestionEntity> WrongQuestions { get; set; }

    /// <summary>
    /// 题目模板集合
    /// </summary>
    public DbSet<QuestionTemplateEntity> QuestionTemplates { get; set; }

    /// <summary>
    /// 题目分类集合
    /// </summary>
    public DbSet<CategoryEntity> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 配置 Question 实体
        modelBuilder.Entity<Question>(entity =>
        {
            entity.ToTable("questions");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.Text)
                .HasColumnName("text")
                .IsRequired();

            entity.Property(e => e.Type)
                .HasColumnName("type")
                .HasConversion<string>()
                .IsRequired();

            entity.Property(e => e.Options)
                .HasColumnName("options")
                .HasConversion(
                    v => string.Join("|||", v),
                    v => v.Split("|||", StringSplitOptions.RemoveEmptyEntries).ToList()
                );

            entity.Property(e => e.CorrectAnswer)
                .HasColumnName("correct_answer")
                .IsRequired();

            entity.Property(e => e.Explanation)
                .HasColumnName("explanation");

            entity.Property(e => e.IsFractionAnswer)
                .HasColumnName("IsFractionAnswer")
                .IsRequired();

            entity.Property(e => e.Difficulty)
                .HasColumnName("difficulty")
                .HasConversion<string>()
                .IsRequired();

            entity.Property(e => e.Category)
                .HasColumnName("category")
                .HasMaxLength(100);

            entity.Property(e => e.Subject)
                .HasColumnName("subject")
                .HasMaxLength(50);

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            // 忽略不需要持久化的属性（仅在内存中使用）
            entity.Ignore(e => e.ShuffledOptions);
            entity.Ignore(e => e.ShuffledCorrectAnswerLetter);
            entity.Ignore(e => e.Tags);

            // 创建索引
            entity.HasIndex(e => e.Subject)
                .HasDatabaseName("idx_questions_subject");

            entity.HasIndex(e => e.Category)
                .HasDatabaseName("idx_questions_category");

            entity.HasIndex(e => e.Difficulty)
                .HasDatabaseName("idx_questions_difficulty");

            entity.HasIndex(e => e.CreatedAt)
                .HasDatabaseName("idx_questions_created_at");
        });

        // 配置 ExamRecordEntity 实体
        modelBuilder.Entity<ExamRecordEntity>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Subject)
                .HasDatabaseName("idx_exam_records_subject");

            entity.HasIndex(e => e.EndTime)
                .HasDatabaseName("idx_exam_records_end_time");

            entity.HasIndex(e => e.CreatedAt)
                .HasDatabaseName("idx_exam_records_created_at");
        });

        // 配置 WrongQuestionEntity 实体
        modelBuilder.Entity<WrongQuestionEntity>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Subject)
                .HasDatabaseName("idx_wrong_questions_subject");

            entity.HasIndex(e => e.QuestionId)
                .HasDatabaseName("idx_wrong_questions_question_id");

            entity.HasIndex(e => e.IsMastered)
                .HasDatabaseName("idx_wrong_questions_is_mastered");

            entity.HasIndex(e => e.WrongAt)
                .HasDatabaseName("idx_wrong_questions_wrong_at");

            entity.HasIndex(e => new { e.Subject, e.QuestionText })
                .HasDatabaseName("idx_wrong_questions_subject_text");
        });

        // 配置 QuestionTemplateEntity 实体
        modelBuilder.Entity<QuestionTemplateEntity>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.TemplateCode)
                .IsUnique()
                .HasDatabaseName("idx_question_templates_code");

            entity.HasIndex(e => e.Subject)
                .HasDatabaseName("idx_question_templates_subject");

            entity.HasIndex(e => e.TypeName)
                .HasDatabaseName("idx_question_templates_type");

            entity.HasIndex(e => e.IsActive)
                .HasDatabaseName("idx_question_templates_active");
        });

        // 配置 CategoryEntity 实体
        modelBuilder.Entity<CategoryEntity>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Subject)
                .HasDatabaseName("idx_categories_subject");

            entity.HasIndex(e => new { e.Subject, e.Name })
                .IsUnique()
                .HasDatabaseName("idx_categories_subject_name");

            entity.HasIndex(e => e.SortOrder)
                .HasDatabaseName("idx_categories_sort_order");

            entity.HasIndex(e => e.IsActive)
                .HasDatabaseName("idx_categories_active");
        });
    }
}
