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
    public class ProductImagesController : Controller
    {
        private readonly BondleDBContext2 _context;

        public ProductImagesController(BondleDBContext2 context)
        {
            _context = context;
        }

        // GET: Creator/ProductImages
        public async Task<IActionResult> Index()
        {
            var bondleDBContext2 = _context.ProductImages.Include(p => p.Variation);
            return View(await bondleDBContext2.ToListAsync());
        }

        // GET: Creator/ProductImages/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productImages = await _context.ProductImages
                .Include(p => p.Variation)
                .FirstOrDefaultAsync(m => m.ImageID == id);
            if (productImages == null)
            {
                return NotFound();
            }

            return View(productImages);
        }

        // GET: Creator/ProductImages/Create
        public IActionResult Create()
        {
            ViewData["VariationID"] = new SelectList(_context.ProductVariations, "VariationID", "VariationID");
            return View();
        }

        // POST: Creator/ProductImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImageID,ImageUrl,SortOrder,ImageCaption,FileSize,CreateDate,UpdateDate,VariationID")] ProductImages productImages)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productImages);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VariationID"] = new SelectList(_context.ProductVariations, "VariationID", "VariationID", productImages.VariationID);
            return View(productImages);
        }

        // GET: Creator/ProductImages/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productImages = await _context.ProductImages.FindAsync(id);
            if (productImages == null)
            {
                return NotFound();
            }
            ViewData["VariationID"] = new SelectList(_context.ProductVariations, "VariationID", "VariationID", productImages.VariationID);
            return View(productImages);
        }

        // POST: Creator/ProductImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ImageID,ImageUrl,SortOrder,ImageCaption,FileSize,CreateDate,UpdateDate,VariationID")] ProductImages productImages)
        {
            if (id != productImages.ImageID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productImages);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductImagesExists(productImages.ImageID))
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
            ViewData["VariationID"] = new SelectList(_context.ProductVariations, "VariationID", "VariationID", productImages.VariationID);
            return View(productImages);
        }

        // GET: Creator/ProductImages/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productImages = await _context.ProductImages
                .Include(p => p.Variation)
                .FirstOrDefaultAsync(m => m.ImageID == id);
            if (productImages == null)
            {
                return NotFound();
            }

            return View(productImages);
        }

        // POST: Creator/ProductImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var productImages = await _context.ProductImages.FindAsync(id);
            if (productImages != null)
            {
                _context.ProductImages.Remove(productImages);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductImagesExists(string id)
        {
            return _context.ProductImages.Any(e => e.ImageID == id);
        }
    }
}
