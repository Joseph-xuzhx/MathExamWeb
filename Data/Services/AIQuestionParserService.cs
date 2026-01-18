using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MathExamWeb.Data.Models;

namespace MathExamWeb.Data.Services
{
    /// <summary>
    /// AI题目解析服务（基于Claude）
    /// </summary>
    public class AIQuestionParserService
    {
        private readonly HttpClient _httpClient;
        private const string DEFAULT_API_ENDPOINT = "https://api.anthropic.com/v1/messages";

        public AIQuestionParserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// 使用Claude AI解析题目文本
        /// </summary>
        public async Task<List<Question>> ParseQuestionsWithAI(string text, string apiKey, string model = "claude-3-5-sonnet-20241022", string? apiEndpoint = null)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new List<Question>();

            if (string.IsNullOrWhiteSpace(apiKey))
                throw new Exception("请先配置API Key");

            try
            {
                // 构建提示词
                string prompt = @"你是一个专业的题目解析助手。请将用户输入的题目文本解析为结构化的JSON格式。

要求：
1. 识别每道题目的题号、题目文本、选项（如果有）、正确答案、解析
2. 自动判断题型（选择题或填空题）
3. 如果是选择题但缺少选项，请根据答案推断可能的选项
4. 如果缺少解析，请生成简短的解析说明
5. 返回JSON数组格式，每个元素包含：
   {
     ""text"": ""题目文本"",
     ""type"": ""multiple_choice"" 或 ""fill_blank"",
     ""options"": [""选项1"", ""选项2"", ...],  // 仅选择题有此字段
     ""correctAnswer"": ""正确答案"",
     ""explanation"": ""题目解析""
   }

请直接返回JSON数组，不要添加任何其他文字。

请解析以下题目：

" + text;

                // 构建Claude API请求
                var requestBody = new
                {
                    model = model,
                    max_tokens = 4096,
                    messages = new[]
                    {
                        new { role = "user", content = prompt }
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
                    throw new Exception($"API调用失败：{response.StatusCode} - {responseContent}");
                }

                // 解析Claude响应
                using var doc = JsonDocument.Parse(responseContent);
                var root = doc.RootElement;
                var messageContent = root.GetProperty("content")[0]
                    .GetProperty("text")
                    .GetString();

                if (string.IsNullOrWhiteSpace(messageContent))
                    throw new Exception("AI返回空内容");

                // 清理可能的markdown代码块标记
                messageContent = messageContent.Trim();
                if (messageContent.StartsWith("```json"))
                    messageContent = messageContent.Substring(7);
                if (messageContent.StartsWith("```"))
                    messageContent = messageContent.Substring(3);
                if (messageContent.EndsWith("```"))
                    messageContent = messageContent.Substring(0, messageContent.Length - 3);
                messageContent = messageContent.Trim();

                // 解析JSON为Question列表
                var parsedQuestions = JsonSerializer.Deserialize<List<AIQuestionResponse>>(messageContent);

                if (parsedQuestions == null || parsedQuestions.Count == 0)
                    throw new Exception("AI未能解析出有效题目");

                // 转换为Question对象
                var questions = new List<Question>();
                foreach (var pq in parsedQuestions)
                {
                    var question = new Question
                    {
                        Id = Guid.NewGuid().ToString(),
                        Text = pq.Text,
                        CorrectAnswer = pq.CorrectAnswer,
                        Explanation = pq.Explanation ?? "暂无解析",
                        Category = "AI导入",
                        Difficulty = DifficultyLevel.Medium,
                        CreatedAt = DateTime.Now
                    };

                    if (pq.Type == "multiple_choice")
                    {
                        question.Type = QuestionType.MultipleChoice;
                        question.Options = pq.Options ?? new List<string>();
                    }
                    else
                    {
                        question.Type = QuestionType.FillInBlank;
                    }

                    questions.Add(question);
                }

                return questions;
            }
            catch (Exception ex)
            {
                throw new Exception($"AI解析失败：{ex.Message}", ex);
            }
        }

        /// <summary>
        /// AI返回的题目响应格式
        /// </summary>
        private class AIQuestionResponse
        {
            public string Text { get; set; } = "";
            public string Type { get; set; } = "fill_blank";
            public List<string>? Options { get; set; }
            public string CorrectAnswer { get; set; } = "";
            public string? Explanation { get; set; }
        }
    }
}
