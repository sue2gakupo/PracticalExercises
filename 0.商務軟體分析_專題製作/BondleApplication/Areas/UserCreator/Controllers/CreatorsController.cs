using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BondleApplication.Access.Data;
using BondleApplication.Models;

namespace BondleApplication.Areas.UserCreator.Controllers
{
    [Area("UserCreator")]
    public class CreatorsController : Controller
    {
        private readonly BondleDBContext2 _context;

        public CreatorsController(BondleDBContext2 context)
        {
            _context = context;
        }

        // GET: UserCreator/Creators
        public async Task<IActionResult> Index()
        {
            var bondleDBContext2 = _context.Creator.Include(c => c.Member);
            return View(await bondleDBContext2.ToListAsync());
        }

        // GET: UserCreator/Creators/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creator = await _context.Creator
                .Include(c => c.Member)
                .FirstOrDefaultAsync(m => m.CreatorID == id);
            if (creator == null)
            {
                return NotFound();
            }

            return View(creator);
        }

        // GET: UserCreator/Creators/Create
        public IActionResult Create()
        {
            ViewData["MemberID"] = new SelectList(_context.Member, "MemberID", "MemberID");
            return View();
        }

        // POST: UserCreator/Creators/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CreatorID,CreatorName,Biography,AvatarUrl,CoverUrl,VerificationStatus,BankAccount,BankCode,AccountHolderName,ECPayMerchantID,ECPayStatus,PlatformFeeRate,ApplyDate,VerificationDate,Status,CreateDate,UpdateDate,MemberID")] Creator creator)
        {
            if (ModelState.IsValid)
            {
                _context.Add(creator);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberID"] = new SelectList(_context.Member, "MemberID", "MemberID", creator.MemberID);
            return View(creator);
        }

        // GET: UserCreator/Creators/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creator = await _context.Creator.FindAsync(id);
            if (creator == null)
            {
                return NotFound();
            }
            ViewData["MemberID"] = new SelectList(_context.Member, "MemberID", "MemberID", creator.MemberID);
            return View(creator);
        }

        // POST: UserCreator/Creators/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CreatorID,CreatorName,Biography,AvatarUrl,CoverUrl,VerificationStatus,BankAccount,BankCode,AccountHolderName,ECPayMerchantID,ECPayStatus,PlatformFeeRate,ApplyDate,VerificationDate,Status,CreateDate,UpdateDate,MemberID")] Creator creator)
        {
            if (id != creator.CreatorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(creator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CreatorExists(creator.CreatorID))
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
            ViewData["MemberID"] = new SelectList(_context.Member, "MemberID", "MemberID", creator.MemberID);
            return View(creator);
        }

        // GET: UserCreator/Creators/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creator = await _context.Creator
                .Include(c => c.Member)
                .FirstOrDefaultAsync(m => m.CreatorID == id);
            if (creator == null)
            {
                return NotFound();
            }

            return View(creator);
        }

        // POST: UserCreator/Creators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var creator = await _context.Creator.FindAsync(id);
            if (creator != null)
            {
                _context.Creator.Remove(creator);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CreatorExists(string id)
        {
            return _context.Creator.Any(e => e.CreatorID == id);
        }
    }
}
