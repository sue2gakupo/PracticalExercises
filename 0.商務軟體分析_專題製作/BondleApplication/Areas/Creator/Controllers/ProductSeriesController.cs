using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BondleApplication.Access.Data;
using BondleApplication.Models;

namespace BondleApplication.Areas.Creator.Controllers
{
    [Area("Creator")]
    public class ProductSeriesController : Controller
    {
        private readonly BondleDBContext2 _context;

        public ProductSeriesController(BondleDBContext2 context)
        {
            _context = context;
        }

        // GET: Creator/ProductSeries
        public async Task<IActionResult> Index()
        {
            var bondleDBContext2 = _context.ProductSeries.Include(p => p.Creator);
            return View(await bondleDBContext2.ToListAsync());
        }

        // GET: Creator/ProductSeries/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productSeries = await _context.ProductSeries
                .Include(p => p.Creator)
                .FirstOrDefaultAsync(m => m.SeriesID == id);
            if (productSeries == null)
            {
                return NotFound();
            }

            return View(productSeries);
        }

        // GET: Creator/ProductSeries/Create
        public IActionResult Create()
        {
            ViewData["CreatorID"] = new SelectList(_context.Creator, "CreatorID", "CreatorID");
            return View();
        }

        // POST: Creator/ProductSeries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SeriesID,SeriesName,Description,CoverImageUrl,Tags,SortOrder,IsPublic,CreateDate,UpdateDate,CreatorID")] ProductSeries productSeries)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productSeries);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatorID"] = new SelectList(_context.Creator, "CreatorID", "CreatorID", productSeries.CreatorID);
            return View(productSeries);
        }

        // GET: Creator/ProductSeries/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productSeries = await _context.ProductSeries.FindAsync(id);
            if (productSeries == null)
            {
                return NotFound();
            }
            ViewData["CreatorID"] = new SelectList(_context.Creator, "CreatorID", "CreatorID", productSeries.CreatorID);
            return View(productSeries);
        }

        // POST: Creator/ProductSeries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("SeriesID,SeriesName,Description,CoverImageUrl,Tags,SortOrder,IsPublic,CreateDate,UpdateDate,CreatorID")] ProductSeries productSeries)
        {
            if (id != productSeries.SeriesID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productSeries);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductSeriesExists(productSeries.SeriesID))
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
            ViewData["CreatorID"] = new SelectList(_context.Creator, "CreatorID", "CreatorID", productSeries.CreatorID);
            return View(productSeries);
        }

        // GET: Creator/ProductSeries/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productSeries = await _context.ProductSeries
                .Include(p => p.Creator)
                .FirstOrDefaultAsync(m => m.SeriesID == id);
            if (productSeries == null)
            {
                return NotFound();
            }

            return View(productSeries);
        }

        // POST: Creator/ProductSeries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var productSeries = await _context.ProductSeries.FindAsync(id);
            if (productSeries != null)
            {
                _context.ProductSeries.Remove(productSeries);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductSeriesExists(string id)
        {
            return _context.ProductSeries.Any(e => e.SeriesID == id);
        }
    }
}
