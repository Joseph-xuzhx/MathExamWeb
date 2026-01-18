using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MathExamWeb.Data.Models;

/// <summary>
/// 题目模板实体（用于应用题、分数运算等题型的模板管理）
/// </summary>
[Table("question_templates")]
public class QuestionTemplateEntity
{
    /// <summary>
    /// 主键
    /// </summary>
    [Key]
    [Column("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// 科目（chinese/math）
    /// </summary>
    [Column("subject")]
    [Required]
    [MaxLength(50)]
    public string Subject { get; set; } = "";

    /// <summary>
    /// 题型名称
    /// </summary>
    [Column("type_name")]
    [Required]
    [MaxLength(100)]
    public string TypeName { get; set; } = "";

    /// <summary>
    /// 模板代码（唯一标识，如 fraction_add, word_problem_1）
    /// </summary>
    [Column("template_code")]
    [Required]
    [MaxLength(100)]
    public string TemplateCode { get; set; } = "";

    /// <summary>
    /// 模板内容（使用占位符，如：{name}有{num1}个苹果...）
    /// </summary>
    [Column("template_content")]
    [Required]
    public string TemplateContent { get; set; } = "";

    /// <summary>
    /// 参数配置（JSON格式，定义占位符的类型和范围）
    /// </summary>
    [Column("parameters_config", TypeName = "jsonb")]
    public string ParametersConfig { get; set; } = "{}";

    /// <summary>
    /// 答案表达式（用于自动计算答案）
    /// </summary>
    [Column("answer_expression")]
    public string AnswerExpression { get; set; } = "";

    /// <summary>
    /// 难度级别
    /// </summary>
    [Column("difficulty")]
    [MaxLength(20)]
    public string Difficulty { get; set; } = "Medium";

    /// <summary>
    /// 分类
    /// </summary>
    [Column("category")]
    [MaxLength(100)]
    public string Category { get; set; } = "";

    /// <summary>
    /// 是否启用
    /// </summary>
    [Column("is_active")]
    public bool IsActive { get; set; } = true;

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
