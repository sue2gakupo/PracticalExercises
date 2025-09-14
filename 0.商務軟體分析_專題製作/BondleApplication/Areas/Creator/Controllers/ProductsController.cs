using BondleApplication.Access.Data;
using BondleApplication.Models;
using BondleApplication.Models.ViewModel.Creator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BondleApplication.Areas.Creator.Controllers
{
    [Area("Creator")]
    public class ProductsController : Controller
    {
        private readonly BondleDBContext2 _context;
        private readonly IWebHostEnvironment _env;

        public ProductsController(BondleDBContext2 context, IWebHostEnvironment env, IdGeneratorService idGenerator)
        {
            _context = context;
            _env = env;

        }



        // GET: Creator/Products
        public async Task<IActionResult> Index()
        {
            var bondleDBContext2 = _context.Product.Include(p => p.Category).Include(p => p.Creator).Include(p => p.Series);
            return View(await bondleDBContext2.ToListAsync());
        }

        // GET: Creator/Products/Details/5
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

        // GET: Creator/Products/Create
        public IActionResult Create()
        {
            // 下拉選單：分類、系列（僅此創作者）
            ViewData["CategoryList"] = new SelectList(_context.Category.AsNoTracking(), "CategoryID", "CategoryName");
            ViewData["SeriesList"] = new SelectList(_context.ProductSeries
                                                    .Where(s => s.CreatorID == "目前登入的 CreatorID")
                                                    .AsNoTracking(), "SeriesID", "SeriesName");

            return View(new ProductCreateViewModel());
        }

        // 修正 CS0161: 確保所有程式碼路徑都有傳回值
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["CategoryList"] = new SelectList(_context.Category.AsNoTracking(), "CategoryID", "CategoryName", model.CategoryID);
                ViewData["SeriesList"] = new SelectList(_context.ProductSeries
                    .Where(s => s.CreatorID == "目前登入的 CreatorID")
                    .AsNoTracking(), "SeriesID", "SeriesName", model.SeriesID);
                return View(model);
            }

            // 建立 Product
            var product = new Product
            {
                ProductID = Guid.NewGuid().ToString("N").Substring(0, 8),
                ProductName = model.ProductName,
                Description = model.Description,
                ProductType = model.ProductType,
                CategoryID = model.CategoryID,
                SeriesID = model.SeriesID,
                Price = model.Price,
                Status = 1, // 預設上架狀態
                CreateDate = DateTime.Now,
                CreatorID = "目前登入的 CreatorID"
            };
            _context.Add(product);
            await _context.SaveChangesAsync();

            // 如果是 DigitalProduct
            if (model.ProductType == 1 && model.Digital != null)
            {
                var uploadPath = Path.Combine(_env.WebRootPath, "uploads", "digital");
                Directory.CreateDirectory(uploadPath);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.Digital.UploadFile.FileName)}";
                var filePath = Path.Combine(uploadPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Digital.UploadFile.CopyToAsync(stream);
                }

                var digital = new DigitalProduct
                {
                    ProductID = product.ProductID,
                    FileFormat = Path.GetExtension(fileName).TrimStart('.'),
                    FileSize = model.Digital.UploadFile.Length,
                    FilePath = $"/uploads/digital/{fileName}",
                    PreviewImagePath = model.Digital.PreviewImage != null ? await SaveImage(model.Digital.PreviewImage) : null,
                    DownloadLimit = model.Digital.DownloadLimit,
                    ValidityDays = model.Digital.ValidityDays,
                    LicenseType = model.Digital.LicenseType,
                    LicenseDescription = model.Digital.LicenseDescription
                };

                _context.Add(digital);
                await _context.SaveChangesAsync();
            }

            // 如果是 PhysicalProduct
            if (model.ProductType == 2 && model.Physical != null)
            {
                var physical = new PhysicalProduct
                {
                    ProductID = product.ProductID,
                    Weight = model.Physical.Weight,
                    Length = model.Physical.Length,
                    Width = model.Physical.Width,
                    Height = model.Physical.Height,
                    DeliveryDays = model.Physical.DeliveryDays,
                    ShippingFeeType = model.Physical.ShippingFeeType,
                    FixedShippingFee = model.Physical.FixedShippingFee,
                    IsFragile = model.Physical.IsFragile,
                    PackagingNote = model.Physical.PackagingNote
                };

                _context.Add(physical);
                await _context.SaveChangesAsync();
            }

            // Variations 與 Images
            foreach (var variationVm in model.Variations)
            {
                var variation = new ProductVariations
                {
                    VariationID = Guid.NewGuid().ToString("N").Substring(0, 8),
                    ProductID = product.ProductID,
                    SKU = variationVm.SKU,
                    VariationName = variationVm.VariationName,
                    Color = variationVm.Color,
                    Size = variationVm.Size,
                    Material = variationVm.Material,
                    Edition = variationVm.Edition,
                    PriceDifference = variationVm.PriceDifference,
                    Stock = variationVm.Stock,
                    SafetyStock = variationVm.SafetyStock,
                    IsActive = variationVm.IsActive,
                    SortOrder = variationVm.SortOrder,
                    IsDefault = variationVm.IsDefault,
                    CreateDate = DateTime.Now
                };

                _context.Add(variation);
                await _context.SaveChangesAsync();

                // 儲存 Variation 的圖片
                var relatedImages = model.Images?.Where(imgVm => imgVm.VariationID == variationVm.VariationID).ToList();
                if (relatedImages != null && relatedImages.Count > 0)
                {
                    foreach (var imgVm in relatedImages)
                    {
                        var imgUrl = await SaveImage(imgVm.ImageFile);

                        var productImage = new ProductImages
                        {
                            ImageID = Guid.NewGuid().ToString("N").Substring(0, 8),
                            VariationID = variation.VariationID,
                            ImageUrl = imgUrl,
                            SortOrder = imgVm.SortOrder,
                            FileSize = imgVm.ImageFile.Length,
                            CreateDate = DateTime.Now
                        };

                        _context.Add(productImage);
                    }
                    await _context.SaveChangesAsync();
                }
            }

            // 所有路徑都需 return
            return RedirectToAction(nameof(Index));
        }
                
            

        // 抽取出來的圖片儲存方法
        private async Task<string> SaveImage(IFormFile file)
        {
            var uploadPath = Path.Combine(_env.WebRootPath, "uploads", "images");
            Directory.CreateDirectory(uploadPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/images/{fileName}";
        }
    



        // GET: Creator/Products/Edit/5
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

        // POST: Creator/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ProductID,ProductName,Description,ProductType,CategoryID,SeriesID,Price,Status,LaunchDate,OfflineDate,SortOrder,PurchaseCount,CreateDate,UpdateDate,CreatorID")] Models.Product product)
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



        private bool ProductExists(string id)
        {
            return _context.Product.Any(e => e.ProductID == id);
        }
    }
}
