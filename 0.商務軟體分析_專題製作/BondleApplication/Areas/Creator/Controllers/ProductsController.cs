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
    public class ProductsController : Controller
    {
        private readonly BondleDBContext2 _context;

        public ProductsController(BondleDBContext2 context)
        {
            _context = context;
        }

        // GET: Creator/Products
        public async Task<IActionResult> Index()
        {
            var bondleDBContext2 = _context.Product.Include(p => p.Category).Include(p => p.Creator).Include(p => p.Series);
            return View(await bondleDBContext2.ToListAsync());
        }

        // GET: Creator/Products/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .Include(p => p.Creator)
                .Include(p => p.Series)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Creator/Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryID");
            ViewData["CreatorID"] = new SelectList(_context.Creator, "CreatorID", "CreatorID");
            ViewData["SeriesID"] = new SelectList(_context.ProductSeries, "SeriesID", "SeriesID");
            return View();
        }

        // POST: Creator/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,ProductName,Description,ProductType,CategoryID,SeriesID,Price,Status,LaunchDate,OfflineDate,SortOrder,PurchaseCount,CreateDate,UpdateDate,CreatorID")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryID", product.CategoryID);
            ViewData["CreatorID"] = new SelectList(_context.Creator, "CreatorID", "CreatorID", product.CreatorID);
            ViewData["SeriesID"] = new SelectList(_context.ProductSeries, "SeriesID", "SeriesID", product.SeriesID);
            return View(product);
        }

        // GET: Creator/Products/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryID", product.CategoryID);
            ViewData["CreatorID"] = new SelectList(_context.Creator, "CreatorID", "CreatorID", product.CreatorID);
            ViewData["SeriesID"] = new SelectList(_context.ProductSeries, "SeriesID", "SeriesID", product.SeriesID);
            return View(product);
        }

        // POST: Creator/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ProductID,ProductName,Description,ProductType,CategoryID,SeriesID,Price,Status,LaunchDate,OfflineDate,SortOrder,PurchaseCount,CreateDate,UpdateDate,CreatorID")] Product product)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductID))
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
            ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryID", product.CategoryID);
            ViewData["CreatorID"] = new SelectList(_context.Creator, "CreatorID", "CreatorID", product.CreatorID);
            ViewData["SeriesID"] = new SelectList(_context.ProductSeries, "SeriesID", "SeriesID", product.SeriesID);
            return View(product);
        }

        // GET: Creator/Products/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .Include(p => p.Creator)
                .Include(p => p.Series)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Creator/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(string id)
        {
            return _context.Product.Any(e => e.ProductID == id);
        }
    }
}
