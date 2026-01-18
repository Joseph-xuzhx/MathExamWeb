using ClosedXML.Excel;
using MathExamWeb.Data.Models;

namespace MathExamWeb.Data.Services;

/// <summary>
/// Excel 导出服务
/// </summary>
public class ExcelExportService
{
    /// <summary>
    /// 导出题目列表到 Excel
    /// </summary>
    /// <param name="questions">题目列表</param>
    /// <param name="fileName">文件名（不含扩展名）</param>
    /// <returns>Excel 文件字节数组</returns>
    public byte[] ExportQuestionsToExcel(List<Question> questions, string fileName = "题库导出")
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("题目列表");

        // 设置表头
        var headers = new[]
        {
            "序号", "科目", "题型", "难度", "分类",
            "题目内容", "选项A", "选项B", "选项C", "选项D",
            "正确答案", "题目解析", "创建时间"
        };

        for (int i = 0; i < headers.Length; i++)
        {
            var cell = worksheet.Cell(1, i + 1);
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.LightBlue;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        }

        // 填充数据
        int row = 2;
        foreach (var question in questions)
        {
            worksheet.Cell(row, 1).Value = row - 1; // 序号
            worksheet.Cell(row, 2).Value = question.Subject == "chinese" ? "语文" : "数学";
            worksheet.Cell(row, 3).Value = question.Type == QuestionType.MultipleChoice ? "选择题" : "填空题";
            worksheet.Cell(row, 4).Value = GetDifficultyText(question.Difficulty);
            worksheet.Cell(row, 5).Value = question.Category;
            worksheet.Cell(row, 6).Value = question.Text;

            // 选项（如果是选择题）
            if (question.Type == QuestionType.MultipleChoice)
            {
                for (int i = 0; i < Math.Min(question.Options.Count, 4); i++)
                {
                    worksheet.Cell(row, 7 + i).Value = question.Options[i];
                }
            }

            worksheet.Cell(row, 11).Value = question.CorrectAnswer;
            worksheet.Cell(row, 12).Value = question.Explanation;
            worksheet.Cell(row, 13).Value = question.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss");

            row++;
        }

        // 自动调整列宽
        worksheet.Columns().AdjustToContents();

        // 设置题目内容和解析列的最大宽度
        worksheet.Column(6).Width = 50; // 题目内容
        worksheet.Column(12).Width = 40; // 题目解析

        // 设置文本自动换行
        worksheet.Column(6).Style.Alignment.WrapText = true;
        worksheet.Column(12).Style.Alignment.WrapText = true;

        // 冻结首行
        worksheet.SheetView.FreezeRows(1);

        // 添加筛选
        worksheet.RangeUsed()?.SetAutoFilter();

        // 保存到内存流
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    /// <summary>
    /// 按科目分组导出题目到 Excel
    /// </summary>
    /// <param name="questions">题目列表</param>
    /// <param name="fileName">文件名（不含扩展名）</param>
    /// <returns>Excel 文件字节数组</returns>
    public byte[] ExportQuestionsBySubjectToExcel(List<Question> questions, string fileName = "题库分类导出")
    {
        using var workbook = new XLWorkbook();

        // 按科目分组
        var subjects = questions.GroupBy(q => q.Subject).OrderBy(g => g.Key);

        foreach (var subjectGroup in subjects)
        {
            var subjectName = subjectGroup.Key == "chinese" ? "语文" : "数学";
            var worksheet = workbook.Worksheets.Add(subjectName);

            // 设置表头（同上）
            var headers = new[]
            {
                "序号", "题型", "难度", "分类",
                "题目内容", "选项A", "选项B", "选项C", "选项D",
                "正确答案", "题目解析", "创建时间"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                var cell = worksheet.Cell(1, i + 1);
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.LightBlue;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }

            // 填充数据
            int row = 2;
            foreach (var question in subjectGroup)
            {
                worksheet.Cell(row, 1).Value = row - 1;
                worksheet.Cell(row, 2).Value = question.Type == QuestionType.MultipleChoice ? "选择题" : "填空题";
                worksheet.Cell(row, 3).Value = GetDifficultyText(question.Difficulty);
                worksheet.Cell(row, 4).Value = question.Category;
                worksheet.Cell(row, 5).Value = question.Text;

                if (question.Type == QuestionType.MultipleChoice)
                {
                    for (int i = 0; i < Math.Min(question.Options.Count, 4); i++)
                    {
                        worksheet.Cell(row, 6 + i).Value = question.Options[i];
                    }
                }

                worksheet.Cell(row, 10).Value = question.CorrectAnswer;
                worksheet.Cell(row, 11).Value = question.Explanation;
                worksheet.Cell(row, 12).Value = question.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss");

                row++;
            }

            // 自动调整列宽
            worksheet.Columns().AdjustToContents();
            worksheet.Column(5).Width = 50;
            worksheet.Column(11).Width = 40;
            worksheet.Column(5).Style.Alignment.WrapText = true;
            worksheet.Column(11).Style.Alignment.WrapText = true;

            worksheet.SheetView.FreezeRows(1);
            worksheet.RangeUsed()?.SetAutoFilter();
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    private string GetDifficultyText(DifficultyLevel difficulty)
    {
        return difficulty switch
        {
            DifficultyLevel.Easy => "简单",
            DifficultyLevel.Medium => "中等",
            DifficultyLevel.Hard => "困难",
            _ => "未知"
        };
    }
}
