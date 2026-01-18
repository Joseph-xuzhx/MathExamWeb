using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MathExamWeb.Data.Models;

/// <summary>
/// 题目分类实体
/// </summary>
[Table("categories")]
public class CategoryEntity
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
    /// 分类名称
    /// </summary>
    [Column("name")]
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = "";

    /// <summary>
    /// 分组名称（用于optgroup显示）
    /// </summary>
    [Column("group_name")]
    [MaxLength(100)]
    public string GroupName { get; set; } = "";

    /// <summary>
    /// 排序序号
    /// </summary>
    [Column("sort_order")]
    public int SortOrder { get; set; } = 0;

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
