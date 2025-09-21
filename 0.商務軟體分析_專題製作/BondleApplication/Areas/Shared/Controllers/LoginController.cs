using BondleApplication.Access.Data;
using BondleApplication.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


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
            string testHash = ComputeSha256Hash("12345");
            ViewData["TestHash"] = testHash;
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
            var existingMember = await _context.Member
                    .FirstOrDefaultAsync(u => u.Email == member.Email && u.PasswordHash == ComputeSha256Hash(member.PasswordHash));

            if (existingMember == null)
            {
                ViewData["Error"] = "帳號或密碼錯誤";
                return View(member);
            }


            // 查詢身分
            var creator = await _context.Creator.FirstOrDefaultAsync(c => c.MemberID == existingMember.MemberID);
            var supporter = await _context.Supporter.FirstOrDefaultAsync(s => s.MemberID == existingMember.MemberID);


            // 登入
            if (creator != null && supporter != null)
            {
                // 雙重身分，導向選擇頁（可自訂 SelectRole 頁面）
                await SignInUser(existingMember, "DualRole");
                return RedirectToAction("SelectRole");
            }
            else if (creator != null)
            {
                await SignInUser(existingMember, "Creator");
                return RedirectToAction("Create", "Products", new { area = "UserCreator" });
            }
            else if (supporter != null)
            {
                await SignInUser(existingMember, "Supporter");
                return RedirectToAction("Index", "Products", new { area = "UserSupporter" });
            }
            else
            {
                ViewData["Error"] = "無效身分";
                return View(member);
            }
        }




        private async Task SignInUser(Member member, string role)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, member.MemberID),
        new Claim(ClaimTypes.Name, member.Name ?? ""),
        new Claim(ClaimTypes.Email, member.Email),
        new Claim(ClaimTypes.Role, role)
    };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Login");
        }

        private static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // 計算雜湊值
                byte[] bytes = sha256Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(rawData));

                // 將 byte 陣列轉換成十六進位字串
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }


        // 選擇身分頁面（如有雙重身分）
        [HttpGet]
        public IActionResult SelectRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectRole(string role)
        {
            if (string.IsNullOrEmpty(role))
                return RedirectToAction("Index");

            if (role == "Creator")
                return RedirectToAction("Create", "Products", new { area = "UserCreator" });
            else if (role == "Supporter")
                return RedirectToAction("Index", "Products", new { area = "UserSupporter" });

            return RedirectToAction("Index");
        }


    }
}
