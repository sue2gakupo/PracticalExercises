using GuestBooks.Models;
using HW_Practice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HW_Practice.Controllers
{
    public class HomeController : Controller
    {
        private readonly GuestBookContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, GuestBookContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _context.Book.Where(b => b.Photo != null).OrderByDescending(b => b.CreateDate).Take(5).ToListAsync();


            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

//任務一：請依據題題上的需求，以Code-First方式建立符合下列需求的模型類別、DbContext及資料庫，必須包含：
//1.	主文資料表(編號(P.K)、主題、發表內容、照片、照片類型、發表人、張貼時間)
//2.	回覆內容資料表(編號(P.K)、回覆內容、回覆人、回覆時間)
//3.	主文資料表與回覆內容資料表之間具關聯，一則主文可以有很多則回覆內容。
//4.	專案名稱、資料庫及資料表名稱可自訂。
//5.	各類別中須設計相對應屬性資料型態(Data Type)及驗證規則。
//6.	須製作初始化時之種子資料(Seed Data)若干筆。
//任務二：請設計前台(使用者端)功能，必須包含：
//1.	使用者可發表新文章，資料儲存於主文資料表
//2.	使用者發表文章時須填寫主題、發表內容、發表人欄位，而照片可有可無。
//3.	主畫面呈現各討論文章主題，並依發表時間的新到舊排列。
//4.	點選主題後可進入的該文章的討論畫面，可看到主文內容、照片及回覆內容。
//5.	使用者可在文章討論畫面回覆該文章。
//6.	回覆文章呈現時須依發表時間的新到舊排列。
//任務三：必須合於一般使用經驗及貼近實務的，包括資料存儲內容、規則及使用流程。

