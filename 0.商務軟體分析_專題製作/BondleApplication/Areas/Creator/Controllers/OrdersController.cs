using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BondleApplication.Access.Data;
using BondleApplication.Models;

namespace BondleApplication.Areas.Creator.Controllers
{
    [Area("Creator")]
    public class OrdersController : Controller
    {
        private readonly BondleDBContext2 _context;

        public OrdersController(BondleDBContext2 context)
        {
            _context = context;
        }

        // GET: Creator/Orders
        public async Task<IActionResult> Index()
        {
            var bondleDBContext2 = _context.Order.Include(o => o.Address).Include(o => o.Creator).Include(o => o.Supporter);
            return View(await bondleDBContext2.ToListAsync());
        }

        // GET: Creator/Orders/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Address)
                .Include(o => o.Creator)
                .Include(o => o.Supporter)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Creator/Orders/Create
        public IActionResult Create()
        {
            ViewData["AddressID"] = new SelectList(_context.ShippingAddress, "AddressID", "AddressID");
            ViewData["CreatorID"] = new SelectList(_context.Creator, "CreatorID", "CreatorID");
            ViewData["SupporterID"] = new SelectList(_context.Supporter, "SupporterID", "SupporterID");
            return View();
        }

        // POST: Creator/Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,TotalAmount,PaymentStatus,OrderStatus,ECPayMerchantID,RecipientName,RecipientPhone,ShippingAddress,CreateDate,PaidDate,ShippedDate,SupporterID,CreatorID,AddressID")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressID"] = new SelectList(_context.ShippingAddress, "AddressID", "AddressID", order.AddressID);
            ViewData["CreatorID"] = new SelectList(_context.Creator, "CreatorID", "CreatorID", order.CreatorID);
            ViewData["SupporterID"] = new SelectList(_context.Supporter, "SupporterID", "SupporterID", order.SupporterID);
            return View(order);
        }

        // GET: Creator/Orders/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["AddressID"] = new SelectList(_context.ShippingAddress, "AddressID", "AddressID", order.AddressID);
            ViewData["CreatorID"] = new SelectList(_context.Creator, "CreatorID", "CreatorID", order.CreatorID);
            ViewData["SupporterID"] = new SelectList(_context.Supporter, "SupporterID", "SupporterID", order.SupporterID);
            return View(order);
        }

        // POST: Creator/Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("OrderID,TotalAmount,PaymentStatus,OrderStatus,ECPayMerchantID,RecipientName,RecipientPhone,ShippingAddress,CreateDate,PaidDate,ShippedDate,SupporterID,CreatorID,AddressID")] Order order)
        {
            if (id != order.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderID))
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
            ViewData["AddressID"] = new SelectList(_context.ShippingAddress, "AddressID", "AddressID", order.AddressID);
            ViewData["CreatorID"] = new SelectList(_context.Creator, "CreatorID", "CreatorID", order.CreatorID);
            ViewData["SupporterID"] = new SelectList(_context.Supporter, "SupporterID", "SupporterID", order.SupporterID);
            return View(order);
        }

        // GET: Creator/Orders/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Address)
                .Include(o => o.Creator)
                .Include(o => o.Supporter)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Creator/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                _context.Order.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(string id)
        {
            return _context.Order.Any(e => e.OrderID == id);
        }
    }
}
