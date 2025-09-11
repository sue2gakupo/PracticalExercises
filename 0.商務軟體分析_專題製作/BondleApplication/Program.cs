using BondleApplication.Access.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BondleDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BondleDBConnection")));

builder.Services.AddDbContext<BondleDBContext2>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BondleDBConnection")));


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


//註冊區域路由
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


// 3. 最後註冊預設路由
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();

