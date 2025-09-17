using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BondleApplication.Access.Data;
using BondleApplication.Models;

namespace BondleApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
     

    public class CategoriesController : Controller
    {
        private readonly BondleDBContext2 _context;

        public CategoriesController(BondleDBContext2 context)
        {
            _context = context;
        }

   
        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var bondleDBContext2 = _context.Category.Include(c => c.ParentCategory);
            return View(await bondleDBContext2.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(string id,string CAName)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.CategoryID == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            ViewData["ParentCategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryID");
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryID,CategoryName,Description,ParentCategoryID,IconUrl,SortOrder,IsActive")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return View();
            }
            ViewData["ParentCategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryID", category.ParentCategoryID);
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewData["ParentCategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryID", category.ParentCategoryID);
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CategoryID,CategoryName,Description,ParentCategoryID,IconUrl,SortOrder,IsActive")] Category category)
        {
            if (id != category.CategoryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryID))
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
            ViewData["ParentCategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryID", category.ParentCategoryID);
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.CategoryID == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category != null)
            {
                _context.Category.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(string id)
        {
            return _context.Category.Any(e => e.CategoryID == id);
        }
    }
}
