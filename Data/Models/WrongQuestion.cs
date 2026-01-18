using MathExamWeb.Data;

namespace MathExamWeb.Data.Models;

/// <summary>
/// 错题记录模型
/// </summary>
public class WrongQuestion
{
    /// <summary>
    /// 错题记录唯一标识
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// 关联的题目ID
    /// </summary>
    public string QuestionId { get; set; } = "";

    /// <summary>
    /// 题目对象（冗余存储，避免题库变更导致找不到题目）
    /// </summary>
    public Question Question { get; set; } = new Question();

    /// <summary>
    /// 用户的错误答案
    /// </summary>
    public string UserAnswer { get; set; } = "";

    /// <summary>
    /// 最近一次答错的时间
    /// </summary>
    public DateTime WrongAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 累计答错次数
    /// </summary>
    public int WrongCount { get; set; } = 1;

    /// <summary>
    /// 是否已掌握
    /// </summary>
    public bool IsMastered { get; set; } = false;

    /// <summary>
    /// 掌握时间（标记为已掌握的时间）
    /// </summary>
    public DateTime? MasteredAt { get; set; }

    /// <summary>
    /// 科目（"chinese" 或 "math"）
    /// </summary>
    public string Subject { get; set; } = "";
}
