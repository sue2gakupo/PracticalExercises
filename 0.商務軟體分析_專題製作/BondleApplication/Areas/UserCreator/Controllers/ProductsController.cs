using BondleApplication.Access.Data;
using BondleApplication.Models;
using BondleApplication.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;


namespace BondleApplication.Areas.UserCreator.Controllers
{
    [Area("UserCreator")]
    //[Authorize]
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

        private void LoadCategories()
        {
            var categories = _context.Category
                .OrderBy(c => c.SortOrder)
                .Select(c => new { c.CategoryID, c.CategoryName })
                .ToList();

            ViewBag.Categories = new SelectList(categories, "CategoryID", "CategoryName");
        }

        private async Task<string?> GetCurrentCreatorIdAsync()
        {
            var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(memberId)) return null;

            return await _context.Creator
                .Where(c => c.MemberID == memberId)
                .Select(c => c.CreatorID)
                .FirstOrDefaultAsync();
        }


        // GET: UserCreator/Products
        public async Task<IActionResult> Index()
        {
            var q = _context.Product
                .Include(p => p.Category)
                .Include(p => p.Creator)
                .Include(p => p.Series);
            return View(await q.ToListAsync());
        }

        // GET: UserCreator/Products/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();

            var product = await _context.Product
                .Include(p => p.Category)
                .Include(p => p.Creator)
                .Include(p => p.Series)
                .FirstOrDefaultAsync(m => m.ProductID == id);

            if (product == null) return NotFound();
            return View(product);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            LoadCategories();

            var creatorId = await GetCurrentCreatorIdAsync();
            if (string.IsNullOrEmpty(creatorId))
            {
                return RedirectToAction("Login", "Login", new { area = "Shared" });
            }

            var vm = new ProductCreateVM
            {
                ProductType = 1,
                CreatorID = creatorId
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult LoadDetails(int type)
        {
            if (type == 1) return ViewComponent("PhysicalProductDetailsViewComponents");
            if (type == 2) return ViewComponent("DigitalProductDetailsViewComponents");
            return Content(string.Empty);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {
            LoadCategories();

            var creatorId = await GetCurrentCreatorIdAsync();
            if (string.IsNullOrEmpty(creatorId))
            {
                ModelState.AddModelError(string.Empty, "無法取得創作者資料，請重新登入。");
            }
            else
            {
                vm.CreatorID = creatorId;
            }

            if (!ModelState.IsValid) return View(vm);

            var now = DateTime.Now;
            // ProductID：Physical=PH******，Digital=DI******
            var productPrefix = vm.ProductType == 2 ? "DI" : "PH";
            var productId = await _idGeneratorService.GenerateIdAsync("Product", productPrefix);
            var product = new Product
            {
                ProductID = productId,
                ProductName = vm.ProductName,
                Price = vm.Price,
                ProductType = (byte)vm.ProductType,
                CategoryID = vm.CategoryID,
                CreatorID = vm.CreatorID,
                Status = 1,
                SortOrder = 0,
                PurchaseCount = 0,
                CreateDate = now,
                UpdateDate = now
            };
            _context.Product.Add(product);

            // 內部：確保預設變體（供主圖或無款式時使用）
            async Task<string> EnsureDefaultVariationAsync()
            {
                var existed = await _context.ProductVariations
                    .Where(v => v.ProductID == productId && v.IsDefault)
                    .Select(v => v.VariationID)
                    .FirstOrDefaultAsync();

                if (!string.IsNullOrEmpty(existed)) return existed;

                var defaultVid = await _idGeneratorService.GenerateIdAsync("ProductVariations", "PV");
                var defaultVar = new ProductVariations
                {
                    VariationID = defaultVid,
                    SKU = defaultVid,
                    VariationName = "Default",
                    PriceDifference = 0,
                    Stock = 0,
                    SafetyStock = 0,
                    IsActive = true,
                    SortOrder = 0,
                    IsDefault = true,
                    CreateDate = now,
                    UpdateDate = now,
                    ProductID = productId
                };
                _context.ProductVariations.Add(defaultVar);
                return defaultVid;
            }


            if (vm.ProductType == 1)
            {
                var phy = new PhysicalProduct
                {
                    ProductID = productId,
                    Weight = vm.Weight,
                    Length = vm.Length,
                    Width = vm.Width,
                    Height = vm.Height,
                    DeliveryDays = vm.DeliveryDays ?? 3,
                    ShippingFeeType = (byte?)(vm.ShippingFeeType ?? 1) ?? 1,
                    FixedShippingFee = (decimal?)vm.FixedShippingFee,
                    IsFragile = vm.IsFragile ?? false,
                    PackagingNote = vm.PackagingNote
                };
                _context.PhysicalProduct.Add(phy);
            }
            else if (vm.ProductType == 2)
            {
                string? digitalPath = null; long? digitalSize = null;
                if (vm.FilePath != null)
                {
                    (digitalPath, digitalSize) = await SaveFile(vm.FilePath, "DigitalFiles");
                }
                var digi = new DigitalProduct
                {
                    ProductID = productId,
                    FileFormat = vm.FileFormat ?? "N/A",
                    FileSize = digitalSize,
                    FilePath = digitalPath ?? string.Empty,
                    DownloadLimit = vm.DownloadLimit ?? 3,
                    ValidityDays = vm.ValidityDays,
                    LicenseType = vm.LicenseType,
                    LicenseDescription = vm.LicenseDescription
                };
                _context.DigitalProduct.Add(digi);


                if (vm.Variations?.Count > 0)
                {
                    foreach (var v in vm.Variations)
                    {
                        var vid = await _idGeneratorService.GenerateIdAsync("ProductVariations", "PV");
                        var variation = new ProductVariations
                        {
                            VariationID = vid,
                            SKU = vid,
                            VariationName = v.VariationName,
                            PriceDifference = v.PriceDifference,
                            Stock = v.Stock,
                            SafetyStock = 0,
                            IsActive = true,
                            SortOrder = 0,
                            IsDefault = false,
                            CreateDate = now,
                            UpdateDate = now,
                            ProductID = productId
                        };
                        _context.ProductVariations.Add(variation);

                        if (v.ImageFile != null)
                        {
                            var imageId = await _idGeneratorService.GenerateIdAsync("ProductImages", "IM");
                            var (imgUrl, imgBytes) = await SaveImageToVariationAsync(v.ImageFile, vid, imageId);

                            _context.ProductImages.Add(new ProductImages
                            {
                                ImageID = imageId,
                                ImageUrl = imgUrl,
                                SortOrder = 0,
                                FileSize = imgBytes,
                                CreateDate = now,
                                UpdateDate = now,
                                VariationID = vid
                            });

                        }
                    }
                }
            }

            // 主圖（不論實體或數位）
            // 主圖
            if (vm.MainImage != null)
            {
                var bindVid = await EnsureDefaultVariationAsync();
                var imageId = await _idGeneratorService.GenerateIdAsync("ProductImages", "IM");
                var (url, bytes) = await SaveImageToVariationAsync(vm.MainImage, bindVid, imageId);

                _context.ProductImages.Add(new ProductImages
                {
                    ImageID = imageId,
                    ImageUrl = url,
                    SortOrder = 0,
                    FileSize = bytes,
                    CreateDate = now,
                    UpdateDate = now,
                    VariationID = bindVid
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // —— Helpers ——
        private async Task<(string url, long bytes)> SaveImageToVariationAsync(IFormFile file, string variationId, string imageId)
        {
            var ext = Path.GetExtension(file.FileName);
            var folder = Path.Combine(_environment.WebRootPath, "ProductImagesFolder", variationId);
            Directory.CreateDirectory(folder);
            var full = Path.Combine(folder, $"{imageId}{ext}");
            using var fs = new FileStream(full, FileMode.Create);
            await file.CopyToAsync(fs);
            return ($"/ProductImagesFolder/{variationId}/{imageId}{ext}", file.Length);
        }


        private async Task<(string path, long bytes)> SaveFile(IFormFile file, string folder)
        {
            var root = Path.Combine(_environment.WebRootPath, folder);
            Directory.CreateDirectory(root);
            var name = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var full = Path.Combine(root, name);
            using var fs = new FileStream(full, FileMode.Create);
            await file.CopyToAsync(fs);
            return (full, file.Length);
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
