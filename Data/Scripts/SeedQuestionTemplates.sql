-- 题目模板初始数据
-- 用于应用题、分数运算等题型的自动生成

-- 数学应用题模板
INSERT INTO question_templates (id, subject, type_name, template_code, template_content, parameters_config, answer_expression, difficulty, category, is_active, created_at, updated_at)
VALUES
-- 买卖应用题
('tpl_math_buy_1', 'math', '买卖应用题', 'word_problem_buy_simple',
'{name}买了{num1}个苹果，每个{price}元，一共花了多少元？',
'{"name": {"type": "name", "values": ["小明", "小红", "小华", "小刚", "小丽"]}, "num1": {"type": "int", "min": 2, "max": 10}, "price": {"type": "decimal", "min": 1, "max": 5}}',
'{num1} * {price}', 'Easy', '应用题-买卖', true, NOW(), NOW()),

('tpl_math_buy_2', 'math', '买卖应用题', 'word_problem_buy_medium',
'{name}有{money}元，买了{num1}本笔记本，每本{price}元，还剩多少元？',
'{"name": {"type": "name", "values": ["小明", "小红", "小华", "小刚", "小丽"]}, "money": {"type": "int", "min": 20, "max": 100}, "num1": {"type": "int", "min": 2, "max": 8}, "price": {"type": "decimal", "min": 2, "max": 10}}',
'{money} - ({num1} * {price})', 'Medium', '应用题-买卖', true, NOW(), NOW()),

-- 路程应用题
('tpl_math_distance_1', 'math', '路程应用题', 'word_problem_distance_simple',
'一辆汽车每小时行驶{speed}千米，行驶了{time}小时，一共行驶了多少千米？',
'{"speed": {"type": "int", "min": 40, "max": 100}, "time": {"type": "int", "min": 2, "max": 8}}',
'{speed} * {time}', 'Easy', '应用题-路程', true, NOW(), NOW()),

('tpl_math_distance_2', 'math', '路程应用题', 'word_problem_distance_medium',
'甲乙两地相距{distance}千米，一辆汽车从甲地开往乙地，每小时行驶{speed}千米，需要多少小时才能到达？',
'{"distance": {"type": "int", "min": 100, "max": 500}, "speed": {"type": "int", "min": 40, "max": 100}}',
'{distance} / {speed}', 'Medium', '应用题-路程', true, NOW(), NOW()),

-- 工程应用题
('tpl_math_work_1', 'math', '工程应用题', 'word_problem_work_simple',
'一项工程，{name}每天完成{num1}份，{days}天完成了多少份？',
'{"name": {"type": "name", "values": ["小明", "小红", "小华"]}, "num1": {"type": "fraction", "numerator_max": 5, "denominator_max": 10}, "days": {"type": "int", "min": 2, "max": 10}}',
'{num1} * {days}', 'Medium', '应用题-工程', true, NOW(), NOW()),

-- 分数加法模板
('tpl_math_fraction_add_1', 'math', '分数加法', 'fraction_add_same_denominator',
'计算：{frac1} + {frac2} = ?',
'{"frac1": {"type": "fraction", "numerator_max": 10, "denominator": "fixed", "denominator_value": "{denominator}"}, "frac2": {"type": "fraction", "numerator_max": 10, "denominator": "fixed", "denominator_value": "{denominator}"}, "denominator": {"type": "int", "min": 2, "max": 12}}',
'{frac1} + {frac2}', 'Easy', '分数加法-同分母', true, NOW(), NOW()),

('tpl_math_fraction_add_2', 'math', '分数加法', 'fraction_add_diff_denominator',
'计算：{frac1} + {frac2} = ?',
'{"frac1": {"type": "fraction", "numerator_max": 10, "denominator_max": 12}, "frac2": {"type": "fraction", "numerator_max": 10, "denominator_max": 12}}',
'{frac1} + {frac2}', 'Medium', '分数加法-异分母', true, NOW(), NOW()),

-- 分数减法模板
('tpl_math_fraction_sub_1', 'math', '分数减法', 'fraction_sub_same_denominator',
'计算：{frac1} - {frac2} = ?',
'{"frac1": {"type": "fraction", "numerator_max": 10, "denominator": "fixed", "denominator_value": "{denominator}"}, "frac2": {"type": "fraction", "numerator_max": 10, "denominator": "fixed", "denominator_value": "{denominator}"}, "denominator": {"type": "int", "min": 2, "max": 12}}',
'{frac1} - {frac2}', 'Easy', '分数减法-同分母', true, NOW(), NOW()),

('tpl_math_fraction_sub_2', 'math', '分数减法', 'fraction_sub_diff_denominator',
'计算：{frac1} - {frac2} = ?',
'{"frac1": {"type": "fraction", "numerator_max": 10, "denominator_max": 12}, "frac2": {"type": "fraction", "numerator_max": 10, "denominator_max": 12}}',
'{frac1} - {frac2}', 'Medium', '分数减法-异分母', true, NOW(), NOW()),

-- 分数乘法模板
('tpl_math_fraction_mul_1', 'math', '分数乘法', 'fraction_mul_simple',
'计算：{frac1} × {num1} = ?',
'{"frac1": {"type": "fraction", "numerator_max": 10, "denominator_max": 12}, "num1": {"type": "int", "min": 2, "max": 10}}',
'{frac1} * {num1}', 'Easy', '分数乘法', true, NOW(), NOW()),

('tpl_math_fraction_mul_2', 'math', '分数乘法', 'fraction_mul_fraction',
'计算：{frac1} × {frac2} = ?',
'{"frac1": {"type": "fraction", "numerator_max": 10, "denominator_max": 12}, "frac2": {"type": "fraction", "numerator_max": 10, "denominator_max": 12}}',
'{frac1} * {frac2}', 'Medium', '分数乘法', true, NOW(), NOW()),

-- 分数除法模板
('tpl_math_fraction_div_1', 'math', '分数除法', 'fraction_div_simple',
'计算：{frac1} ÷ {num1} = ?',
'{"frac1": {"type": "fraction", "numerator_max": 10, "denominator_max": 12}, "num1": {"type": "int", "min": 2, "max": 10}}',
'{frac1} / {num1}', 'Medium', '分数除法', true, NOW(), NOW()),

('tpl_math_fraction_div_2', 'math', '分数除法', 'fraction_div_fraction',
'计算：{frac1} ÷ {frac2} = ?',
'{"frac1": {"type": "fraction", "numerator_max": 10, "denominator_max": 12}, "frac2": {"type": "fraction", "numerator_max": 10, "denominator_max": 12}}',
'{frac1} / {frac2}', 'Hard', '分数除法', true, NOW(), NOW()),

-- 百分数应用题
('tpl_math_percent_1', 'math', '百分数应用题', 'percent_discount',
'一件衣服原价{price}元，打{discount}折后，现价多少元？',
'{"price": {"type": "int", "min": 100, "max": 500}, "discount": {"type": "decimal", "min": 7, "max": 9.5, "step": 0.5}}',
'{price} * ({discount} / 10)', 'Easy', '百分数-折扣', true, NOW(), NOW()),

('tpl_math_percent_2', 'math', '百分数应用题', 'percent_increase',
'一个数是{num1}，比{num2}多百分之几？',
'{"num1": {"type": "int", "min": 100, "max": 200}, "num2": {"type": "int", "min": 50, "max": 100}}',
'(({num1} - {num2}) / {num2}) * 100', 'Medium', '百分数-增长率', true, NOW(), NOW());

-- 语文题目模板（成语、近义词、反义词等）
INSERT INTO question_templates (id, subject, type_name, template_code, template_content, parameters_config, answer_expression, difficulty, category, is_active, created_at, updated_at)
VALUES
-- 成语填空
('tpl_chinese_idiom_1', 'chinese', '成语填空', 'idiom_fill_blank',
'{idiom_part1}____{idiom_part2}',
'{"idiom_part1": {"type": "string"}, "idiom_part2": {"type": "string"}, "answer": {"type": "string"}}',
'{answer}', 'Medium', '成语', true, NOW(), NOW()),

-- 近义词
('tpl_chinese_synonym_1', 'chinese', '近义词', 'synonym_choose',
'"{word}"的近义词是（　　）',
'{"word": {"type": "string"}, "options": {"type": "array"}, "answer": {"type": "string"}}',
'{answer}', 'Easy', '近义词', true, NOW(), NOW()),

-- 反义词
('tpl_chinese_antonym_1', 'chinese', '反义词', 'antonym_choose',
'"{word}"的反义词是（　　）',
'{"word": {"type": "string"}, "options": {"type": "array"}, "answer": {"type": "string"}}',
'{answer}', 'Easy', '反义词', true, NOW(), NOW());
