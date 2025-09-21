using BondleApplication.Access.Data;
using BondleApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BondleApplication.Areas.UserCreator.Controllers
{
    [Area("UserCreator")]
    public class ProductImagesController : Controller
    {
        private readonly BondleDBContext2 _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IdGeneratorService _idGeneratorService;


        public ProductImagesController(BondleDBContext2 context, IWebHostEnvironment environment, IdGeneratorService idGeneratorService )
        {
            _context = context;
            _environment = environment;
            _idGeneratorService = idGeneratorService;
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


        // GET: UserCreator/ProductImages/Create
        public IActionResult Create()
        {
            ViewData["VariationID"] = new SelectList(_context.ProductVariations, "VariationID", "VariationID");
            return View();
        }

        // POST: UserCreator/ProductImages/Create
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

        [HttpPost]
        public async Task<IActionResult> Upload(string variationId, IFormFile file, int sortOrder = 0, string? caption = null)
        {
            if (string.IsNullOrWhiteSpace(variationId) || file == null || file.Length == 0) return BadRequest();

            // 確認變體存在
            var exists = await _context.ProductVariations.AnyAsync(v => v.VariationID == variationId);
            if (!exists) return NotFound("Variation not found.");

            var imageId = await _idGeneratorService.GenerateIdAsync("ProductImages", "IM");
            var ext = Path.GetExtension(file.FileName);
            var folder = Path.Combine(_environment.WebRootPath, "ProductImagesFolder", variationId);
            Directory.CreateDirectory(folder);
            var full = Path.Combine(folder, $"{imageId}{ext}");
            using (var fs = new FileStream(full, FileMode.Create)) { await file.CopyToAsync(fs); }

            var url = $"/ProductImagesFolder/{variationId}/{imageId}{ext}";
            var now = DateTime.UtcNow;

            _context.ProductImages.Add(new ProductImages
            {
                ImageID = imageId,
                ImageUrl = url,
                ImageCaption = caption,
                SortOrder = sortOrder,
                FileSize = file.Length,
                CreateDate = now,
                UpdateDate = now,
                VariationID = variationId
            });
            await _context.SaveChangesAsync();
            return Json(new { success = true, imageId, url });
        }

       

        [HttpPost]
        public async Task<IActionResult> Delete(string imageId)
        {
            if (string.IsNullOrWhiteSpace(imageId)) return BadRequest();
            var img = await _context.ProductImages.FirstOrDefaultAsync(i => i.ImageID == imageId);
            if (img == null) return NotFound();

            // 刪實體檔案
            try
            {
                var path = Path.Combine(_environment.WebRootPath, "ProductImagesFolder", img.VariationID, Path.GetFileName(img.ImageUrl));
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            }
            catch { /* 忽略檔案不存在 */ }

            _context.ProductImages.Remove(img);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        

        // GET: UserCreator/ProductImages/Edit/5
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




        private bool ProductImagesExists(string id)
        {
            return _context.ProductImages.Any(e => e.ImageID == id);
        }
    }
}


//[HttpPost]
//[ValidateAntiForgeryToken]
//public async Task<IActionResult> Upload(string variationId, IFormFile imageFile, string imageCaption, string id)
//{
//    if (imageFile != null && imageFile.Length > 0)
//    {
//        var uploadsPath = Path.Combine(_environment.WebRootPath, "ProductImagesFolder", variationId);
//        Directory.CreateDirectory(uploadsPath);

//        // 商生自訂格式的 ImageID
//        var lastImage = await _context.ProductImages
//            .OrderByDescending(m => m.ImageID == id)
//            .FirstOrDefaultAsync();

//        int nextNumber = 1;
//        if (lastImage != null && int.TryParse(lastImage.ImageID.Substring(2), out int lastNumber))
//        {
//            nextNumber = lastNumber + 1;
//        }
//        string newImageID = $"IM{nextNumber.ToString("D6")}";


//        // 以 ImageID 作為檔名，副檔名保留原始格式
//        var fileExtension = Path.GetExtension(imageFile.FileName);
//        var fileName = $"{newImageID}{fileExtension}";
//        var filePath = Path.Combine(uploadsPath, fileName);

//        using (var stream = new FileStream(filePath, FileMode.Create))
//        {
//            await imageFile.CopyToAsync(stream);
//        }


//        var productImage = new ProductImages
//        {
//            ImageID = lastImage, // 自增式函數
//            ImageUrl = $"/ProductImagesFolder/{variationId}/{lastImage}",
//            ImageCaption = imageCaption,
//            FileSize = imageFile.Length,     
//            VariationID = variationId
//        };

//        _context.ProductImages.Add(productImage);
//        await _context.SaveChangesAsync();

//        return Json(new { success = true, message = "圖片上傳成功" });
//    }

//    return Json(new { success = false, message = "請選擇要上傳的圖片" });
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




// POST: UserCreator/ProductImages/Delete/5
//[HttpPost, ActionName("Delete")]
//[ValidateAntiForgeryToken]
//public async Task<IActionResult> DeleteConfirmed(string id)
//{
//    var productImages = await _context.ProductImages.FindAsync(id);
//    if (productImages != null)
//    {
//        _context.ProductImages.Remove(productImages);
//    }

//    await _context.SaveChangesAsync();
//    return RedirectToAction(nameof(Index));
//}

//public IActionResult GetByVariationID(string variationId)
//{
//    var images = _context.ProductImages
//        .Where(i => i.VariationID == variationId)
//        .ToList();

//    ViewBag.VariationID = variationId;
//    return PartialView("_ProductImagesForm", images);
//}
