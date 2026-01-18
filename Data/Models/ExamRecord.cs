using MathExamWeb.Data.Models;

namespace MathExamWeb.Data.Models;

/// <summary>
/// 考试记录模型
/// </summary>
public class ExamRecord
{
    /// <summary>
    /// 记录唯一标识
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// 科目（"chinese" 或 "math"）
    /// </summary>
    public string Subject { get; set; } = "";

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 题目总数
    /// </summary>
    public int TotalQuestions { get; set; }

    /// <summary>
    /// 正确题数
    /// </summary>
    public int CorrectCount { get; set; }

    /// <summary>
    /// 总分
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    /// 正确率（0-1之间）
    /// </summary>
    public double AccuracyRate => TotalQuestions > 0 ? (double)CorrectCount / TotalQuestions : 0;

    /// <summary>
    /// 难度级别（可选，null表示混合难度）
    /// </summary>
    public DifficultyLevel? Difficulty { get; set; }

    /// <summary>
    /// 答题详情列表
    /// </summary>
    public List<AnswerDetail> Answers { get; set; } = new List<AnswerDetail>();
}
