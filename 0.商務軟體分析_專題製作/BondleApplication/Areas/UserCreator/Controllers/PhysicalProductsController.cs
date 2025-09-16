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
    public class PhysicalProductsController : Controller
    {
        private readonly BondleDBContext2 _context;

        public PhysicalProductsController(BondleDBContext2 context)
        {
            _context = context;
        }

        // GET: UserCreator/PhysicalProducts
        public async Task<IActionResult> Index()
        {
            var bondleDBContext2 = _context.PhysicalProduct.Include(p => p.Product);
            return View(await bondleDBContext2.ToListAsync());
        }

        // GET: UserCreator/PhysicalProducts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var physicalProduct = await _context.PhysicalProduct
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (physicalProduct == null)
            {
                return NotFound();
            }

            return View(physicalProduct);
        }

        // GET: UserCreator/PhysicalProducts/Create
        public IActionResult Create()
        {
            //ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID");
            return View();
        }

        // POST: UserCreator/PhysicalProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,Weight,Length,Width,Height,DeliveryDays,ShippingFeeType,FixedShippingFee,IsFragile,PackagingNote")] PhysicalProduct physicalProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(physicalProduct);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "實體產品資料已儲存" });
            }

            var errors = ModelState.Values
               .SelectMany(v => v.Errors)
               .Select(e => e.ErrorMessage);
            return Json(new { success = false, errors = errors });

            //ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", physicalProduct.ProductID);
            //return View(physicalProduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(PhysicalProduct physicalProduct)
        {
            if (ModelState.IsValid)
            {
                _context.PhysicalProduct.Update(physicalProduct);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "實體產品資料已更新" });
            }

            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return Json(new { success = false, errors = errors });
        }

        public IActionResult GetByProductId(string productId)
        {
            var physicalProduct = _context.PhysicalProduct
                .FirstOrDefault(p => p.ProductID == productId);

            if (physicalProduct == null)
                physicalProduct = new PhysicalProduct { ProductID = productId };

            return PartialView("_PhysicalProductForm", physicalProduct);
        }



        //// GET: UserCreator/PhysicalProducts/Edit/5
        //public async Task<IActionResult> Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var physicalProduct = await _context.PhysicalProduct.FindAsync(id);
        //    if (physicalProduct == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", physicalProduct.ProductID);
        //    return View(physicalProduct);
        //}

        //// POST: UserCreator/PhysicalProducts/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, [Bind("ProductID,Weight,Length,Width,Height,DeliveryDays,ShippingFeeType,FixedShippingFee,IsFragile,PackagingNote")] PhysicalProduct physicalProduct)
        //{
        //    if (id != physicalProduct.ProductID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(physicalProduct);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!PhysicalProductExists(physicalProduct.ProductID))
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
        //    ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", physicalProduct.ProductID);
        //    return View(physicalProduct);
        //}

        //// GET: UserCreator/PhysicalProducts/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var physicalProduct = await _context.PhysicalProduct
        //        .Include(p => p.Product)
        //        .FirstOrDefaultAsync(m => m.ProductID == id);
        //    if (physicalProduct == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(physicalProduct);
        //}

        //// POST: UserCreator/PhysicalProducts/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    var physicalProduct = await _context.PhysicalProduct.FindAsync(id);
        //    if (physicalProduct != null)
        //    {
        //        _context.PhysicalProduct.Remove(physicalProduct);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool PhysicalProductExists(string id)
        {
            return _context.PhysicalProduct.Any(e => e.ProductID == id);
        }
    }
}
