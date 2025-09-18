using BondleApplication.Access.Data;
using BondleApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BondleApplication.Areas.UserCreator.Controllers
{
    [Area("UserCreator")]
    public class ProductsController : Controller
    {
        private readonly BondleDBContext2 _context;
        private readonly IdGeneratorService _idGeneratorService;
        private readonly IWebHostEnvironment _environment;

        public ProductsController(BondleDBContext2 context, IdGeneratorService idGeneratorService, IWebHostEnvironment environment)
        {
            _context = context;
            _idGeneratorService = idGeneratorService;
            _environment = environment;
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
         

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryID", product.CategoryID);
            ViewData["CreatorID"] = new SelectList(_context.Creator, "CreatorID", "CreatorID", product.CreatorID);
            ViewData["SeriesID"] = new SelectList(_context.ProductSeries, "SeriesID", "SeriesID", product.SeriesID);

            return View(product);
        }

        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SelectCategory(Category category)
        {
            _context.Category.Add(category);
            await _context.SaveChangesAsync();
            return View();
        }




        public IActionResult Upload()
        {
            var product = new Product();
            ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryID");
            ViewData["CreatorID"] = new SelectList(_context.Creator, "CreatorID", "CreatorID");
            ViewData["SeriesID"] = new SelectList(_context.ProductSeries, "SeriesID", "SeriesID");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(string variationId, IFormFile imageFile, string imageCaption, string id)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsPath = Path.Combine(_environment.WebRootPath, "ProductImagesFolder", variationId);
                Directory.CreateDirectory(uploadsPath);

                // 商生自訂格式的 ImageID
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
