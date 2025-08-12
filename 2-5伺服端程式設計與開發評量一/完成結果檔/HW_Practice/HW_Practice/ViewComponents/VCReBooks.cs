using GuestBooks.Models;
using Microsoft.AspNetCore.Mvc;

namespace GuestBooks.ViewComponents
{
    public class VCReBooks : ViewComponent
    {
       private readonly GuestBookContext _context;

        public VCReBooks(GuestBookContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string bookID) 
        { 
        
        
        
        
        }



    }
}
