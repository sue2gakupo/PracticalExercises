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
    public class ProductsController : Controller
    {
        private readonly BondleDBContext2 _context;
        private readonly IdGeneratorService _idGeneratorService;

        public ProductsController(BondleDBContext2 context, IdGeneratorService idGeneratorService)
        {
            _context = context;
            _idGeneratorService = idGeneratorService;
        }

        // GET: UserCreator/Products
        public async Task<IActionResult> Index()
        {
            var bondleDBContext2 = _context.Product.Include(p => p.Category).Include(p => p.Creator).Include(p => p.Series);
            return View(await bondleDBContext2.ToListAsync());
        }

        // GET: UserCreator/Products/Details/5
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

        // GET: UserCreator/Products/Create
        public IActionResult Create()
        {
            var product = new Product();
            ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryID");
            ViewData["CreatorID"] = new SelectList(_context.Creator, "CreatorID", "CreatorID");
            ViewData["SeriesID"] = new SelectList(_context.ProductSeries, "SeriesID", "SeriesID");
            return View();
        }

        // POST: UserCreator/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,ProductName,Description,ProductType,CategoryID,SeriesID,Price,Status,LaunchDate,OfflineDate,SortOrder,PurchaseCount,CreateDate,UpdateDate,CreatorID")] Product product)
        {
            if (ModelState.IsValid)
            {
                ViewBag.CategoryList = new SelectList(_context.Category
                    .AsNoTracking(), "CategoryID", "CategoryName");
              
                 

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryName"] = new SelectList(_context.Category, "CategoryName", "CategoryName", product.CategoryID);
            ViewData["CreatorID"] = new SelectList(_context.Creator, "CreatorID", "CreatorID", product.CreatorID);
            ViewData["SeriesID"] = new SelectList(_context.ProductSeries, "SeriesID", "SeriesID", product.SeriesID);

            return View(product);
        }

        // GET: UserCreator/Products/Edit/5
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

        // POST: UserCreator/Products/Edit/5
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

        // GET: UserCreator/Products/Delete/5
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

        // POST: UserCreator/Products/Delete/5
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
