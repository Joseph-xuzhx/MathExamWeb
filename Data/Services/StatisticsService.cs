using System.Text.Json;
using MathExamWeb.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MathExamWeb.Data.Services;

/// <summary>
/// 统计服务 - 管理考试记录和统计分析
/// </summary>
public class StatisticsService
{
    private readonly MathExamDbContext _context;

    public StatisticsService(MathExamDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 保存考试记录
    /// </summary>
    public async Task SaveExamRecord(ExamRecord record)
    {
        // 转换为实体对象
        var entity = new ExamRecordEntity
        {
            Id = record.Id,
            Subject = record.Subject,
            StartTime = record.StartTime,
            EndTime = record.EndTime,
            Score = record.Score,
            TotalQuestions = record.TotalQuestions,
            CorrectCount = record.CorrectCount,
            AccuracyRate = record.AccuracyRate,
            Difficulty = record.Difficulty?.ToString(),
            AnswersJson = JsonSerializer.Serialize(record.Answers),
            CreatedAt = DateTime.UtcNow
        };

        await _context.ExamRecords.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// 获取所有考试记录
    /// </summary>
    public async Task<List<ExamRecord>> GetAllRecords()
    {
        var entities = await _context.ExamRecords
            .OrderByDescending(r => r.EndTime)
            .ToListAsync();

        return entities.Select(e => new ExamRecord
        {
            Id = e.Id,
            Subject = e.Subject,
            StartTime = e.StartTime,
            EndTime = e.EndTime,
            Score = e.Score,
            TotalQuestions = e.TotalQuestions,
            CorrectCount = e.CorrectCount,
            Difficulty = string.IsNullOrEmpty(e.Difficulty) ? null : Enum.Parse<DifficultyLevel>(e.Difficulty),
            Answers = JsonSerializer.Deserialize<List<AnswerDetail>>(e.AnswersJson) ?? new List<AnswerDetail>()
        }).ToList();
    }

    /// <summary>
    /// 获取指定科目的统计摘要
    /// </summary>
    public async Task<StatisticsSummary> GetSummary(string subject)
    {
        var allRecords = await GetAllRecords();
        var records = allRecords.Where(r => r.Subject == subject).ToList();

        if (!records.Any())
        {
            return new StatisticsSummary
            {
                Subject = subject,
                TotalExams = 0
            };
        }

        var summary = new StatisticsSummary
        {
            Subject = subject,
            TotalExams = records.Count,
            TotalQuestions = records.Sum(r => r.TotalQuestions),
            AverageScore = records.Average(r => r.Score),
            AverageAccuracy = records.Average(r => r.AccuracyRate),
            BestScore = records.Max(r => r.Score),
            RecentRecords = records.OrderByDescending(r => r.EndTime).Take(5).ToList(),
            WeakCategories = GetWeakCategories(records)
        };

        return summary;
    }

    /// <summary>
    /// 分析薄弱题型（正确率<60%）
    /// </summary>
    private Dictionary<string, double> GetWeakCategories(List<ExamRecord> records)
    {
        var allAnswers = records.SelectMany(r => r.Answers).ToList();
        if (!allAnswers.Any())
        {
            return new Dictionary<string, double>();
        }

        var categoryStats = allAnswers
            .GroupBy(a => a.Category)
            .Select(g => new
            {
                Category = g.Key,
                Total = g.Count(),
                Correct = g.Count(a => a.IsCorrect),
                Accuracy = (double)g.Count(a => a.IsCorrect) / g.Count()
            })
            .Where(x => x.Accuracy < 0.6 && x.Total >= 3) // 至少答过3题且正确率<60%
            .OrderBy(x => x.Accuracy)
            .ToDictionary(x => x.Category, x => x.Accuracy);

        return categoryStats;
    }

    /// <summary>
    /// 清除所有记录（危险操作）
    /// </summary>
    public async Task ClearAllRecords()
    {
        await _context.ExamRecords.ExecuteDeleteAsync();
    }
}

/// <summary>
/// 统计摘要模型
/// </summary>
public class StatisticsSummary
{
    /// <summary>
    /// 科目
    /// </summary>
    public string Subject { get; set; } = "";

    /// <summary>
    /// 总测试次数
    /// </summary>
    public int TotalExams { get; set; }

    /// <summary>
    /// 累计答题数
    /// </summary>
    public int TotalQuestions { get; set; }

    /// <summary>
    /// 平均分
    /// </summary>
    public double AverageScore { get; set; }

    /// <summary>
    /// 平均正确率
    /// </summary>
    public double AverageAccuracy { get; set; }

    /// <summary>
    /// 最高分
    /// </summary>
    public int BestScore { get; set; }

    /// <summary>
    /// 薄弱题型（题型名称 → 正确率）
    /// </summary>
    public Dictionary<string, double> WeakCategories { get; set; } = new Dictionary<string, double>();

    /// <summary>
    /// 最近5次记录
    /// </summary>
    public List<ExamRecord> RecentRecords { get; set; } = new List<ExamRecord>();
}
