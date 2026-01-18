namespace MathExamWeb.Data.Models;

/// <summary>
/// 答题详情模型
/// </summary>
public class AnswerDetail
{
    /// <summary>
    /// 题目ID
    /// </summary>
    public string QuestionId { get; set; } = "";

    /// <summary>
    /// 题目文本
    /// </summary>
    public string QuestionText { get; set; } = "";

    /// <summary>
    /// 正确答案
    /// </summary>
    public string CorrectAnswer { get; set; } = "";

    /// <summary>
    /// 用户答案
    /// </summary>
    public string UserAnswer { get; set; } = "";

    /// <summary>
    /// 是否正确
    /// </summary>
    public bool IsCorrect { get; set; }

    /// <summary>
    /// 题目分类
    /// </summary>
    public string Category { get; set; } = "";

    /// <summary>
    /// 答题时间
    /// </summary>
    public DateTime AnsweredAt { get; set; } = DateTime.Now;
}
