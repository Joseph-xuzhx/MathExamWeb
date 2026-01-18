# MathExamWeb

基于 Blazor Server 的数学语文在线考试系统

## 功能特性

- 📝 **在线考试系统**
  - 数学考试（选择题、填空题、计算题、应用题等）
  - 语文考试（选择题、填空题、古诗文、阅读理解等）
  - 多种难度级别支持
  - 实时答题进度显示

- 📚 **题库管理**
  - 题目的增删改查
  - Excel 批量导入题目
  - 分类管理（支持多级分类）
  - 多条件筛选和分页显示
  - 手工添加题目功能

- 📊 **统计分析**
  - 考试记录和历史查询
  - 成绩统计和趋势分析
  - 错题本功能
  - 分类别统计

- 💾 **数据持久化**
  - PostgreSQL 数据库存储
  - Entity Framework Core ORM
  - 数据库迁移管理

## 技术栈

- **前端框架**: Blazor Server (.NET 8.0)
- **数据库**: PostgreSQL 16
- **ORM**: Entity Framework Core 8.0.10
- **UI框架**: Bootstrap 5
- **其他**:
  - Npgsql (PostgreSQL 提供程序)
  - EPPlus (Excel 处理)
  - Tesseract OCR (图像识别)

## 快速开始

### 前置要求

- .NET 8.0 SDK
- PostgreSQL 16+

### 安装步骤

1. 克隆仓库
```bash
git clone https://github.com/Joseph-xuzhx/MathExamWeb.git
cd MathExamWeb
```

2. 配置数据库连接
编辑 `appsettings.json`，修改数据库连接字符串：
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=mathexam;Username=postgres;Password=your_password"
  }
}
```

3. 运行数据库迁移
```bash
dotnet ef database update
```

4. 启动应用
```bash
dotnet run
```

5. 访问应用
打开浏览器访问 `http://localhost:5000`

## 项目结构

```
MathExamWeb/
├── Components/          # Blazor 组件
│   └── Base/           # 基础组件
├── Data/               # 数据层
│   ├── Models/         # 实体模型
│   ├── Services/       # 业务服务
│   └── Scripts/        # SQL 脚本
├── Migrations/         # EF Core 迁移文件
├── Pages/              # Blazor 页面
├── wwwroot/            # 静态资源
└── Program.cs          # 应用入口

```

## 数据库表结构

- `questions` - 题目表
- `exam_records` - 考试记录表
- `wrong_questions` - 错题记录表
- `categories` - 分类字典表
- `question_templates` - 题目模板表

## 主要功能模块

### 1. 考试模块
- 支持数学和语文两个科目
- 可选择难度级别和题目数量
- 实时显示答题进度
- 自动评分和错题标记

### 2. 题库管理
- 题目的完整 CRUD 操作
- Excel 批量导入（支持自定义模板）
- 多维度筛选（科目、类型、难度、分类）
- 分页显示和排序

### 3. 分类管理
- 分类的增删改查
- 支持分组管理
- 从题库自动导入分类
- 启用/禁用状态控制

### 4. 统计分析
- 历次考试记录查询
- 成绩趋势图表
- 错题统计和复习
- 按科目、难度等维度分析

## 开发说明

### 添加数据库迁移

```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

### 构建发布

```bash
dotnet publish -c Release
```

## 许可证

本项目采用 MIT 许可证

## 贡献

欢迎提交 Issue 和 Pull Request！

## 作者

Joseph-xuzhx

---

🤖 由 Claude Sonnet 4.5 协助开发
