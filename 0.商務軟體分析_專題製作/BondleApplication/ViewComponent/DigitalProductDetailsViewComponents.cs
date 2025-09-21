using BondleApplication.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BondleApplication.ViewComponentParts;

public class DigitalProductDetailsViewComponents : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var vm = new ProductCreateVM();
        vm.Variations.Add(new ProductVariationVM()); // 預設一組
        return View(vm); // Views/Shared/Components/DigitalProductDetailsVC/Default.cshtml
    }
}