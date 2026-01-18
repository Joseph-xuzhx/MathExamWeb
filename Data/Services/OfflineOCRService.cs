using System;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text;

namespace MathExamWeb.Data.Services
{
    /// <summary>
    /// 离线OCR识别服务（基于Tesseract命令行）
    /// </summary>
    public class OfflineOCRService
    {
        private readonly string _tempPath;

        public OfflineOCRService()
        {
            // 使用系统临时目录
            _tempPath = Path.Combine(Path.GetTempPath(), "MathExamOCR");
            if (!Directory.Exists(_tempPath))
            {
                Directory.CreateDirectory(_tempPath);
            }
        }

        /// <summary>
        /// 识别图片中的文字（中文+英文）
        /// </summary>
        public async Task<string> RecognizeImageText(byte[] imageBytes, string language = "chi_sim+eng")
        {
            if (imageBytes == null || imageBytes.Length == 0)
                throw new Exception("图片数据为空");

            string? tempImagePath = null;
            string? tempOutputPath = null;

            try
            {
                // 创建临时文件
                tempImagePath = Path.Combine(_tempPath, $"{Guid.NewGuid()}.png");
                tempOutputPath = Path.Combine(_tempPath, $"{Guid.NewGuid()}");

                // 保存图片到临时文件
                await File.WriteAllBytesAsync(tempImagePath, imageBytes);

                // 调用tesseract命令行
                var startInfo = new ProcessStartInfo
                {
                    FileName = "tesseract",
                    Arguments = $"\"{tempImagePath}\" \"{tempOutputPath}\" -l {language}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(startInfo))
                {
                    if (process == null)
                        throw new Exception("无法启动Tesseract进程");

                    await process.WaitForExitAsync();

                    if (process.ExitCode != 0)
                    {
                        var error = await process.StandardError.ReadToEndAsync();
                        throw new Exception($"Tesseract执行失败: {error}");
                    }
                }

                // 读取识别结果
                string outputFile = tempOutputPath + ".txt";
                if (File.Exists(outputFile))
                {
                    var text = await File.ReadAllTextAsync(outputFile);
                    return text.Trim();
                }
                else
                {
                    throw new Exception("识别结果文件不存在");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"OCR识别失败：{ex.Message}", ex);
            }
            finally
            {
                // 清理临时文件
                try
                {
                    if (tempImagePath != null && File.Exists(tempImagePath))
                        File.Delete(tempImagePath);

                    if (tempOutputPath != null && File.Exists(tempOutputPath + ".txt"))
                        File.Delete(tempOutputPath + ".txt");
                }
                catch { }
            }
        }

        /// <summary>
        /// 识别图片中的文字（仅中文）
        /// </summary>
        public async Task<string> RecognizeChineseText(byte[] imageBytes)
        {
            return await RecognizeImageText(imageBytes, "chi_sim");
        }

        /// <summary>
        /// 识别图片中的文字（仅英文）
        /// </summary>
        public async Task<string> RecognizeEnglishText(byte[] imageBytes)
        {
            return await RecognizeImageText(imageBytes, "eng");
        }

        /// <summary>
        /// 获取可用的语言列表
        /// </summary>
        public string[] GetAvailableLanguages()
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "tesseract",
                    Arguments = "--list-langs",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(startInfo))
                {
                    if (process == null)
                        return new[] { "chi_sim", "eng" };

                    process.WaitForExit();
                    var output = process.StandardOutput.ReadToEnd();

                    // 解析输出获取语言列表
                    var lines = output.Split('\n');
                    var languages = new List<string>();

                    foreach (var line in lines)
                    {
                        if (line.Contains("List of available languages"))
                            continue;

                        if (!string.IsNullOrWhiteSpace(line) &&
                            !line.StartsWith("Error") &&
                            !line.StartsWith("Tesseract"))
                        {
                            languages.Add(line.Trim());
                        }
                    }

                    return languages.Count > 0 ? languages.ToArray() : new[] { "chi_sim", "eng" };
                }
            }
            catch
            {
                // 如果无法获取，返回基本的中英文
                return new[] { "chi_sim", "eng" };
            }
        }

        /// <summary>
        /// 检查服务是否可用
        /// </summary>
        public bool IsAvailable()
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "tesseract",
                    Arguments = "--version",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(startInfo))
                {
                    if (process == null) return false;
                    process.WaitForExit(3000);
                    return process.ExitCode == 0;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}

