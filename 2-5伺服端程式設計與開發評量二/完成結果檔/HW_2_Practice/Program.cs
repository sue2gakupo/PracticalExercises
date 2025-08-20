using HW_2_Practice.Services;

var builder = WebApplication.CreateBuilder(args);


// 註冊 MVC 控制器
builder.Services.AddControllers();

// 註冊 API 探索服務 (用於 Swagger)
builder.Services.AddEndpointsApiExplorer();

// 註冊 Swagger 服務並設定
builder.Services.AddSwaggerGen();

// 註冊 HttpClient (用於呼叫第三方 API)
builder.Services.AddHttpClient();

// 註冊自訂服務 - 依賴注入的關鍵設定
// Scoped：每個 HTTP 請求建立一個實例
builder.Services.AddScoped<IAirQualityService, AirQualityService>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();


// 開發環境(Development)才啟用 Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();           // 啟用 Swagger JSON 端點
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();  // 強制 HTTPS 重新導向
app.UseAuthorization();     // 授權中介軟體 (目前未使用)
app.MapControllers();       // 對應控制器路由


app.Run();                  // 啟動應用程式