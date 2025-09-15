using BondleApplication.Access.Data;
using BondleApplication.Models;
using BondleApplication.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BondleApplication.Areas.UserSupporter.Controllers
{
    [Area("UserSupporter")]
    public class ProductsController : Controller
    {
        private readonly BondleDBContext2 _context;

        public ProductsController(BondleDBContext2 context)
        {
            _context = context;
        }

  
        public async Task<IActionResult> Index()
        {
            try
            {
                // 先查商品及其款式與圖片
                var products = await _context.Product
                    .Where(p => p.Status == 1) // 只顯示上架中的商品
                    .Include(p => p.ProductVariations)
                    .ThenInclude(pv => pv.ProductImages)
                    .ToListAsync();

                // 轉成 ViewModel
                var productViewModels = products.Select(p =>
                {
                    // 選擇預設款式，如果沒有就選第一個有效款式
                    var productVariation = p.ProductVariations
                        .Where(pv => pv.IsActive && pv.IsDefault)
                        .FirstOrDefault() ?? p.ProductVariations
                        .Where(pv => pv.IsActive)
                        .FirstOrDefault();

                    if (productVariation == null)
                        return null; // 沒有有效款式就跳過

                    // 取款式的第一張圖片（依 SortOrder 排序）
                    var productImage = productVariation.ProductImages?
                        .OrderBy(pi => pi.SortOrder)
                        .FirstOrDefault();

                    return new VisitorProductViewModel
                    {
                        ProductID = p.ProductID,
                        ProductName = p.ProductName,
                        Price = p.Price,
                        PurchaseCount = p.PurchaseCount,
                        VariationID = productVariation.VariationID,
                        VariationName = productVariation.VariationName,
                        Stock = productVariation.Stock,
                        ImageUrl = productImage?.ImageUrl ?? "/NoImage/no_image.png",
                        SortOrder = productImage?.SortOrder ?? 0
                    };
                })
                .Where(vm => vm != null) // 過濾掉沒有有效款式的商品
                .ToList();

                return View(productViewModels);
            }
            catch (Exception ex)
            {
                // 記錄錯誤 (可取消註解並加入 Logger)
                // _logger.LogError(ex, "Error occurred while fetching products");

                // 返回空列表
                return View(new List<VisitorProductViewModel>());
            }
        }


// GET: Supporter/Products/Details/5
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

      

        private bool ProductExists(string id)
        {
            return _context.Product.Any(e => e.ProductID == id);
        }
    }
}
