using BondleApplication.Access.Data;
using BondleApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BondleApplication.Areas.Shared.Controllers
{
    [Area("Shared")]
    public class RegisterController : Controller
    {
        private readonly BondleDBContext2 _context;

        public RegisterController(BondleDBContext2 context)
        {
            _context = context;
        }

        // GET: Shared/Register
        public async Task<IActionResult> Index()
        {
            return View(await _context.Member.ToListAsync());
        }

        // GET: Shared/Register/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.MemberID == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Shared/Register/Create
        public IActionResult Register()
        {
            return View();
        }

        // POST: Shared/Register/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("MemberID,Email,PasswordHash,GoogleUserID,Name,IsEmailVerified,CreateDate,LastLoginDate,Status")] Member member)
        {
            // 檢查 Email 是否已存在
            if (await _context.Member.AnyAsync(m => m.Email == member.Email))
            {
                ModelState.AddModelError("Email", "此電子信箱已被註冊");
                return View(member);
            }

            if (string.IsNullOrEmpty(member.Email) || string.IsNullOrEmpty(member.PasswordHash))
            {
                ViewData["Error"] = "請輸入完整資訊";
                return View(member);
            }



            // 建立新會員
            member.MemberID = Guid.NewGuid().ToString();
            // 密碼雜湊（範例：SHA256，可依實際需求調整）
            member.PasswordHash = ComputeSha256Hash(member.PasswordHash);
            member.IsEmailVerified = false;
            member.CreateDate = DateTime.Now;
            member.LastLoginDate = null;
            member.Status = 1;

            _context.Add(member);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");


            //if (ModelState.IsValid)
            //{
            //    _context.Add(member);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(member);
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

        // GET: Shared/Register/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Shared/Register/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MemberID,Email,PasswordHash,GoogleUserID,Name,IsEmailVerified,CreateDate,LastLoginDate,Status")] Member member)
        {
            if (id != member.MemberID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.MemberID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Shared/Register/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.MemberID == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Shared/Register/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var member = await _context.Member.FindAsync(id);
            if (member != null)
            {
                _context.Member.Remove(member);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(string id)
        {
            return _context.Member.Any(e => e.MemberID == id);
        }
    }
}
