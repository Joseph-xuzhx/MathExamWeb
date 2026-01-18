namespace MathExamWeb.Data.Models
{
    /// <summary>
    /// 考试模式
    /// </summary>
    public enum ExamMode
    {
        /// <summary>
        /// 逐题模式：一题一题做，做完提交查看结果
        /// </summary>
        Sequential = 0,

        /// <summary>
        /// 试卷模式：一次性生成所有题目，全部答完后统一提交批改
        /// </summary>
        Paper = 1
    }
}
