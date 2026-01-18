using MathExamWeb.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MathExamWeb.Data.Services;

/// <summary>
/// 题目模板种子数据服务
/// </summary>
public class QuestionTemplateSeedService
{
    private readonly MathExamDbContext _context;

    public QuestionTemplateSeedService(MathExamDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 初始化题目模板数据
    /// </summary>
    public async Task SeedTemplatesAsync()
    {
        // 检查是否已有数据
        if (await _context.QuestionTemplates.AnyAsync())
        {
            Console.WriteLine("题目模板已存在，跳过初始化");
            return;
        }

        var templates = new List<QuestionTemplateEntity>
        {
            // 数学应用题 - 买卖
            new QuestionTemplateEntity
            {
                Id = "tpl_math_buy_1",
                Subject = "math",
                TypeName = "买卖应用题",
                TemplateCode = "word_problem_buy_simple",
                TemplateContent = "{name}买了{num1}个苹果，每个{price}元，一共花了多少元？",
                ParametersConfig = """{"name": {"type": "name", "values": ["小明", "小红", "小华", "小刚", "小丽"]}, "num1": {"type": "int", "min": 2, "max": 10}, "price": {"type": "decimal", "min": 1, "max": 5}}""",
                AnswerExpression = "{num1} * {price}",
                Difficulty = "Easy",
                Category = "应用题-买卖",
                IsActive = true
            },
            new QuestionTemplateEntity
            {
                Id = "tpl_math_buy_2",
                Subject = "math",
                TypeName = "买卖应用题",
                TemplateCode = "word_problem_buy_medium",
                TemplateContent = "{name}有{money}元，买了{num1}本笔记本，每本{price}元，还剩多少元？",
                ParametersConfig = """{"name": {"type": "name", "values": ["小明", "小红", "小华", "小刚", "小丽"]}, "money": {"type": "int", "min": 20, "max": 100}, "num1": {"type": "int", "min": 2, "max": 8}, "price": {"type": "decimal", "min": 2, "max": 10}}""",
                AnswerExpression = "{money} - ({num1} * {price})",
                Difficulty = "Medium",
                Category = "应用题-买卖",
                IsActive = true
            },

            // 路程应用题
            new QuestionTemplateEntity
            {
                Id = "tpl_math_distance_1",
                Subject = "math",
                TypeName = "路程应用题",
                TemplateCode = "word_problem_distance_simple",
                TemplateContent = "一辆汽车每小时行驶{speed}千米，行驶了{time}小时，一共行驶了多少千米？",
                ParametersConfig = """{"speed": {"type": "int", "min": 40, "max": 100}, "time": {"type": "int", "min": 2, "max": 8}}""",
                AnswerExpression = "{speed} * {time}",
                Difficulty = "Easy",
                Category = "应用题-路程",
                IsActive = true
            },
            new QuestionTemplateEntity
            {
                Id = "tpl_math_distance_2",
                Subject = "math",
                TypeName = "路程应用题",
                TemplateCode = "word_problem_distance_medium",
                TemplateContent = "甲乙两地相距{distance}千米，一辆汽车从甲地开往乙地，每小时行驶{speed}千米，需要多少小时才能到达？",
                ParametersConfig = """{"distance": {"type": "int", "min": 100, "max": 500}, "speed": {"type": "int", "min": 40, "max": 100}}""",
                AnswerExpression = "{distance} / {speed}",
                Difficulty = "Medium",
                Category = "应用题-路程",
                IsActive = true
            },

            // 分数加法
            new QuestionTemplateEntity
            {
                Id = "tpl_math_fraction_add_1",
                Subject = "math",
                TypeName = "分数加法",
                TemplateCode = "fraction_add_same_denominator",
                TemplateContent = "计算：{frac1} + {frac2} = ?",
                ParametersConfig = """{"frac1": {"type": "fraction", "numerator_max": 10, "denominator": "same"}, "frac2": {"type": "fraction", "numerator_max": 10, "denominator": "same"}}""",
                AnswerExpression = "{frac1} + {frac2}",
                Difficulty = "Easy",
                Category = "分数加法-同分母",
                IsActive = true
            },
            new QuestionTemplateEntity
            {
                Id = "tpl_math_fraction_add_2",
                Subject = "math",
                TypeName = "分数加法",
                TemplateCode = "fraction_add_diff_denominator",
                TemplateContent = "计算：{frac1} + {frac2} = ?",
                ParametersConfig = """{"frac1": {"type": "fraction", "numerator_max": 10, "denominator_max": 12}, "frac2": {"type": "fraction", "numerator_max": 10, "denominator_max": 12}}""",
                AnswerExpression = "{frac1} + {frac2}",
                Difficulty = "Medium",
                Category = "分数加法-异分母",
                IsActive = true
            },

            // 分数减法
            new QuestionTemplateEntity
            {
                Id = "tpl_math_fraction_sub_1",
                Subject = "math",
                TypeName = "分数减法",
                TemplateCode = "fraction_sub_same_denominator",
                TemplateContent = "计算：{frac1} - {frac2} = ?",
                ParametersConfig = """{"frac1": {"type": "fraction", "numerator_max": 10, "denominator": "same"}, "frac2": {"type": "fraction", "numerator_max": 10, "denominator": "same"}}""",
                AnswerExpression = "{frac1} - {frac2}",
                Difficulty = "Easy",
                Category = "分数减法-同分母",
                IsActive = true
            },

            // 分数乘法
            new QuestionTemplateEntity
            {
                Id = "tpl_math_fraction_mul_1",
                Subject = "math",
                TypeName = "分数乘法",
                TemplateCode = "fraction_mul_simple",
                TemplateContent = "计算：{frac1} × {num1} = ?",
                ParametersConfig = """{"frac1": {"type": "fraction", "numerator_max": 10, "denominator_max": 12}, "num1": {"type": "int", "min": 2, "max": 10}}""",
                AnswerExpression = "{frac1} * {num1}",
                Difficulty = "Easy",
                Category = "分数乘法",
                IsActive = true
            },

            // 百分数应用题
            new QuestionTemplateEntity
            {
                Id = "tpl_math_percent_1",
                Subject = "math",
                TypeName = "百分数应用题",
                TemplateCode = "percent_discount",
                TemplateContent = "一件衣服原价{price}元，打{discount}折后，现价多少元？",
                ParametersConfig = """{"price": {"type": "int", "min": 100, "max": 500}, "discount": {"type": "decimal", "min": 7, "max": 9.5}}""",
                AnswerExpression = "{price} * ({discount} / 10)",
                Difficulty = "Easy",
                Category = "百分数-折扣",
                IsActive = true
            }
        };

        await _context.QuestionTemplates.AddRangeAsync(templates);
        await _context.SaveChangesAsync();

        Console.WriteLine($"成功初始化 {templates.Count} 个题目模板");
    }
}
