using GuestBooks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            //取得回覆留言資料
            var reBook = await _context.ReBook.Where(r => r.BookID== bookID).OrderByDescending(r => r.CreateDate).ToListAsync();

            return View(reBook);

        }



    }
}
