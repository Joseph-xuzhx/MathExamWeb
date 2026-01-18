using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MathExamWeb.Data.Services
{
    /// <summary>
    /// PDF处理服务
    /// </summary>
    public class PDFService
    {
        /// <summary>
        /// 从PDF提取文本
        /// </summary>
        public Task<string> ExtractTextFromPDF(byte[] pdfBytes)
        {
            if (pdfBytes == null || pdfBytes.Length == 0)
                throw new Exception("PDF数据为空");

            try
            {
                // 方案1：使用iTextSharp提取文本（需要NuGet包）
                // 由于需要添加依赖，这里先返回提示

                // 方案2：将PDF转为图片，然后使用OCR
                // 这需要额外的库支持

                return Task.FromResult("PDF文本提取功能开发中。\n\n建议：\n1. 将PDF导出为图片后上传\n2. 或复制PDF中的文字直接粘贴到文本框");
            }
            catch (Exception ex)
            {
                throw new Exception($"PDF处理失败：{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 检查是否为有效的PDF文件
        /// </summary>
        public bool IsValidPDF(byte[] fileBytes)
        {
            if (fileBytes == null || fileBytes.Length < 5)
                return false;

            // PDF文件以 %PDF- 开头
            var header = Encoding.ASCII.GetString(fileBytes, 0, Math.Min(5, fileBytes.Length));
            return header.StartsWith("%PDF-");
        }
    }
}
