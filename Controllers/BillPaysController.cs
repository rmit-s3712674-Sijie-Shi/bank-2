using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bank.Data;
using bank.Models;

namespace bank.Controllers
{
    public class BillPaysController : Controller
    {
        private readonly bankContext _context;

        public BillPaysController(bankContext context)
        {
            _context = context;
        }

        // GET: BillPays
        public async Task<IActionResult> Index()
        {
            return View(await _context.BillPay.ToListAsync());
        }

        // GET: BillPays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billPay = await _context.BillPay
                .FirstOrDefaultAsync(m => m.BillPayID == id);
            if (billPay == null)
            {
                return NotFound();
            }

            return View(billPay);
        }

        // GET: BillPays/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BillPays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BillPayID,Period,AccountNumber1,PayeeID,Amount,ScheduleDate,BillPayTimeUtc")] BillPay billPay)
        {
            if (ModelState.IsValid)
            {
                _context.Add(billPay);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(billPay);
        }

        // GET: BillPays/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billPay = await _context.BillPay.FindAsync(id);
            if (billPay == null)
            {
                return NotFound();
            }
            return View(billPay);
        }

        // POST: BillPays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BillPayID,Period,AccountNumber1,PayeeID,Amount,ScheduleDate,BillPayTimeUtc")] BillPay billPay)
        {
            if (id != billPay.BillPayID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(billPay);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillPayExists(billPay.BillPayID))
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
            return View(billPay);
        }

        // GET: BillPays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billPay = await _context.BillPay
                .FirstOrDefaultAsync(m => m.BillPayID == id);
            if (billPay == null)
            {
                return NotFound();
            }

            return View(billPay);
        }

        // POST: BillPays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var billPay = await _context.BillPay.FindAsync(id);
            _context.BillPay.Remove(billPay);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BillPayExists(int id)
        {
            return _context.BillPay.Any(e => e.BillPayID == id);
        }
    }
}
