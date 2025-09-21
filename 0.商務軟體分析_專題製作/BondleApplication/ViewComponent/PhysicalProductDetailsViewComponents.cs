using BondleApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace BondleApplication.ViewComponentParts;

public class PhysicalProductDetailsViewComponents : ViewComponent
{
    public IViewComponentResult Invoke() => View(); // Views/Shared/Components/PhysicalProductDetailsVC/Default.cshtml
}

