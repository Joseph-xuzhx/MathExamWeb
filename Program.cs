using MathExamWeb.Data;
using MathExamWeb.Data.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 添加数据库上下文
builder.Services.AddDbContext<MathExamDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => { options.DetailedErrors = true; });
builder.Services.AddSingleton<MathQuestionService>();
builder.Services.AddSingleton<ChineseQuestionService>();
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<MathExamWeb.Data.Services.StatisticsService>();
builder.Services.AddScoped<MathExamWeb.Data.Services.WrongQuestionService>();
builder.Services.AddHttpClient<MathExamWeb.Data.Services.AIQuestionParserService>();
builder.Services.AddHttpClient<MathExamWeb.Data.Services.OCRService>();
builder.Services.AddScoped<MathExamWeb.Data.Services.PDFService>();
builder.Services.AddSingleton<MathExamWeb.Data.Services.OfflineOCRService>();
builder.Services.AddScoped<QuestionRepository>();
builder.Services.AddScoped<ExcelExportService>();
builder.Services.AddScoped<QuestionTemplateSeedService>();
builder.Services.AddScoped<CategorySeedService>();

var app = builder.Build();

// 初始化数据
using (var scope = app.Services.CreateScope())
{
    var templateSeedService = scope.ServiceProvider.GetRequiredService<QuestionTemplateSeedService>();
    await templateSeedService.SeedTemplatesAsync();

    var categorySeedService = scope.ServiceProvider.GetRequiredService<CategorySeedService>();
    await categorySeedService.SeedCategoriesAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
