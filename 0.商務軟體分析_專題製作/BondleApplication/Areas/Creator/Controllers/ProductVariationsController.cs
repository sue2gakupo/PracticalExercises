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
    public class ProductVariationsController : Controller
    {
        private readonly BondleDBContext2 _context;

        public ProductVariationsController(BondleDBContext2 context)
        {
            _context = context;
        }

        // GET: Creator/ProductVariations
        public async Task<IActionResult> Index()
        {
            var bondleDBContext2 = _context.ProductVariations.Include(p => p.Product);
            return View(await bondleDBContext2.ToListAsync());
        }

        // GET: Creator/ProductVariations/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productVariations = await _context.ProductVariations
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.VariationID == id);
            if (productVariations == null)
            {
                return NotFound();
            }

            return View(productVariations);
        }

        // GET: Creator/ProductVariations/Create
        public IActionResult Create()
        {
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID");
            return View();
        }

        // POST: Creator/ProductVariations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VariationID,SKU,VariationName,Color,Size,Material,Edition,PriceDifference,Stock,SafetyStock,IsActive,SortOrder,IsDefault,CreateDate,UpdateDate,ProductID")] ProductVariations productVariations)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productVariations);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", productVariations.ProductID);
            return View(productVariations);
        }

        // GET: Creator/ProductVariations/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productVariations = await _context.ProductVariations.FindAsync(id);
            if (productVariations == null)
            {
                return NotFound();
            }
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", productVariations.ProductID);
            return View(productVariations);
        }

        // POST: Creator/ProductVariations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("VariationID,SKU,VariationName,Color,Size,Material,Edition,PriceDifference,Stock,SafetyStock,IsActive,SortOrder,IsDefault,CreateDate,UpdateDate,ProductID")] ProductVariations productVariations)
        {
            if (id != productVariations.VariationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productVariations);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductVariationsExists(productVariations.VariationID))
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
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", productVariations.ProductID);
            return View(productVariations);
        }

        // GET: Creator/ProductVariations/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productVariations = await _context.ProductVariations
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.VariationID == id);
            if (productVariations == null)
            {
                return NotFound();
            }

            return View(productVariations);
        }

        // POST: Creator/ProductVariations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var productVariations = await _context.ProductVariations.FindAsync(id);
            if (productVariations != null)
            {
                _context.ProductVariations.Remove(productVariations);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductVariationsExists(string id)
        {
            return _context.ProductVariations.Any(e => e.VariationID == id);
        }
    }
}
