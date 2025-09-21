using BondleApplication.Access.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BondleDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BondleDBConnection")));

builder.Services.AddDbContext<BondleDBContext2>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BondleDBConnection")));

builder.Services.AddScoped<IdGeneratorService>();

builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.ViewLocationFormats.Add("/Views/Shared/Partials/{0}.cshtml");
});



// 這裡新增認證服務
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Shared/Login/Login"; // 設定登入頁面的路徑
        options.LogoutPath = "/Shared/Login/Logout"; // 設定登出頁面的路徑
        options.ExpireTimeSpan = TimeSpan.FromDays(7); // 設定 Cookie 過期時間
        options.SlidingExpiration = true; // 允許滑動過期
    });



////////////////////////////////////////////////////////////////
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseAuthentication();


//註冊區域路由
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


//最後註冊預設路由
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();

