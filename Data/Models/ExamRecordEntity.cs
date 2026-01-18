using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MathExamWeb.Data.Models;

/// <summary>
/// 考试记录实体（数据库表）
/// </summary>
[Table("exam_records")]
public class ExamRecordEntity
{
    /// <summary>
    /// 主键
    /// </summary>
    [Key]
    [Column("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// 科目
    /// </summary>
    [Column("subject")]
    [Required]
    [MaxLength(50)]
    public string Subject { get; set; } = "";

    /// <summary>
    /// 开始时间
    /// </summary>
    [Column("start_time")]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    [Column("end_time")]
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 得分
    /// </summary>
    [Column("score")]
    public int Score { get; set; }

    /// <summary>
    /// 总题数
    /// </summary>
    [Column("total_questions")]
    public int TotalQuestions { get; set; }

    /// <summary>
    /// 答对数量
    /// </summary>
    [Column("correct_count")]
    public int CorrectCount { get; set; }

    /// <summary>
    /// 正确率
    /// </summary>
    [Column("accuracy_rate")]
    public double AccuracyRate { get; set; }

    /// <summary>
    /// 难度级别（可选）
    /// </summary>
    [Column("difficulty")]
    [MaxLength(20)]
    public string? Difficulty { get; set; }

    /// <summary>
    /// 答题详情（JSON格式）
    /// </summary>
    [Column("answers_json", TypeName = "jsonb")]
    public string AnswersJson { get; set; } = "[]";

    /// <summary>
    /// 创建时间
    /// </summary>
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
