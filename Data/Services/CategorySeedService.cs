using MathExamWeb.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MathExamWeb.Data.Services;

/// <summary>
/// 分类种子数据服务
/// </summary>
public class CategorySeedService
{
    private readonly MathExamDbContext _context;

    public CategorySeedService(MathExamDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 初始化默认分类数据
    /// </summary>
    public async Task SeedCategoriesAsync()
    {
        // 检查是否已有数据
        if (await _context.Categories.AnyAsync())
        {
            Console.WriteLine("分类数据已存在，跳过初始化");
            return;
        }

        var categories = new List<CategoryEntity>
        {
            // 数学分类
            // 基础运算
            new CategoryEntity { Subject = "math", Name = "加法", GroupName = "基础运算", SortOrder = 1 },
            new CategoryEntity { Subject = "math", Name = "减法", GroupName = "基础运算", SortOrder = 2 },
            new CategoryEntity { Subject = "math", Name = "乘法", GroupName = "基础运算", SortOrder = 3 },
            new CategoryEntity { Subject = "math", Name = "除法", GroupName = "基础运算", SortOrder = 4 },
            new CategoryEntity { Subject = "math", Name = "混合运算", GroupName = "基础运算", SortOrder = 5 },

            // 分数运算
            new CategoryEntity { Subject = "math", Name = "分数加法", GroupName = "分数运算", SortOrder = 10 },
            new CategoryEntity { Subject = "math", Name = "分数减法", GroupName = "分数运算", SortOrder = 11 },
            new CategoryEntity { Subject = "math", Name = "分数乘法", GroupName = "分数运算", SortOrder = 12 },
            new CategoryEntity { Subject = "math", Name = "分数除法", GroupName = "分数运算", SortOrder = 13 },

            // 应用题
            new CategoryEntity { Subject = "math", Name = "应用题-买卖", GroupName = "应用题", SortOrder = 20 },
            new CategoryEntity { Subject = "math", Name = "应用题-路程", GroupName = "应用题", SortOrder = 21 },
            new CategoryEntity { Subject = "math", Name = "应用题-工程", GroupName = "应用题", SortOrder = 22 },
            new CategoryEntity { Subject = "math", Name = "应用题-综合", GroupName = "应用题", SortOrder = 23 },

            // 其他
            new CategoryEntity { Subject = "math", Name = "百分数", GroupName = "其他", SortOrder = 30 },
            new CategoryEntity { Subject = "math", Name = "小数", GroupName = "其他", SortOrder = 31 },
            new CategoryEntity { Subject = "math", Name = "几何", GroupName = "其他", SortOrder = 32 },
            new CategoryEntity { Subject = "math", Name = "统计", GroupName = "其他", SortOrder = 33 },
            new CategoryEntity { Subject = "math", Name = "比和比例", GroupName = "其他", SortOrder = 34 },

            // 语文分类
            // 古诗文
            new CategoryEntity { Subject = "chinese", Name = "古诗词", GroupName = "古诗文", SortOrder = 1 },
            new CategoryEntity { Subject = "chinese", Name = "名句填空", GroupName = "古诗文", SortOrder = 2 },
            new CategoryEntity { Subject = "chinese", Name = "文言文", GroupName = "古诗文", SortOrder = 3 },

            // 词汇
            new CategoryEntity { Subject = "chinese", Name = "近义词", GroupName = "词汇", SortOrder = 10 },
            new CategoryEntity { Subject = "chinese", Name = "反义词", GroupName = "词汇", SortOrder = 11 },
            new CategoryEntity { Subject = "chinese", Name = "成语", GroupName = "词汇", SortOrder = 12 },
            new CategoryEntity { Subject = "chinese", Name = "词语辨析", GroupName = "词汇", SortOrder = 13 },

            // 语法
            new CategoryEntity { Subject = "chinese", Name = "修辞手法", GroupName = "语法", SortOrder = 20 },
            new CategoryEntity { Subject = "chinese", Name = "病句修改", GroupName = "语法", SortOrder = 21 },
            new CategoryEntity { Subject = "chinese", Name = "标点符号", GroupName = "语法", SortOrder = 22 },
            new CategoryEntity { Subject = "chinese", Name = "句式变换", GroupName = "语法", SortOrder = 23 },

            // 阅读理解
            new CategoryEntity { Subject = "chinese", Name = "阅读理解", GroupName = "阅读理解", SortOrder = 30 },
            new CategoryEntity { Subject = "chinese", Name = "现代文阅读", GroupName = "阅读理解", SortOrder = 31 }
        };

        await _context.Categories.AddRangeAsync(categories);
        await _context.SaveChangesAsync();

        Console.WriteLine($"成功初始化 {categories.Count} 个分类");
    }
}
