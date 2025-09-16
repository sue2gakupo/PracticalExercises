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
    public class ProductVariationsController : Controller
    {
        private readonly BondleDBContext2 _context;

        public ProductVariationsController(BondleDBContext2 context)
        {
            _context = context;
        }

        // GET: UserCreator/ProductVariations
        public async Task<IActionResult> Index()
        {
            var bondleDBContext2 = _context.ProductVariations.Include(p => p.Product);
            return View(await bondleDBContext2.ToListAsync());
        }

        // GET: UserCreator/ProductVariations/Details/5
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

        // GET: UserCreator/ProductVariations/Create
        public IActionResult Create()
        {
            //ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID");
            return View();
        }

        // POST: UserCreator/ProductVariations/Create
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
                return Json(new { success = true, message = "商品款式已新增" });
            }

            var errors = ModelState.Values
          .SelectMany(v => v.Errors)
          .Select(e => e.ErrorMessage);
            return Json(new { success = false, errors = errors });

            //ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", productVariations.ProductID);
            //return View(productVariations);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProductVariations variation)
        {
            if (ModelState.IsValid)
            {
                _context.ProductVariations.Update(variation);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "商品款式已更新" });
            }

            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return Json(new { success = false, errors = errors });
        }





        //// GET: UserCreator/ProductVariations/Edit/5
        //public async Task<IActionResult> Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var productVariations = await _context.ProductVariations.FindAsync(id);
        //    if (productVariations == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", productVariations.ProductID);
        //    return View(productVariations);
        //}

        //// POST: UserCreator/ProductVariations/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, [Bind("VariationID,SKU,VariationName,Color,Size,Material,Edition,PriceDifference,Stock,SafetyStock,IsActive,SortOrder,IsDefault,CreateDate,UpdateDate,ProductID")] ProductVariations productVariations)
        //{
        //    if (id != productVariations.VariationID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(productVariations);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ProductVariationsExists(productVariations.VariationID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", productVariations.ProductID);
        //    return View(productVariations);
        //}

        // GET: UserCreator/ProductVariations/Delete/5
        public async Task<IActionResult> Delete(string id)
        {

            var variation = await _context.ProductVariations.FindAsync(id);
            if (variation != null)
            {
                _context.ProductVariations.Remove(variation);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "商品款式已刪除" });
            }
            return Json(new { success = false, message = "找不到該商品款式" });
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var productVariations = await _context.ProductVariations
            //    .Include(p => p.Product)
            //    .FirstOrDefaultAsync(m => m.VariationID == id);
            //if (productVariations == null)
            //{
            //    return NotFound();
            //}

            //return View(productVariations);
        }

        // POST: UserCreator/ProductVariations/Delete/5
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

        public IActionResult GetByProductId(string productId)
        {
            var variations = _context.ProductVariations
                .Where(v => v.ProductID == productId)
                .ToList();

            return PartialView("_ProductVariationsForm", variations);
        }

        public IActionResult GetNewVariationForm(string productId)
        {
            var variation = new ProductVariations { ProductID = productId };
            return PartialView("_ProductVariationItem", variation);
        }



       

        private bool ProductVariationsExists(string id)
        {
            return _context.ProductVariations.Any(e => e.VariationID == id);
        }
    }
}
