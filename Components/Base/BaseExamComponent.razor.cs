using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MathExamWeb.Data;
using MathExamWeb.Data.Models;
using MathExamWeb.Data.Services.Interfaces;
using System.Text.Json;

namespace MathExamWeb.Components.Base;

/// <summary>
/// 考试组件基类
/// </summary>
/// <typeparam name="TService">实现了 IQuestionService 接口的题目服务类型</typeparam>
public abstract partial class BaseExamComponent<TService> : ComponentBase
    where TService : IQuestionService
{
    #region 依赖注入

    [Inject] protected TService QuestionService { get; set; } = default!;
    [Inject] protected LocalStorageService Storage { get; set; } = default!;
    [Inject] protected MathExamWeb.Data.Services.StatisticsService StatisticsService { get; set; } = default!;
    [Inject] protected MathExamWeb.Data.Services.WrongQuestionService WrongQuestionService { get; set; } = default!;

    #endregion

    #region 抽象方法（子类必须实现）

    /// <summary>
    /// 验证用户答案是否正确
    /// </summary>
    /// <param name="userAnswer">用户输入的答案</param>
    /// <param name="question">当前题目</param>
    /// <returns>是否正确</returns>
    protected abstract bool ValidateAnswer(string userAnswer, Question question);

    /// <summary>
    /// 获取 LocalStorage 存储键前缀
    /// </summary>
    /// <returns>存储键前缀（如 "chinese" 或 "math"）</returns>
    protected abstract string GetStoragePrefix();

    /// <summary>
    /// 获取科目名称
    /// </summary>
    /// <returns>科目名称（如 "语文" 或 "数学"）</returns>
    protected abstract string GetSubjectName();

    #endregion

    #region 状态变量

    /// <summary>
    /// 考试模式
    /// </summary>
    protected ExamMode SelectedExamMode = ExamMode.Sequential;

    /// <summary>
    /// 当前题目
    /// </summary>
    protected Question? CurrentQuestion;

    /// <summary>
    /// 用户输入的答案
    /// </summary>
    protected string UserAnswer { get; set; } = "";

    /// <summary>
    /// 是否已提交答案
    /// </summary>
    protected bool IsSubmitted = false;

    /// <summary>
    /// 当前答案是否正确
    /// </summary>
    protected bool IsCorrect = false;

    /// <summary>
    /// 当前得分
    /// </summary>
    protected int Score = 0;

    /// <summary>
    /// 目标题目数量
    /// </summary>
    protected int TargetCount = 10;

    /// <summary>
    /// 选择的难度级别（null 表示随机难度）
    /// </summary>
    protected DifficultyLevel? SelectedDifficulty = null;

    /// <summary>
    /// 是否已开始测试
    /// </summary>
    protected bool Started = false;

    /// <summary>
    /// 是否已完成测试
    /// </summary>
    protected bool Finished = false;

    /// <summary>
    /// 试卷模式：所有题目列表
    /// </summary>
    protected List<Question> AllQuestions = new List<Question>();

    /// <summary>
    /// 试卷模式：用户答案字典（题目索引 -> 答案）
    /// </summary>
    protected Dictionary<int, string> PaperAnswers = new Dictionary<int, string>();

    /// <summary>
    /// 试卷模式：是否已提交试卷
    /// </summary>
    protected bool PaperSubmitted = false;

    /// <summary>
    /// 当前会话已见过的题目文本集合（用于去重）
    /// </summary>
    protected HashSet<string> Seen = new HashSet<string>();

    /// <summary>
    /// 当前会话已答题目列表
    /// </summary>
    protected List<AskedItem> Asked = new List<AskedItem>();

    /// <summary>
    /// 是否显示复习列表
    /// </summary>
    protected bool ShowReview = false;

    /// <summary>
    /// 持久化的已答题目列表（从 LocalStorage 加载）
    /// </summary>
    protected List<AskedItem> PersistedAsked = new List<AskedItem>();

    /// <summary>
    /// 持久化的已见题目文本集合（从 LocalStorage 加载）
    /// </summary>
    protected HashSet<string> PersistedSeen = new HashSet<string>();

    /// <summary>
    /// 当前考试记录
    /// </summary>
    private ExamRecord? CurrentExamRecord;

    #endregion

    #region 生命周期方法

    /// <summary>
    /// 标记是否已加载数据
    /// </summary>
    private bool _dataLoaded = false;

    /// <summary>
    /// 组件初始化
    /// </summary>
    protected override void OnInitialized()
    {
        Started = false;
    }

    /// <summary>
    /// 渲染后加载 LocalStorage 数据（确保 JavaScript 已准备好）
    /// </summary>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && !_dataLoaded)
        {
            _dataLoaded = true;
            string prefix = GetStoragePrefix();

            try
            {
                var askedJson = await Storage.GetAsync($"asked_{prefix}_items");
                var seenJson = await Storage.GetAsync($"seen_{prefix}_texts");

                if (!string.IsNullOrWhiteSpace(askedJson))
                {
                    try
                    {
                        PersistedAsked = JsonSerializer.Deserialize<List<AskedItem>>(askedJson) ?? new List<AskedItem>();
                    }
                    catch
                    {
                        PersistedAsked = new List<AskedItem>();
                    }
                }

                if (!string.IsNullOrWhiteSpace(seenJson))
                {
                    try
                    {
                        PersistedSeen = JsonSerializer.Deserialize<HashSet<string>>(seenJson) ?? new HashSet<string>();
                    }
                    catch
                    {
                        PersistedSeen = new HashSet<string>();
                    }
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载数据失败: {ex.Message}");
            }
        }
    }

    #endregion

    #region 核心业务方法

    /// <summary>
    /// 开始测试
    /// </summary>
    protected void StartExam()
    {
        Score = 0;
        Started = true;
        Finished = false;
        ShowReview = false;
        Asked.Clear();
        Seen.Clear();
        PaperSubmitted = false;

        // 初始化考试记录
        CurrentExamRecord = new ExamRecord
        {
            Subject = GetStoragePrefix(),
            StartTime = DateTime.UtcNow,
            TotalQuestions = TargetCount,
            Difficulty = SelectedDifficulty
        };

        if (SelectedExamMode == ExamMode.Paper)
        {
            // 试卷模式：一次性生成所有题目
            GenerateAllQuestions();
        }
        else
        {
            // 逐题模式：生成第一题
            NextQuestion();
        }
    }

    /// <summary>
    /// 生成下一题
    /// </summary>
    protected void NextQuestion()
    {
        if (Asked.Count >= TargetCount)
        {
            Finished = true;
            CurrentQuestion = null;
            return;
        }

        int attempts = 0;
        Question q;
        do
        {
            q = QuestionService.GenerateQuestion(SelectedDifficulty);
            attempts++;
            if (attempts > 100) break; // 防止无限循环
        } while (Seen.Contains(q.Text) || PersistedSeen.Contains(q.Text));

        // 对选择题进行选项随机排序
        q.ShuffleOptions();

        CurrentQuestion = q;
        Seen.Add(q.Text);
        UserAnswer = "";
        IsSubmitted = false;
        IsCorrect = false;
    }

    /// <summary>
    /// 提交答案
    /// </summary>
    protected async Task SubmitAnswer()
    {
        if (IsSubmitted || CurrentQuestion == null) return;

        // 调用子类实现的验证逻辑
        IsCorrect = ValidateAnswer(UserAnswer, CurrentQuestion);

        if (IsCorrect)
        {
            Score += 10;
        }
        else
        {
            // 保存到错题本
            try
            {
                await WrongQuestionService.AddWrongQuestion(
                    CurrentQuestion,
                    UserAnswer.Trim(),
                    GetStoragePrefix()
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存错题失败: {ex.Message}");
            }
        }

        // 记录答题详情
        var item = new AskedItem
        {
            Q = CurrentQuestion,
            Correct = IsCorrect,
            User = UserAnswer.Trim()
        };

        Asked.Add(item);
        PersistedAsked.Add(item);
        PersistedSeen.Add(CurrentQuestion.Text);

        // 保存到考试记录
        if (CurrentExamRecord != null)
        {
            CurrentExamRecord.Answers.Add(new AnswerDetail
            {
                QuestionId = CurrentQuestion.Id,
                QuestionText = CurrentQuestion.Text,
                CorrectAnswer = CurrentQuestion.CorrectAnswer,
                UserAnswer = UserAnswer.Trim(),
                IsCorrect = IsCorrect,
                Category = CurrentQuestion.Category,
                AnsweredAt = DateTime.UtcNow
            });
        }

        // 保存到 LocalStorage
        await SaveProgress();

        IsSubmitted = true;
    }

    /// <summary>
    /// 保存进度到 LocalStorage
    /// </summary>
    protected async Task SaveProgress()
    {
        string prefix = GetStoragePrefix();
        var askedJson = JsonSerializer.Serialize(PersistedAsked);
        var seenJson = JsonSerializer.Serialize(PersistedSeen);

        await Storage.SetAsync($"asked_{prefix}_items", askedJson);
        await Storage.SetAsync($"seen_{prefix}_texts", seenJson);
    }

    /// <summary>
    /// 处理键盘按键事件（支持回车提交）
    /// </summary>
    protected async Task HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter" && !string.IsNullOrWhiteSpace(UserAnswer) && !IsSubmitted)
        {
            await SubmitAnswer();
        }
    }

    /// <summary>
    /// 切换复习列表显示状态
    /// </summary>
    protected void ToggleReview()
    {
        ShowReview = !ShowReview;
    }

    /// <summary>
    /// 完成测试
    /// </summary>
    protected async Task CompleteExam()
    {
        Finished = true;
        CurrentQuestion = null;

        // 保存考试记录到统计服务
        if (CurrentExamRecord != null)
        {
            CurrentExamRecord.EndTime = DateTime.UtcNow;
            CurrentExamRecord.Score = Score;
            CurrentExamRecord.CorrectCount = Asked.Count(a => a.Correct);

            try
            {
                await StatisticsService.SaveExamRecord(CurrentExamRecord);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存考试记录失败: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// 重置到开始状态
    /// </summary>
    protected void ResetToStart()
    {
        Started = false;
        Finished = false;
        CurrentQuestion = null;
        AllQuestions.Clear();
        PaperAnswers.Clear();
        PaperSubmitted = false;
    }

    /// <summary>
    /// 试卷模式：生成所有题目
    /// </summary>
    protected void GenerateAllQuestions()
    {
        AllQuestions.Clear();
        PaperAnswers.Clear();

        HashSet<string> sessionSeen = new HashSet<string>();
        int attempts = 0;
        int maxAttempts = TargetCount * 50; // 防止无限循环

        while (AllQuestions.Count < TargetCount && attempts < maxAttempts)
        {
            attempts++;
            Question q = QuestionService.GenerateQuestion(SelectedDifficulty);

            // 确保题目不重复
            if (!sessionSeen.Contains(q.Text) && !PersistedSeen.Contains(q.Text))
            {
                // 对选择题进行选项随机排序
                q.ShuffleOptions();

                AllQuestions.Add(q);
                sessionSeen.Add(q.Text);
                Seen.Add(q.Text);
            }
        }

        // 初始化答案字典
        for (int i = 0; i < AllQuestions.Count; i++)
        {
            PaperAnswers[i] = "";
        }
    }

    /// <summary>
    /// 更新试卷答案并触发 UI 更新
    /// </summary>
    protected void UpdateAnswer(int index, string answer)
    {
        PaperAnswers[index] = answer;
        StateHasChanged();
    }

    /// <summary>
    /// 试卷模式：提交试卷
    /// </summary>
    protected async Task SubmitPaper()
    {
        if (PaperSubmitted) return;

        Score = 0;

        // 批改所有题目
        for (int i = 0; i < AllQuestions.Count; i++)
        {
            var question = AllQuestions[i];
            string userAnswer = PaperAnswers.ContainsKey(i) ? PaperAnswers[i] : "";

            // 验证答案
            bool isCorrect = !string.IsNullOrWhiteSpace(userAnswer) && ValidateAnswer(userAnswer, question);

            if (isCorrect)
            {
                Score += 10;
            }
            else
            {
                // 保存到错题本
                try
                {
                    await WrongQuestionService.AddWrongQuestion(
                        question,
                        userAnswer.Trim(),
                        GetStoragePrefix()
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"保存错题失败: {ex.Message}");
                }
            }

            // 记录答题详情
            var item = new AskedItem
            {
                Q = question,
                Correct = isCorrect,
                User = userAnswer.Trim()
            };

            Asked.Add(item);
            PersistedAsked.Add(item);
            PersistedSeen.Add(question.Text);

            // 保存到考试记录
            if (CurrentExamRecord != null)
            {
                CurrentExamRecord.Answers.Add(new AnswerDetail
                {
                    QuestionId = question.Id,
                    QuestionText = question.Text,
                    CorrectAnswer = question.CorrectAnswer,
                    UserAnswer = userAnswer.Trim(),
                    IsCorrect = isCorrect,
                    Category = question.Category,
                    AnsweredAt = DateTime.UtcNow
                });
            }
        }

        // 保存到 LocalStorage
        await SaveProgress();

        // 所有批改完成后再设置提交标志，防止 UI 提前渲染导致索引越界
        PaperSubmitted = true;

        // 完成考试
        await CompleteExam();
    }

    #endregion

    #region 内部类

    /// <summary>
    /// 已答题目详情
    /// </summary>
    protected class AskedItem
    {
        /// <summary>
        /// 题目
        /// </summary>
        public Question Q { get; set; } = new Question();

        /// <summary>
        /// 是否正确
        /// </summary>
        public bool Correct { get; set; }

        /// <summary>
        /// 用户答案
        /// </summary>
        public string User { get; set; } = "";
    }

    #endregion
}
