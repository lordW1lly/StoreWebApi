using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreWebApi.Models;

namespace StoreWebApi.Controllers
{
    public class CustomerOrdersController : Controller
    {
        private readonly StoreDBContext _context;

        public CustomerOrdersController(StoreDBContext context)
        {
            _context = context;
        }

        // GET: CustomerOrders
        public async Task<IActionResult> Index()
        {
            var storeDBContext = _context.CustomerOrder.Include(c => c.Customer).Include(c => c.OrderStatus);
            return View(await storeDBContext.ToListAsync());
        }

        // GET: CustomerOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerOrder = await _context.CustomerOrder
                .Include(c => c.Customer)
                .Include(c => c.OrderStatus)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerOrder == null)
            {
                return NotFound();
            }

            return View(customerOrder);
        }

        // GET: CustomerOrders/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "FirstName");
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatus, "Id", "Name");
            return View();
        }

        // POST: CustomerOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerId,Date,OrderStatusId,Amount")] CustomerOrder customerOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customerOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "FirstName", customerOrder.CustomerId);
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatus, "Id", "Name", customerOrder.OrderStatusId);
            return View(customerOrder);
        }

        // GET: CustomerOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerOrder = await _context.CustomerOrder.FindAsync(id);
            if (customerOrder == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "FirstName", customerOrder.CustomerId);
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatus, "Id", "Name", customerOrder.OrderStatusId);
            return View(customerOrder);
        }

        // POST: CustomerOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,Date,OrderStatusId,Amount")] CustomerOrder customerOrder)
        {
            if (id != customerOrder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerOrderExists(customerOrder.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "FirstName", customerOrder.CustomerId);
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatus, "Id", "Name", customerOrder.OrderStatusId);
            return View(customerOrder);
        }

        // GET: CustomerOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerOrder = await _context.CustomerOrder
                .Include(c => c.Customer)
                .Include(c => c.OrderStatus)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerOrder == null)
            {
                return NotFound();
            }

            return View(customerOrder);
        }

        // POST: CustomerOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customerOrder = await _context.CustomerOrder.FindAsync(id);
            _context.CustomerOrder.Remove(customerOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerOrderExists(int id)
        {
            return _context.CustomerOrder.Any(e => e.Id == id);
        }
    }
}
