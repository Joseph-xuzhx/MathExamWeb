using System;
using MathExamWeb.Data.Models;
using MathExamWeb.Data.Services.Interfaces;

namespace MathExamWeb.Data
{
    public class MathQuestionService : IQuestionService
    {
        private Random _random = new Random();

        public Question GenerateQuestion(DifficultyLevel? difficulty = null)
        {
            // 50% 概率生成选择题，50% 概率生成填空题
            bool isMultipleChoice = _random.Next(0, 2) == 0;

            if (isMultipleChoice)
            {
                return GenerateMultipleChoice();
            }
            else
            {
                int type = _random.Next(1, 6); // 1: Fraction, 2: Ratio, 3: Equation, 4: Geometry, 5: Word Problem
                string question = "";
                string correctAnswer = "";
                string explanation = "";
                bool isFractionAnswer = false;

                switch (type)
                {
                    case 1:
                        (question, correctAnswer, explanation) = GenerateFractionQuestion();
                        isFractionAnswer = true;
                        break;
                    case 2:
                        (question, correctAnswer, explanation) = GenerateRatioQuestion();
                        break;
                    case 3:
                        (question, correctAnswer, explanation) = GenerateEquationQuestion();
                        isFractionAnswer = true;
                        break;
                    case 4:
                        (question, correctAnswer, explanation) = GenerateGeometryQuestion();
                        break;
                    case 5:
                        (question, correctAnswer, explanation) = GenerateWordProblem();
                        break;
                }

                return new Question
                {
                    Type = QuestionType.FillInBlank,
                    Text = question,
                    CorrectAnswer = correctAnswer,
                    Explanation = explanation,
                    IsFractionAnswer = isFractionAnswer,
                    Category = "填空题",
                    Difficulty = difficulty ?? DifficultyLevel.Medium
                };
            }
        }

        private (string, string, string) GenerateFractionQuestion()
        {
            int op = _random.Next(0, 4); // +, -, *, /
            Fraction a = new Fraction(_random.Next(1, 10), _random.Next(2, 10));
            Fraction b = new Fraction(_random.Next(1, 10), _random.Next(2, 10));
            
            Fraction result;
            string opSymbol = "";
            string explanation = "";

            switch (op)
            {
                case 0: 
                    opSymbol = "+"; 
                    result = a + b;
                    explanation = $"计算过程：\n1. 通分：{a} + {b} = {(a.Numerator * b.Denominator)}/{(a.Denominator * b.Denominator)} + {(b.Numerator * a.Denominator)}/{(a.Denominator * b.Denominator)}\n2. 相加：= {(a.Numerator * b.Denominator + b.Numerator * a.Denominator)}/{(a.Denominator * b.Denominator)}\n3. 约分后得：{result}";
                    break;
                case 1: 
                    if (a.Numerator * b.Denominator < b.Numerator * a.Denominator) { var t = a; a = b; b = t; }
                    opSymbol = "-"; 
                    result = a - b;
                    explanation = $"计算过程：\n1. 通分：{a} - {b} = {(a.Numerator * b.Denominator)}/{(a.Denominator * b.Denominator)} - {(b.Numerator * a.Denominator)}/{(a.Denominator * b.Denominator)}\n2. 相减：= {(a.Numerator * b.Denominator - b.Numerator * a.Denominator)}/{(a.Denominator * b.Denominator)}\n3. 约分后得：{result}";
                    break;
                case 2: 
                    opSymbol = "×"; 
                    result = a * b;
                    explanation = $"计算过程：\n1. 分子乘分子：{a.Numerator} × {b.Numerator} = {a.Numerator * b.Numerator}\n2. 分母乘分母：{a.Denominator} × {b.Denominator} = {a.Denominator * b.Denominator}\n3. 结果：{(a.Numerator * b.Numerator)}/{(a.Denominator * b.Denominator)}\n4. 约分后得：{result}";
                    break;
                default: 
                    opSymbol = "÷"; 
                    result = a / b;
                    explanation = $"计算过程：\n1. 除以一个数等于乘以它的倒数：{a} ÷ {b} = {a} × {b.Denominator}/{b.Numerator}\n2. 执行乘法：{(a.Numerator * b.Denominator)}/{(a.Denominator * b.Numerator)}\n3. 约分后得：{result}";
                    break;
            }

            return ($"{a} {opSymbol} {b} = ?", result.ToString(), explanation);
        }

        private (string, string, string) GenerateRatioQuestion()
        {
            int c = _random.Next(2, 10);
            int b = _random.Next(1, 10) * c; 
            int a = _random.Next(1, 10);
            
            int ratio = b / c;
            int x = a * ratio;
            
            int pos = _random.Next(0, 2);
            string q = "";
            string exp = "";

            if (pos == 0)
            {
                q = $"求 x 的值：x : {a} = {b} : {c}";
                exp = $"根据比例的基本性质：两个外项的积等于两个内项的积。\n1. 列出方程：x × {c} = {a} × {b}\n2. 计算右边：{c}x = {a * b}\n3. 解方程：x = {a * b} ÷ {c}\n4. 得出 x = {x}";
            }
            else
            {
                q = $"求 x 的值：{a} : x = {c} : {b}";
                exp = $"根据比例的基本性质：两个外项的积等于两个内项的积。\n1. 列出方程：x × {c} = {a} × {b}\n2. 计算右边：{c}x = {a * b}\n3. 解方程：x = {a * b} ÷ {c}\n4. 得出 x = {x}";
            }
            return (q, x.ToString(), exp);
        }

        private (string, string, string) GenerateEquationQuestion()
        {
            int x = _random.Next(1, 11);
            int a = _random.Next(1, 6);
            int b = _random.Next(1, 20);
            int c = a * x + b;

            string exp = $"解方程步骤：\n1. 移项：{a}x = {c} - {b}\n2. 化简：{a}x = {c - b}\n3. 两边同时除以 {a}：x = {c - b} ÷ {a}\n4. 得出 x = {x}";
            return ($"解方程：{a}x + {b} = {c}，x = ?", x.ToString(), exp);
        }

        private (string, string, string) GenerateGeometryQuestion()
        {
            int r = _random.Next(1, 11);
            bool askArea = _random.Next(0, 2) == 0;
            
            if (askArea)
            {
                double area = 3.14 * r * r;
                string ans = area.ToString("0.##");
                string exp = $"圆的面积公式 S = πr²\n计算过程：\nS = 3.14 × {r}²\n  = 3.14 × {r * r}\n  = {ans}";
                return ($"一个圆的半径是 {r} 厘米，求它的面积 (π取3.14) = ?", ans, exp);
            }
            else
            {
                double c = 2 * 3.14 * r;
                string ans = c.ToString("0.##");
                string exp = $"圆的周长公式 C = 2πr\n计算过程：\nC = 2 × 3.14 × {r}\n  = 6.28 × {r}\n  = {ans}";
                return ($"一个圆的半径是 {r} 厘米，求它的周长 (π取3.14) = ?", ans, exp);
            }
        }

        private (string, string, string) GenerateWordProblem()
        {
            int subType = _random.Next(0, 4); // 0: Discount, 1: Work, 2: Ratio Allocation, 3: Speed/Distance
            string q = "", a = "", e = "";

            switch (subType)
            {
                case 0: // 折扣问题
                    int original = _random.Next(1, 20) * 100; // 100 - 1900
                    int discount = _random.Next(5, 10); // 5折 - 9折
                    int price = original * discount / 10;
                    q = $"一件商品原价 {original} 元，现在打 {discount} 折出售，现价是多少元？";
                    a = price.ToString();
                    e = $"计算公式：现价 = 原价 × (折扣 ÷ 10)\n计算过程：{original} × {discount/10.0} = {price}";
                    break;

                case 1: // 工程问题 (凑整数结果)
                    int[][] pairs = new int[][] { 
                        new int[]{10, 15}, new int[]{20, 30}, new int[]{12, 24}, 
                        new int[]{6, 12}, new int[]{20, 5}, new int[]{30, 15}, new int[]{12, 6} 
                    };
                    int[] pair = pairs[_random.Next(0, pairs.Length)];
                    int t1 = pair[0];
                    int t2 = pair[1];
                    int together = (t1 * t2) / (t1 + t2);
                    
                    q = $"一项工程，甲队单独做需要 {t1} 天完成，乙队单独做需要 {t2} 天完成。两队合作需要多少天完成？";
                    a = together.ToString();
                    e = $"把工作总量看作单位“1”。\n甲的效率是 1/{t1}，乙的效率是 1/{t2}。\n合作效率 = 1/{t1} + 1/{t2} = {(t1+t2)}/{(t1*t2)} = 1/{together}\n合作时间 = 1 ÷ (1/{together}) = {together} 天";
                    break;

                case 2: // 按比分配
                    int r1 = _random.Next(2, 6);
                    int r2 = _random.Next(2, 6);
                    int totalParts = r1 + r2;
                    int multiplier = _random.Next(10, 50); // 倍数
                    int total = totalParts * multiplier;
                    int val1 = r1 * multiplier;

                    q = $"学校买来 {total} 本图书，按 {r1}:{r2} 分给五、六年级。五年级分得多少本？";
                    a = val1.ToString();
                    e = $"总份数 = {r1} + {r2} = {totalParts}\n五年级占总数的 {r1}/{totalParts}\n五年级分得：{total} × ({r1}/{totalParts}) = {val1} 本";
                    break;

                case 3: // 行程问题 (相遇)
                    int v1 = _random.Next(40, 90); // 速度 km/h
                    int v2 = _random.Next(40, 90);
                    int t = _random.Next(2, 6); // 时间 h
                    int dist = (v1 + v2) * t;
                    
                    q = $"甲、乙两车同时从相距 {dist} 千米的两地相对开出，甲车每小时行 {v1} 千米，乙车每小时行 {v2} 千米。经过几小时两车相遇？";
                    a = t.ToString();
                    e = $"速度和 = {v1} + {v2} = {v1+v2} 千米/小时\n相遇时间 = 总路程 ÷ 速度和\n计算过程：{dist} ÷ {v1+v2} = {t} 小时";
                    break;
            }
            return (q, a, e);
        }

        public List<Question> GetQuestionsByDifficulty(DifficultyLevel difficulty)
        {
            // 暂时返回空列表，在阶段三实现难度分级时完善
            return new List<Question>();
        }

        public List<string> GetCategories()
        {
            return new List<string> { "分数运算", "比例问题", "一元一次方程", "几何问题", "应用题" };
        }

        public Dictionary<string, int> GetCategoryStatistics()
        {
            return new Dictionary<string, int>
            {
                { "分数运算", -1 },  // -1 表示无限生成
                { "比例问题", -1 },
                { "一元一次方程", -1 },
                { "几何问题", -1 },
                { "应用题", -1 }
            };
        }

        private Question GenerateMultipleChoice()
        {
            var items = new (string question, string correct, string[] options, string exp)[]
            {
                // 分数运算题（10道）
                ("计算 1/2 + 1/3 的结果是？", "5/6",
                 new[] { "5/6", "2/5", "1/6", "3/5" },
                 "通分后：1/2 + 1/3 = 3/6 + 2/6 = 5/6"),

                ("计算 3/4 - 1/2 的结果是？", "1/4",
                 new[] { "1/4", "2/4", "1/2", "1/8" },
                 "通分后：3/4 - 1/2 = 3/4 - 2/4 = 1/4"),

                ("计算 2/3 × 3/4 的结果是？", "1/2",
                 new[] { "1/2", "6/12", "5/7", "2/3" },
                 "分子乘分子，分母乘分母：2×3 / 3×4 = 6/12 = 1/2"),

                ("计算 3/5 ÷ 2/3 的结果是？", "9/10",
                 new[] { "9/10", "6/15", "1/2", "2/5" },
                 "除以一个分数等于乘以它的倒数：3/5 × 3/2 = 9/10"),

                ("一个数的3/5是30，这个数是多少？", "50",
                 new[] { "50", "18", "45", "60" },
                 "用除法：30 ÷ 3/5 = 30 × 5/3 = 50"),

                ("分数 12/18 化简后是？", "2/3",
                 new[] { "2/3", "6/9", "4/6", "1/2" },
                 "分子分母同时除以最大公约数6：12÷6 / 18÷6 = 2/3"),

                ("比较大小：5/6 ( ) 7/8", "＜",
                 new[] { "＜", "＞", "＝", "无法比较" },
                 "通分：5/6 = 20/24，7/8 = 21/24，所以 5/6 ＜ 7/8"),

                ("计算 1 - 2/5 的结果是？", "3/5",
                 new[] { "3/5", "2/5", "1/5", "4/5" },
                 "1 = 5/5，5/5 - 2/5 = 3/5"),

                ("一根绳子长 3/4 米，用去 1/3，还剩多少米？", "5/12米",
                 new[] { "5/12米", "1/2米", "1/4米", "7/12米" },
                 "3/4 - 1/3 = 9/12 - 4/12 = 5/12米"),

                ("把 5 米长的绳子平均分成 8 段，每段是全长的几分之几？", "1/8",
                 new[] { "1/8", "5/8", "8/5", "1/5" },
                 "平均分成8段，每段是全长的 1÷8 = 1/8"),

                // 百分数应用题（10道）
                ("一件商品打8折出售，相当于降价百分之几？", "20%",
                 new[] { "20%", "80%", "8%", "12%" },
                 "打8折即按原价的80%出售，降价 = 100% - 80% = 20%"),

                ("一个班有50人，其中男生30人，男生占全班人数的百分之几？", "60%",
                 new[] { "60%", "40%", "30%", "50%" },
                 "男生占比 = 30 ÷ 50 = 0.6 = 60%"),

                ("一种商品原价200元，提价10%后又降价10%，现价是多少元？", "198元",
                 new[] { "198元", "200元", "180元", "220元" },
                 "提价后：200×(1+10%)=220元，降价后：220×(1-10%)=198元"),

                ("小明的身高是150cm，比爸爸矮20%，爸爸身高是多少cm？", "187.5cm",
                 new[] { "187.5cm", "180cm", "170cm", "175cm" },
                 "设爸爸身高为x，x×(1-20%)=150，x×0.8=150，x=187.5"),

                ("一本书120页，已读30%，已读多少页？", "36页",
                 new[] { "36页", "30页", "40页", "84页" },
                 "已读页数 = 120 × 30% = 120 × 0.3 = 36页"),

                ("某商品成本80元，按成本价提高25%标价，商品标价是多少元？", "100元",
                 new[] { "100元", "105元", "95元", "120元" },
                 "标价 = 80 × (1 + 25%) = 80 × 1.25 = 100元"),

                ("一个数的40%是16，这个数是多少？", "40",
                 new[] { "40", "32", "24", "48" },
                 "16 ÷ 40% = 16 ÷ 0.4 = 40"),

                ("学校图书馆新购图书500本，其中科技书占20%，科技书有多少本？", "100本",
                 new[] { "100本", "80本", "120本", "150本" },
                 "科技书 = 500 × 20% = 500 × 0.2 = 100本"),

                ("25是50的百分之几？", "50%",
                 new[] { "50%", "25%", "75%", "100%" },
                 "25 ÷ 50 = 0.5 = 50%"),

                ("一件衣服降价20%后售价为80元，原价是多少元？", "100元",
                 new[] { "100元", "96元", "60元", "64元" },
                 "设原价为x，x×(1-20%)=80，x×0.8=80，x=100"),

                // 比例与比的应用题（8道）
                ("化简比：18:24 = ?", "3:4",
                 new[] { "3:4", "9:12", "6:8", "2:3" },
                 "18和24的最大公约数是6，18÷6:24÷6 = 3:4"),

                ("一个长方形长6cm，宽4cm，长与宽的比是？", "3:2",
                 new[] { "3:2", "2:3", "6:4", "4:6" },
                 "长:宽 = 6:4 = 3:2（化简）"),

                ("甲数是20，乙数是25，甲数与乙数的比是？", "4:5",
                 new[] { "4:5", "5:4", "20:25", "2:3" },
                 "甲:乙 = 20:25 = 4:5（化简）"),

                ("按3:2分配60，较大的一份是多少？", "36",
                 new[] { "36", "24", "30", "40" },
                 "总份数 = 3+2 = 5，较大份 = 60 × 3/5 = 36"),

                ("一种农药，药粉和水按1:500配制，现有药粉2kg，需要加水多少kg？", "1000kg",
                 new[] { "1000kg", "500kg", "250kg", "2000kg" },
                 "水的质量 = 2 × 500 = 1000kg"),

                ("求比值：0.5:0.25 = ?", "2",
                 new[] { "2", "0.5", "0.25", "1" },
                 "0.5 ÷ 0.25 = 2"),

                ("如果 x:3 = 4:6，那么 x = ?", "2",
                 new[] { "2", "3", "4", "6" },
                 "根据比例性质：x × 6 = 3 × 4，6x = 12，x = 2"),

                ("把一个比的前项扩大2倍，后项也扩大2倍，比值会？", "不变",
                 new[] { "不变", "扩大2倍", "缩小2倍", "扩大4倍" },
                 "比值 = 前项÷后项，前后项同时扩大相同倍数，比值不变"),

                // 几何图形题（8道）
                ("下列哪个图形的对称轴最多？", "圆",
                 new[] { "圆", "正方形", "等腰三角形", "长方形" },
                 "圆有无数条对称轴，正方形有4条，长方形有2条，等腰三角形有1条"),

                ("一个圆的半径是5cm，它的直径是多少cm？", "10cm",
                 new[] { "10cm", "5cm", "15cm", "20cm" },
                 "直径 = 半径 × 2 = 5 × 2 = 10cm"),

                ("一个正方形的周长是20cm，它的面积是多少平方厘米？", "25平方厘米",
                 new[] { "25平方厘米", "20平方厘米", "100平方厘米", "16平方厘米" },
                 "边长 = 20 ÷ 4 = 5cm，面积 = 5 × 5 = 25平方厘米"),

                ("长方形的长是8cm，宽是5cm，周长是多少cm？", "26cm",
                 new[] { "26cm", "13cm", "40cm", "24cm" },
                 "周长 = (长+宽) × 2 = (8+5) × 2 = 26cm"),

                ("一个三角形的三个内角度数比是2:3:4，最大的角是多少度？", "80度",
                 new[] { "80度", "60度", "90度", "40度" },
                 "总份数 = 2+3+4 = 9，最大角 = 180° × 4/9 = 80°"),

                ("下列图形中，是轴对称图形的是？", "等腰三角形",
                 new[] { "等腰三角形", "平行四边形", "梯形", "任意三角形" },
                 "等腰三角形有一条对称轴，平行四边形和一般梯形不是轴对称图形"),

                ("一个圆的周长是12.56cm（π取3.14），它的半径是多少cm？", "2cm",
                 new[] { "2cm", "4cm", "6cm", "8cm" },
                 "C = 2πr，12.56 = 2 × 3.14 × r，r = 12.56 ÷ 6.28 = 2cm"),

                ("一个长方体有多少条棱？", "12条",
                 new[] { "12条", "8条", "6条", "10条" },
                 "长方体有12条棱，分为3组，每组4条"),

                // 统计与数据题（7道）
                ("数据 5, 3, 7, 5, 8, 5, 6 的众数是？", "5",
                 new[] { "5", "7", "6", "8" },
                 "众数是出现次数最多的数，5出现了3次，是众数"),

                ("数据 2, 4, 6, 8, 10 的平均数是？", "6",
                 new[] { "6", "5", "7", "8" },
                 "平均数 = (2+4+6+8+10) ÷ 5 = 30 ÷ 5 = 6"),

                ("把数据 5, 9, 3, 7, 1 从小到大排列，中位数是？", "5",
                 new[] { "5", "7", "3", "9" },
                 "排序后：1, 3, 5, 7, 9，中位数是中间的数，即5"),

                ("一组数据：10, 12, 8, 10, 15，这组数据的极差是？", "7",
                 new[] { "7", "5", "10", "15" },
                 "极差 = 最大值 - 最小值 = 15 - 8 = 7"),

                ("小明6次测验成绩分别是：85, 90, 88, 92, 90, 95，平均成绩是？", "90分",
                 new[] { "90分", "88分", "92分", "85分" },
                 "平均分 = (85+90+88+92+90+95) ÷ 6 = 540 ÷ 6 = 90分"),

                ("一个骰子投掷一次，出现偶数的可能性是？", "1/2",
                 new[] { "1/2", "1/3", "1/6", "2/3" },
                 "骰子有6个面，偶数有2,4,6三个，可能性 = 3/6 = 1/2"),

                ("在一副扑克牌（去掉大小王）中，任意抽一张，抽到红桃的可能性是？", "1/4",
                 new[] { "1/4", "1/2", "1/13", "4/13" },
                 "扑克牌去掉大小王有52张，红桃有13张，可能性 = 13/52 = 1/4"),

                // 数的运算题（7道）
                ("下列各数中，最大的是？", "3.14",
                 new[] { "3.14", "3.1", "3.009", "3.01" },
                 "比较小数大小，整数部分相同看小数部分，3.14最大"),

                ("计算：25 × 4 + 75 × 4 = ?", "400",
                 new[] { "400", "300", "500", "450" },
                 "运用乘法分配律：(25+75) × 4 = 100 × 4 = 400"),

                ("0.125 等于几分之几？", "1/8",
                 new[] { "1/8", "1/4", "1/5", "1/10" },
                 "0.125 = 125/1000 = 1/8（约分）"),

                ("3.6 ÷ 0.4 = ?", "9",
                 new[] { "9", "0.9", "90", "0.09" },
                 "被除数和除数同时扩大10倍：36 ÷ 4 = 9"),

                ("一个数的小数点向右移动一位，这个数就？", "扩大10倍",
                 new[] { "扩大10倍", "缩小10倍", "扩大100倍", "不变" },
                 "小数点向右移动一位，数扩大10倍"),

                ("把 2.05 改写成以百分之一为单位的数是？", "205个百分之一",
                 new[] { "205个百分之一", "20.5个百分之一", "2.05个百分之一", "2050个百分之一" },
                 "2.05 = 205 × 0.01 = 205个百分之一"),

                ("下列算式中，积最大的是？", "0.8×1.2",
                 new[] { "0.8×1.2", "0.6×1.2", "0.8×0.9", "0.5×1.1" },
                 "0.8×1.2=0.96，0.6×1.2=0.72，0.8×0.9=0.72，0.5×1.1=0.55"),
            };

            var pick = items[_random.Next(items.Length)];
            var shuffledOptions = pick.options.OrderBy(x => _random.Next()).ToList();

            return new Question
            {
                Type = QuestionType.MultipleChoice,
                Text = pick.question,
                CorrectAnswer = pick.correct,
                Options = shuffledOptions,
                Explanation = pick.exp,
                Category = "选择题",
                Difficulty = DifficultyLevel.Medium
            };
        }
    }
}
