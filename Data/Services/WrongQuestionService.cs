using System.Text.Json;
using MathExamWeb.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MathExamWeb.Data.Services;

/// <summary>
/// 错题服务 - 管理错题记录
/// </summary>
public class WrongQuestionService
{
    private readonly MathExamDbContext _context;

    public WrongQuestionService(MathExamDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 添加错题
    /// </summary>
    public async Task AddWrongQuestion(Question question, string userAnswer, string subject)
    {
        // 检查是否已存在（根据科目和题目文本去重）
        var existing = await _context.WrongQuestions
            .FirstOrDefaultAsync(w => w.Subject == subject && w.QuestionText == question.Text);

        if (existing != null)
        {
            // 更新现有记录
            existing.WrongCount++;
            existing.WrongAt = DateTime.UtcNow;
            existing.UserAnswer = userAnswer;
            existing.IsMastered = false; // 重新答错，取消掌握状态
            existing.MasteredAt = null;
            existing.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            // 添加新记录
            var entity = new WrongQuestionEntity
            {
                QuestionId = question.Id,
                Subject = subject,
                QuestionText = question.Text,
                CorrectAnswer = question.CorrectAnswer,
                UserAnswer = userAnswer,
                Explanation = question.Explanation ?? "",
                Category = question.Category ?? "",
                QuestionType = question.Type.ToString(),
                Difficulty = question.Difficulty.ToString(),
                QuestionJson = JsonSerializer.Serialize(question),
                WrongAt = DateTime.UtcNow,
                WrongCount = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _context.WrongQuestions.AddAsync(entity);
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// 标记为已掌握
    /// </summary>
    public async Task MarkAsMastered(string wrongQuestionId)
    {
        var entity = await _context.WrongQuestions.FindAsync(wrongQuestionId);

        if (entity != null)
        {
            entity.IsMastered = true;
            entity.MasteredAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// 删除错题记录
    /// </summary>
    public async Task DeleteWrongQuestion(string wrongQuestionId)
    {
        var entity = await _context.WrongQuestions.FindAsync(wrongQuestionId);
        if (entity != null)
        {
            _context.WrongQuestions.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// 获取所有错题（包括已掌握）
    /// </summary>
    public async Task<List<WrongQuestion>> GetAllWrongQuestions(string? subject = null)
    {
        var query = _context.WrongQuestions.AsQueryable();

        if (!string.IsNullOrEmpty(subject))
        {
            query = query.Where(w => w.Subject == subject);
        }

        var entities = await query
            .OrderByDescending(w => w.WrongAt)
            .ToListAsync();

        return entities.Select(e => new WrongQuestion
        {
            Id = e.Id,
            QuestionId = e.QuestionId,
            Subject = e.Subject,
            UserAnswer = e.UserAnswer,
            WrongAt = e.WrongAt,
            WrongCount = e.WrongCount,
            IsMastered = e.IsMastered,
            MasteredAt = e.MasteredAt,
            Question = JsonSerializer.Deserialize<Question>(e.QuestionJson) ?? new Question()
        }).ToList();
    }

    /// <summary>
    /// 获取未掌握的错题
    /// </summary>
    public async Task<List<WrongQuestion>> GetUnmasteredQuestions(string subject)
    {
        var entities = await _context.WrongQuestions
            .Where(w => w.Subject == subject && !w.IsMastered)
            .OrderByDescending(w => w.WrongCount)
            .ThenByDescending(w => w.WrongAt)
            .ToListAsync();

        return entities.Select(e => new WrongQuestion
        {
            Id = e.Id,
            QuestionId = e.QuestionId,
            Subject = e.Subject,
            UserAnswer = e.UserAnswer,
            WrongAt = e.WrongAt,
            WrongCount = e.WrongCount,
            IsMastered = e.IsMastered,
            MasteredAt = e.MasteredAt,
            Question = JsonSerializer.Deserialize<Question>(e.QuestionJson) ?? new Question()
        }).ToList();
    }

    /// <summary>
    /// 获取已掌握的错题
    /// </summary>
    public async Task<List<WrongQuestion>> GetMasteredQuestions(string subject)
    {
        var entities = await _context.WrongQuestions
            .Where(w => w.Subject == subject && w.IsMastered)
            .OrderByDescending(w => w.MasteredAt)
            .ToListAsync();

        return entities.Select(e => new WrongQuestion
        {
            Id = e.Id,
            QuestionId = e.QuestionId,
            Subject = e.Subject,
            UserAnswer = e.UserAnswer,
            WrongAt = e.WrongAt,
            WrongCount = e.WrongCount,
            IsMastered = e.IsMastered,
            MasteredAt = e.MasteredAt,
            Question = JsonSerializer.Deserialize<Question>(e.QuestionJson) ?? new Question()
        }).ToList();
    }

    /// <summary>
    /// 清除所有错题（危险操作）
    /// </summary>
    public async Task ClearAllWrongQuestions()
    {
        await _context.WrongQuestions.ExecuteDeleteAsync();
    }
}
