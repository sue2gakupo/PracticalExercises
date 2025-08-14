using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GuestBooks.Models;

namespace GuestBooks.Controllers
{
    public class RePostBooksController : Controller
    {
        private readonly GuestBookContext _context;

        public RePostBooksController(GuestBookContext context)
        {
            _context = context;
        }

       

        public IActionResult Create(string BookID)
        {
            ViewData["BookID"] = BookID;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReBookID,Description,Author,CreateDate,BookID")] ReBook reBook)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reBook);
                await _context.SaveChangesAsync();
                return Json(reBook);
            }
          
            return Json(reBook);
        }

        public IActionResult GetReBookByViewComponent(string BookID)
        {

            return ViewComponent("VCReBooks", new { bookID = BookID });

        }

    }
}
