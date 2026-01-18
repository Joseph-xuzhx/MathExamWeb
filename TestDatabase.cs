using MathExamWeb.Data;
using MathExamWeb.Data.Services;
using Microsoft.EntityFrameworkCore;

namespace MathExamWeb;

public class DatabaseTester
{
    public static async Task RunTests(string[] args)
    {
        Console.WriteLine("=== 开始测试数据库功能 ===\n");

        var connectionString = "Host=localhost;Port=5432;Database=mathexam;Username=sep229";

        var optionsBuilder = new DbContextOptionsBuilder<MathExamDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        using var context = new MathExamDbContext(optionsBuilder.Options);
        var repo = new QuestionRepository(context);

        // 测试 1: 获取所有题目
        Console.WriteLine("📋 测试 1: 获取所有题目");
        var allQuestions = await repo.GetAllAsync();
        Console.WriteLine($"✅ 总题目数: {allQuestions.Count}");
        Console.WriteLine();

        // 测试 2: 按科目查询
        Console.WriteLine("📚 测试 2: 按科目查询");
        var chineseQuestions = await repo.GetBySubjectAsync("chinese");
        var mathQuestions = await repo.GetBySubjectAsync("math");
        Console.WriteLine($"✅ 语文题目数: {chineseQuestions.Count}");
        Console.WriteLine($"✅ 数学题目数: {mathQuestions.Count}");
        Console.WriteLine();

        // 测试 3: 查看题目详情
        Console.WriteLine("🔍 测试 3: 查看题目详情");
        if (allQuestions.Any())
        {
            var firstQuestion = allQuestions.First();
            Console.WriteLine($"题目ID: {firstQuestion.Id}");
            Console.WriteLine($"科目: {firstQuestion.Subject}");
            Console.WriteLine($"题型: {firstQuestion.Type}");
            Console.WriteLine($"难度: {firstQuestion.Difficulty}");
            Console.WriteLine($"题目: {firstQuestion.Text}");
            Console.WriteLine($"答案: {firstQuestion.CorrectAnswer}");
            if (firstQuestion.Options.Any())
            {
                Console.WriteLine("选项:");
                for (int i = 0; i < firstQuestion.Options.Count; i++)
                {
                    Console.WriteLine($"  {(char)('A' + i)}. {firstQuestion.Options[i]}");
                }
            }
        }
        Console.WriteLine();

        // 测试 4: 统计信息
        Console.WriteLine("📊 测试 4: 统计信息");
        var totalCount = await repo.GetCountAsync();
        var chineseCount = await repo.GetCountBySubjectAsync("chinese");
        var mathCount = await repo.GetCountBySubjectAsync("math");
        Console.WriteLine($"✅ 总题目数: {totalCount}");
        Console.WriteLine($"✅ 语文题数: {chineseCount}");
        Console.WriteLine($"✅ 数学题数: {mathCount}");
        Console.WriteLine();

        // 测试 5: 按分类查询
        Console.WriteLine("🏷️  测试 5: 按分类查询");
        var categories = allQuestions
            .Where(q => !string.IsNullOrEmpty(q.Category))
            .GroupBy(q => q.Category)
            .OrderBy(g => g.Key);

        foreach (var category in categories)
        {
            Console.WriteLine($"✅ {category.Key}: {category.Count()} 题");
        }
        Console.WriteLine();

        Console.WriteLine("=== 所有测试完成！✨ ===");
    }
}
