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
    public class ProductImagesController : Controller
    {
        private readonly BondleDBContext2 _context;
        private readonly IWebHostEnvironment _environment;


        public ProductImagesController(BondleDBContext2 context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: UserCreator/ProductImages
        public async Task<IActionResult> Index()
        {
            var bondleDBContext2 = _context.ProductImages.Include(p => p.Variation);
            return View(await bondleDBContext2.ToListAsync());
        }

        // GET: UserCreator/ProductImages/Details/5
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(string variationId, IFormFile imageFile, string imageCaption, string id)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsPath = Path.Combine(_environment.WebRootPath, "ProductImagesFolder", variationId);
                Directory.CreateDirectory(uploadsPath);

                // 產生自訂格式的 ImageID
                var lastImage = await _context.ProductImages
                    .OrderByDescending(m => m.ImageID == id)
                    .FirstOrDefaultAsync();

                int nextNumber = 1;
                if (lastImage != null && int.TryParse(lastImage.ImageID.Substring(2), out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
                string newImageID = $"IM{nextNumber.ToString("D6")}";


                // 以 ImageID 作為檔名，副檔名保留原始格式
                var fileExtension = Path.GetExtension(imageFile.FileName);
                var fileName = $"{newImageID}{fileExtension}";
                var filePath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }


                var productImage = new ProductImages
                {
                    ImageID = lastImage, // 自增式函數
                    ImageUrl = $"/ProductImagesFolder/{variationId}/{lastImage}",
                    ImageCaption = imageCaption,
                    FileSize = imageFile.Length,     
                    VariationID = variationId
                };

                _context.ProductImages.Add(productImage);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "圖片上傳成功" });
            }

            return Json(new { success = false, message = "請選擇要上傳的圖片" });
        }


        // GET: UserCreator/ProductImages/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var image = await _context.ProductImages.FindAsync(id);
            if (image != null)
            {
                // 刪除實際檔案
                var filePath = Path.Combine(_environment.WebRootPath, image.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                _context.ProductImages.Remove(image);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "圖片已刪除" });
            }
            return Json(new { success = false, message = "找不到該圖片" });


            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var productImages = await _context.ProductImages
            //    .Include(p => p.Variation)
            //    .FirstOrDefaultAsync(m => m.ImageID == id);
            //if (productImages == null)
            //{
            //    return NotFound();
            //}

            //return View(productImages);
        }

        // POST: UserCreator/ProductImages/Delete/5
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

        public IActionResult GetByVariationID(string variationId)
        {
            var images = _context.ProductImages
                .Where(i => i.VariationID == variationId)
                .ToList();

            ViewBag.VariationID = variationId;
            return PartialView("_ProductImagesForm", images);
        }




        //// GET: UserCreator/ProductImages/Create
        //public IActionResult Create()
        //{
        //    ViewData["VariationID"] = new SelectList(_context.ProductVariations, "VariationID", "VariationID");
        //    return View();
        //}

        //// POST: UserCreator/ProductImages/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ImageID,ImageUrl,SortOrder,ImageCaption,FileSize,CreateDate,UpdateDate,VariationID")] ProductImages productImages)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(productImages);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["VariationID"] = new SelectList(_context.ProductVariations, "VariationID", "VariationID", productImages.VariationID);
        //    return View(productImages);
        //}

        //// GET: UserCreator/ProductImages/Edit/5
        //public async Task<IActionResult> Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var productImages = await _context.ProductImages.FindAsync(id);
        //    if (productImages == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["VariationID"] = new SelectList(_context.ProductVariations, "VariationID", "VariationID", productImages.VariationID);
        //    return View(productImages);
        //}

        //// POST: UserCreator/ProductImages/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, [Bind("ImageID,ImageUrl,SortOrder,ImageCaption,FileSize,CreateDate,UpdateDate,VariationID")] ProductImages productImages)
        //{
        //    if (id != productImages.ImageID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(productImages);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ProductImagesExists(productImages.ImageID))
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
        //    ViewData["VariationID"] = new SelectList(_context.ProductVariations, "VariationID", "VariationID", productImages.VariationID);
        //    return View(productImages);
        //}



        private bool ProductImagesExists(string id)
        {
            return _context.ProductImages.Any(e => e.ImageID == id);
        }
    }
}
