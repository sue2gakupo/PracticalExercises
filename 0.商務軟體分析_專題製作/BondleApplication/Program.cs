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



//1.���U�S�����
//�h�h������
app.MapControllerRoute(
    name: "Categories",
    pattern: "Manager/{action}/{id?}", 
    defaults: new { Controller="Categories",action="Index"});

//�ۭq�R�A���|
app.MapControllerRoute(
    name: "about",
    pattern: "about-us",
    defaults: new { controller = "Home", action = "Privacy" });


// �a���h�ӰѼƪ�����
app.MapControllerRoute(
    name: "order",
    pattern: "order/{orderId}/{productId}",
    defaults: new { controller = "Order", action = "Detail" });


//2. �A���U�ϰ����
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
        name: "admin",
        pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}");

// 3. �̫���U�w�]����

app.MapControllerRoute(
    name: "aaa",
    pattern: "{area=User}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");





//app.MapControllerRoute(
//    name: "default2",
//    pattern: "{controller=Home}/{action=Index}/{id?}");


//app.MapControllerRoute(
//    name: "default",
//    pattern: "{area}/{controller=Home}/{action=Index}/{id?}");




app.Run();
