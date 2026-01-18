using MathExamWeb.Data.Models;

namespace MathExamWeb.Data;

/// <summary>
/// 题目类型枚举
/// </summary>
public enum QuestionType
{
    /// <summary>
    /// 填空题（需要输入答案）
    /// </summary>
    FillInBlank = 0,

    /// <summary>
    /// 单选题（从选项中选择一个）
    /// </summary>
    MultipleChoice = 1
}

/// <summary>
/// 题目数据模型
/// </summary>
public class Question
{
    /// <summary>
    /// 题目唯一标识
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// 题目类型（填空题/选择题）
    /// </summary>
    public QuestionType Type { get; set; } = QuestionType.FillInBlank;

    /// <summary>
    /// 题目文本
    /// </summary>
    public string Text { get; set; } = "";

    /// <summary>
    /// 正确答案
    /// </summary>
    public string CorrectAnswer { get; set; } = "";

    /// <summary>
    /// 选择题选项列表（仅选择题使用，包含正确答案）
    /// </summary>
    public List<string> Options { get; set; } = new List<string>();

    /// <summary>
    /// 题目解析
    /// </summary>
    public string Explanation { get; set; } = "";

    /// <summary>
    /// 是否为分数答案（数学题目专用）
    /// </summary>
    public bool IsFractionAnswer { get; set; }

    /// <summary>
    /// 题目难度级别
    /// </summary>
    public DifficultyLevel Difficulty { get; set; } = DifficultyLevel.Medium;

    /// <summary>
    /// 题目分类（如"分数运算"、"名句填空"等）
    /// </summary>
    public string Category { get; set; } = "";

    /// <summary>
    /// 科目（chinese-语文，math-数学）
    /// </summary>
    public string Subject { get; set; } = "chinese";

    /// <summary>
    /// 题目标签（用于更细粒度的分类）
    /// </summary>
    public List<string> Tags { get; set; } = new List<string>();

    /// <summary>
    /// 题目创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 打乱后的选项列表（用于考试显示，不持久化到数据库）
    /// </summary>
    public List<string> ShuffledOptions { get; set; } = new List<string>();

    /// <summary>
    /// 打乱后的正确答案字母（如 A、B、C、D，不持久化到数据库）
    /// </summary>
    public string ShuffledCorrectAnswerLetter { get; set; } = "";

    /// <summary>
    /// 对选择题选项进行随机排序，并更新正确答案字母
    /// </summary>
    public void ShuffleOptions()
    {
        if (Type != QuestionType.MultipleChoice || !Options.Any())
        {
            return;
        }

        // 创建副本并打乱
        var random = new Random();
        ShuffledOptions = Options.OrderBy(x => random.Next()).ToList();

        // 找到正确答案在新顺序中的位置
        int correctIndex = ShuffledOptions.FindIndex(opt => opt.Trim() == CorrectAnswer.Trim());

        if (correctIndex >= 0 && correctIndex < 26)
        {
            ShuffledCorrectAnswerLetter = ((char)('A' + correctIndex)).ToString();
        }
        else
        {
            // 如果没找到，使用原始顺序
            ShuffledOptions = new List<string>(Options);
            ShuffledCorrectAnswerLetter = "A";
        }
    }
}
