using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MathExamWeb.Data.Services
{
    /// <summary>
    /// OCR识别服务（使用Claude Vision API）
    /// </summary>
    public class OCRService
    {
        private readonly HttpClient _httpClient;
        private const string DEFAULT_API_ENDPOINT = "https://api.anthropic.com/v1/messages";

        public OCRService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// 使用Claude Vision识别图片中的文字
        /// </summary>
        public async Task<string> RecognizeImageText(byte[] imageBytes, string apiKey, string imageFormat = "png", string? apiEndpoint = null)
        {
            if (imageBytes == null || imageBytes.Length == 0)
                throw new Exception("图片数据为空");

            if (string.IsNullOrWhiteSpace(apiKey))
                throw new Exception("请先配置API Key");

            try
            {
                // 将图片转为base64
                string base64Image = Convert.ToBase64String(imageBytes);

                // 确定正确的媒体类型
                string mediaType = imageFormat.ToLower() switch
                {
                    "jpg" => "image/jpeg",
                    "jpeg" => "image/jpeg",
                    "png" => "image/png",
                    "gif" => "image/gif",
                    "webp" => "image/webp",
                    _ => "image/png"
                };

                // 构建Claude API请求
                var requestBody = new
                {
                    model = "claude-3-5-sonnet-20241022", // 使用Claude 3.5 Sonnet
                    max_tokens = 4096,
                    messages = new object[]
                    {
                        new
                        {
                            role = "user",
                            content = new object[]
                            {
                                new
                                {
                                    type = "image",
                                    source = new
                                    {
                                        type = "base64",
                                        media_type = mediaType,
                                        data = base64Image
                                    }
                                },
                                new
                                {
                                    type = "text",
                                    text = "请识别图片中的所有文字内容，按原格式输出。如果是题目，请尽量保持题号、选项、答案等格式。直接输出识别的文字，不要添加任何解释。"
                                }
                            }
                        }
                    }
                };

                var jsonContent = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
                _httpClient.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
                // 模拟 Claude Code CLI 的 User-Agent
                if (!_httpClient.DefaultRequestHeaders.Contains("User-Agent"))
                {
                    _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "claude-code/2.0.32");
                }

                // 使用自定义API端点或默认端点
                string endpoint = string.IsNullOrWhiteSpace(apiEndpoint) ? DEFAULT_API_ENDPOINT : apiEndpoint;

                // 发送请求
                var response = await _httpClient.PostAsync(endpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"OCR识别失败：{response.StatusCode} - {responseContent}");
                }

                // 解析Claude响应
                using var doc = JsonDocument.Parse(responseContent);
                var root = doc.RootElement;
                var messageContent = root.GetProperty("content")[0]
                    .GetProperty("text")
                    .GetString();

                if (string.IsNullOrWhiteSpace(messageContent))
                    throw new Exception("OCR返回空内容");

                return messageContent.Trim();
            }
            catch (Exception ex)
            {
                throw new Exception($"OCR识别失败：{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 使用免费OCR API识别图片（备用方案）
        /// </summary>
        public Task<string> RecognizeImageWithFreeOCR(byte[] imageBytes)
        {
            // 可以集成免费的OCR服务，如OCR.space
            // 这里先返回提示信息
            return Task.FromResult("免费OCR功能开发中，请使用AI识别模式（需要API Key）");
        }
    }
}
