# Tesseract OCR 语言包安装指南

## 📦 需要的语言包

离线OCR功能需要以下Tesseract语言包：

- **chi_sim.traineddata** - 简体中文（必需，约17MB）
- **eng.traineddata** - 英文（必需，约22MB）

## 🚀 快速下载方案

### 方案1：浏览器直接下载（推荐⭐）

**标准版（准确度高，文件大）：**
1. 打开：https://github.com/tesseract-ocr/tessdata/tree/main
2. 下载以下文件：
   - [chi_sim.traineddata](https://github.com/tesseract-ocr/tessdata/raw/main/chi_sim.traineddata)
   - [eng.traineddata](https://github.com/tesseract-ocr/tessdata/raw/main/eng.traineddata)

**Fast版（速度快，文件小，推荐）：**
1. 打开：https://github.com/tesseract-ocr/tessdata_fast/tree/main
2. 下载以下文件：
   - [chi_sim.traineddata](https://github.com/tesseract-ocr/tessdata_fast/raw/main/chi_sim.traineddata)（~3MB）
   - [eng.traineddata](https://github.com/tesseract-ocr/tessdata_fast/raw/main/eng.traineddata)（~4MB）

### 方案2：使用国内镜像（更快）

```bash
# 在项目根目录执行
cd /Users/sep229/Documents/trae_projects/HelloWorld/MathExamWeb/tessdata

# 从Gitee镜像下载（如果有）
curl -L -o chi_sim.traineddata "https://gitee.com/mirrors/tesseract-ocr_tessdata_fast/raw/main/chi_sim.traineddata"
curl -L -o eng.traineddata "https://gitee.com/mirrors/tesseract-ocr_tessdata_fast/raw/main/eng.traineddata"
```

### 方案3：使用命令行工具

```bash
cd /Users/sep229/Documents/trae_projects/HelloWorld/MathExamWeb/tessdata

# 使用wget（如果已安装）
wget https://github.com/tesseract-ocr/tessdata_fast/raw/main/chi_sim.traineddata
wget https://github.com/tesseract-ocr/tessdata_fast/raw/main/eng.traineddata

# 或使用aria2c（多线程下载，更快）
aria2c -x 16 https://github.com/tesseract-ocr/tessdata_fast/raw/main/chi_sim.traineddata
aria2c -x 16 https://github.com/tesseract-ocr/tessdata_fast/raw/main/eng.traineddata
```

## 📁 安装位置

下载后的文件应放在以下目录：
```
/Users/sep229/Documents/trae_projects/HelloWorld/MathExamWeb/tessdata/
├── chi_sim.traineddata
└── eng.traineddata
```

## ✅ 验证安装

安装完成后，启动项目：
```bash
dotnet run
```

访问 `http://localhost:5000/question-import`，选择"🔒 离线OCR"模式。

如果看到：
- ✅ 离线OCR已就绪
- 可用语言：**chi_sim, eng**

说明安装成功！

## 🆚 版本对比

| 版本 | 文件大小 | 识别准确度 | 识别速度 | 推荐场景 |
|------|----------|-----------|----------|----------|
| **tessdata** | 大（17-22MB） | ⭐⭐⭐⭐⭐ | 较慢 | 高精度需求 |
| **tessdata_fast** ⭐ | 小（3-4MB） | ⭐⭐⭐⭐ | 快 | 日常使用 |
| **tessdata_best** | 很大（50MB+） | ⭐⭐⭐⭐⭐ | 很慢 | 专业场景 |

**建议**：一般使用推荐 **tessdata_fast** 版本，平衡了速度和准确度。

## 🔗 官方资源

- Tesseract官网：https://github.com/tesseract-ocr/tesseract
- 标准语言包：https://github.com/tesseract-ocr/tessdata
- 快速语言包：https://github.com/tesseract-ocr/tessdata_fast
- 最佳语言包：https://github.com/tesseract-ocr/tessdata_best

## ❓ 常见问题

**Q: 下载速度很慢怎么办？**
A: 尝试使用浏览器下载，或使用迅雷等下载工具。

**Q: 是否需要安装Tesseract程序本身？**
A: 不需要！项目已通过NuGet包含了Tesseract引擎，只需下载语言包。

**Q: 可以只下载中文语言包吗？**
A: 可以，但建议同时下载中英文，因为很多题目包含英文字母和数字。

**Q: 语言包放错位置了怎么办？**
A: 确保文件直接放在 `tessdata/` 目录下，不要有子文件夹。
