using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MathExamWeb.Data.Models;

/// <summary>
/// 错题记录实体（数据库表）
/// </summary>
[Table("wrong_questions")]
public class WrongQuestionEntity
{
    /// <summary>
    /// 主键
    /// </summary>
    [Key]
    [Column("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// 题目ID
    /// </summary>
    [Column("question_id")]
    [Required]
    [MaxLength(100)]
    public string QuestionId { get; set; } = "";

    /// <summary>
    /// 科目
    /// </summary>
    [Column("subject")]
    [Required]
    [MaxLength(50)]
    public string Subject { get; set; } = "";

    /// <summary>
    /// 题目文本
    /// </summary>
    [Column("question_text")]
    [Required]
    public string QuestionText { get; set; } = "";

    /// <summary>
    /// 正确答案
    /// </summary>
    [Column("correct_answer")]
    [Required]
    public string CorrectAnswer { get; set; } = "";

    /// <summary>
    /// 用户答案
    /// </summary>
    [Column("user_answer")]
    public string UserAnswer { get; set; } = "";

    /// <summary>
    /// 题目解析
    /// </summary>
    [Column("explanation")]
    public string Explanation { get; set; } = "";

    /// <summary>
    /// 题目分类
    /// </summary>
    [Column("category")]
    [MaxLength(100)]
    public string Category { get; set; } = "";

    /// <summary>
    /// 题目类型
    /// </summary>
    [Column("question_type")]
    [MaxLength(50)]
    public string QuestionType { get; set; } = "";

    /// <summary>
    /// 难度
    /// </summary>
    [Column("difficulty")]
    [MaxLength(20)]
    public string Difficulty { get; set; } = "";

    /// <summary>
    /// 完整题目数据（JSON格式）
    /// </summary>
    [Column("question_json", TypeName = "jsonb")]
    public string QuestionJson { get; set; } = "{}";

    /// <summary>
    /// 答错时间
    /// </summary>
    [Column("wrong_at")]
    public DateTime WrongAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// 错误次数
    /// </summary>
    [Column("wrong_count")]
    public int WrongCount { get; set; } = 1;

    /// <summary>
    /// 是否已掌握
    /// </summary>
    [Column("is_mastered")]
    public bool IsMastered { get; set; } = false;

    /// <summary>
    /// 掌握时间
    /// </summary>
    [Column("mastered_at")]
    public DateTime? MasteredAt { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// 更新时间
    /// </summary>
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
