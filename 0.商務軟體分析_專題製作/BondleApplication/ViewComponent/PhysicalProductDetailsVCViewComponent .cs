using BondleApplication.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BondleApplication.ViewComponentParts;

public class PhysicalProductDetailsVCViewComponents : ViewComponent
{

    public IViewComponentResult Invoke() => View(new ProductCreateVM()); // Views/Shared/Components/PhysicalProductDetailsVC/Default.cshtml
}
                                                                         

