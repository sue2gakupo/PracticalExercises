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
    public class DigitalProductsController : Controller
    {
        private readonly BondleDBContext2 _context;

        public DigitalProductsController(BondleDBContext2 context)
        {
            _context = context;
        }

        // GET: UserCreator/DigitalProducts
        public async Task<IActionResult> Index()
        {
            var bondleDBContext2 = _context.DigitalProduct.Include(d => d.Product);
            return View(await bondleDBContext2.ToListAsync());
        }

        // GET: UserCreator/DigitalProducts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var digitalProduct = await _context.DigitalProduct
                .Include(d => d.Product)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (digitalProduct == null)
            {
                return NotFound();
            }

            return View(digitalProduct);
        }

        // GET: UserCreator/DigitalProducts/Create
        public IActionResult Create()
        {
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID");
            return View();
        }

        // POST: UserCreator/DigitalProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,FileFormat,FileSize,FilePath,PreviewImagePath,DownloadLimit,ValidityDays,LicenseType,LicenseDescription")] DigitalProduct digitalProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(digitalProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", digitalProduct.ProductID);
            return View(digitalProduct);
        }

        // GET: UserCreator/DigitalProducts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var digitalProduct = await _context.DigitalProduct.FindAsync(id);
            if (digitalProduct == null)
            {
                return NotFound();
            }
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", digitalProduct.ProductID);
            return View(digitalProduct);
        }

        // POST: UserCreator/DigitalProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ProductID,FileFormat,FileSize,FilePath,PreviewImagePath,DownloadLimit,ValidityDays,LicenseType,LicenseDescription")] DigitalProduct digitalProduct)
        {
            if (id != digitalProduct.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(digitalProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DigitalProductExists(digitalProduct.ProductID))
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
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", digitalProduct.ProductID);
            return View(digitalProduct);
        }

        // GET: UserCreator/DigitalProducts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var digitalProduct = await _context.DigitalProduct
                .Include(d => d.Product)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (digitalProduct == null)
            {
                return NotFound();
            }

            return View(digitalProduct);
        }

        // POST: UserCreator/DigitalProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var digitalProduct = await _context.DigitalProduct.FindAsync(id);
            if (digitalProduct != null)
            {
                _context.DigitalProduct.Remove(digitalProduct);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DigitalProductExists(string id)
        {
            return _context.DigitalProduct.Any(e => e.ProductID == id);
        }
    }
}
