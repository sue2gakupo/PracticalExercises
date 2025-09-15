using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BondleApplication.Models.ViewModel.Creator
{
    public class VariationsViewModel
    {
        public string ProductID { get; set; }
        public List<ProductVariationsCreateVM> Variations { get; set; } = new();
    }
}

