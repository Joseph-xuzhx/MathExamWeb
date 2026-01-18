using MathExamWeb.Data.Models;

namespace MathExamWeb.Data.Services.Interfaces;

/// <summary>
/// 题目服务接口
/// </summary>
public interface IQuestionService
{
    /// <summary>
    /// 生成一道题目
    /// </summary>
    /// <param name="difficulty">难度级别（可选，null表示随机难度）</param>
    /// <returns>生成的题目</returns>
    Question GenerateQuestion(DifficultyLevel? difficulty = null);

    /// <summary>
    /// 根据难度获取题目列表（用于固定题库的服务）
    /// </summary>
    /// <param name="difficulty">难度级别</param>
    /// <returns>指定难度的题目列表</returns>
    List<Question> GetQuestionsByDifficulty(DifficultyLevel difficulty);

    /// <summary>
    /// 获取所有题型分类
    /// </summary>
    /// <returns>题型分类列表</returns>
    List<string> GetCategories();

    /// <summary>
    /// 获取各题型的题目数量统计
    /// </summary>
    /// <returns>题型-数量字典</returns>
    Dictionary<string, int> GetCategoryStatistics();
}
