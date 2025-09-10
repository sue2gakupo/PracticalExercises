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




//1.註冊特殊路由
//多層次路由
//app.MapControllerRoute(
//    name: "Categories",
//    pattern: "Manager/{action=Index}/{id?}", 
//    defaults: new { area="Admin",contoller= "Categories" });


//自訂靜態路徑
app.MapControllerRoute(
    name: "about",
    pattern: "about-us",
    defaults: new { controller = "Home", action = "Privacy" });



// 帶有多個參數的路由
app.MapControllerRoute(
    name: "order",
    pattern: "order/{orderId}/{productId}",
    defaults: new { controller = "Order", action = "Detail" });




//2. 再註冊區域路由
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

//app.MapControllerRoute(
//        name: "admin",
//        pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}");

//app.MapControllerRoute(
//        name: "shared",
//        pattern: "Shared/{controller=Dashboard}/{action=Index}/{id?}");


// 3. 最後註冊預設路由

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");







app.Run();

