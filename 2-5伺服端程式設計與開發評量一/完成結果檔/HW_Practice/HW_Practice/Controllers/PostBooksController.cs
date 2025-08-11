﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GuestBooks.Models;

namespace GuestBooks.Controllers
{
    public class PostBooksController : Controller
    {
        private readonly GuestBookContext _context;

        public PostBooksController(GuestBookContext context)
        {
            _context = context;
        }

        // GET: PostBooks
        public async Task<IActionResult> Index()
        {
            var result = await _context.Book.OrderByDescending(b => b.CreateDate).ToListAsync();

            return View(result);
        }

        // GET: PostBooks/Details/5
        public async Task<IActionResult> Display(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.BookID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: PostBooks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PostBooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookID,Title,Description,Photo,Author,CreateDate")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }


        private bool BookExists(string id)
        {
            return _context.Book.Any(e => e.BookID == id);
        }
    }
}
