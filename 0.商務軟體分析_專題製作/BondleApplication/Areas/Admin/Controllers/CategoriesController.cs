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

   

        public async Task<IActionResult> Index()
        {
            var bondleDBContext2 = _context.Category;
            return View(await bondleDBContext2.ToListAsync());
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryID,CategoryName,Description,IconUrl,SortOrder,IsActive")] Category category)
        {

            if (ModelState.IsValid)
            {
                // 取得目前最大 CategoryID
                var lastId = await _context.Category
                    .OrderByDescending(c => c.CategoryID)
                    .Select(c => c.CategoryID)
                    .FirstOrDefaultAsync();

                int nextNumber = 1;
                if (!string.IsNullOrEmpty(lastId) && lastId.Length == 8 && lastId.StartsWith("CA"))
                {
                    if (int.TryParse(lastId.Substring(2), out int lastNumber))
                    {
                        nextNumber = lastNumber + 1;
                    }
                }

                category.CategoryID = $"CA{nextNumber.ToString("D6")}";

                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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
            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CategoryID,CategoryName,Description,IconUrl,SortOrder,IsActive")] Category category)
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
            return View(category);
        }

       

        private bool CategoryExists(string id)
        {
            return _context.Category.Any(e => e.CategoryID == id);
        }
    }
}
