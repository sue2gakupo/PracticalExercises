using BondleApplication.Access.Data;
using BondleApplication.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace BondleApplication.Areas.Shared.Controllers
{
    [Area("Shared")]
    public class LoginController : Controller
    {
        private readonly BondleDBContext2 _context;

        public LoginController(BondleDBContext2 context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Member member)
        {

            if (member == null || string.IsNullOrEmpty(member.Email) || string.IsNullOrEmpty(member.PasswordHash))
            {
                ViewData["Error"] = "請輸入帳號和密碼";
                return View(member);
            }

            // 查詢會員
            var user = await _context.Member
                .FirstOrDefaultAsync(u => u.Email == member.Email && u.PasswordHash == ComputeSha256Hash(member.PasswordHash));

            if (user == null)
            {
                ViewData["Error"] = "帳號或密碼錯誤，請重新輸入";
                return View(member);
            }

            // 判斷身分
            bool isCreator = await _context.Creator.AnyAsync(c => c.MemberID == user.MemberID);
            bool isSupporter = await _context.Supporter.AnyAsync(s => s.MemberID == user.MemberID);

            if (!isCreator && !isSupporter)
            {
                ViewData["Error"] = "無效身分";
                return View(member);
            }


            // 建立 Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.MemberID),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
            };

            if (isCreator && !isSupporter)
                claims.Add(new Claim(ClaimTypes.Role, "Creator"));
            else if (!isCreator && isSupporter)
                claims.Add(new Claim(ClaimTypes.Role, "Supporter"));
            else if (isCreator && isSupporter)
                claims.Add(new Claim("DualRole", "true")); // 雙重身分




            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // 導向不同頁面
            if (isCreator && !isSupporter)
                return RedirectToAction("Index", "Dashboard", new { area = "Creator" });
            else if (!isCreator && isSupporter)
                return RedirectToAction("Index", "Products", new { area = "Supporter" });
            else
                return RedirectToAction("SelectRole"); // 雙重身分選擇

        }

        [HttpGet]
        public IActionResult SelectRole()
        {
            return View(); // 提供選擇 Creator 或 Supporter 的按鈕
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectRole(string role)
        {
            var memberID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(memberID)) return RedirectToAction("Index");

            if (role == "Creator")
                return RedirectToAction("Index", "Dashboard", new { area = "Creator" });
            else if (role == "Supporter")
                return RedirectToAction("Index", "Products", new { area = "Supporter" });

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index","Login");
        }

        private string ComputeSha256Hash(string rawData)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(rawData);
                var hashBytes = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }


    }
}
