using BondleApplication.Access.Data;
using BondleApplication.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BondleApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
     
    public class CategoriesController : Controller
    {
        private readonly BondleDBContext2 _context;
        private readonly IWebHostEnvironment _env;

        public CategoriesController(BondleDBContext2 context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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
        public async Task<IActionResult> Create(
        [Bind("CategoryName,Description,IsActive,SortOrder")] Category category)
        {
            if (!ModelState.IsValid) return View(category);

            // 產生 CategoryID：CA000001 起跳
            var lastId = await _context.Category
                .OrderByDescending(c => c.CategoryID)
                .Select(c => c.CategoryID)
                .FirstOrDefaultAsync();

            var next = 1;
            if (!string.IsNullOrEmpty(lastId) && lastId.StartsWith("CA") &&
                int.TryParse(lastId.Substring(2), out var n)) next = n + 1;

            category.CategoryID = $"CA{next:D6}";

            _context.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

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
