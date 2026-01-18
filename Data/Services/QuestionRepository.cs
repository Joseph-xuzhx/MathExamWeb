using Microsoft.EntityFrameworkCore;

namespace MathExamWeb.Data.Services;

/// <summary>
/// 题目仓储服务 - 负责题目的数据库操作
/// </summary>
public class QuestionRepository
{
    private readonly MathExamDbContext _context;

    public QuestionRepository(MathExamDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 保存题目到数据库
    /// </summary>
    /// <param name="question">题目对象</param>
    public async Task AddAsync(Question question)
    {
        await _context.Questions.AddAsync(question);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// 批量保存题目
    /// </summary>
    /// <param name="questions">题目列表</param>
    public async Task AddRangeAsync(List<Question> questions)
    {
        await _context.Questions.AddRangeAsync(questions);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// 根据ID获取题目
    /// </summary>
    /// <param name="id">题目ID</param>
    public async Task<Question?> GetByIdAsync(string id)
    {
        return await _context.Questions.FindAsync(id);
    }

    /// <summary>
    /// 获取所有题目
    /// </summary>
    public async Task<List<Question>> GetAllAsync()
    {
        return await _context.Questions
            .OrderByDescending(q => q.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// 根据科目获取题目
    /// </summary>
    /// <param name="subject">科目（chinese 或 math）</param>
    public async Task<List<Question>> GetBySubjectAsync(string subject)
    {
        return await _context.Questions
            .Where(q => q.Subject == subject)
            .OrderByDescending(q => q.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// 根据分类获取题目
    /// </summary>
    /// <param name="category">题目分类</param>
    public async Task<List<Question>> GetByCategoryAsync(string category)
    {
        return await _context.Questions
            .Where(q => q.Category == category)
            .OrderByDescending(q => q.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// 更新题目
    /// </summary>
    /// <param name="question">题目对象</param>
    public async Task UpdateAsync(Question question)
    {
        _context.Questions.Update(question);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// 删除题目
    /// </summary>
    /// <param name="id">题目ID</param>
    public async Task DeleteAsync(string id)
    {
        var question = await GetByIdAsync(id);
        if (question != null)
        {
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// 获取题目总数
    /// </summary>
    public async Task<int> GetCountAsync()
    {
        return await _context.Questions.CountAsync();
    }

    /// <summary>
    /// 根据科目获取题目数量
    /// </summary>
    /// <param name="subject">科目</param>
    public async Task<int> GetCountBySubjectAsync(string subject)
    {
        return await _context.Questions
            .Where(q => q.Subject == subject)
            .CountAsync();
    }
}
