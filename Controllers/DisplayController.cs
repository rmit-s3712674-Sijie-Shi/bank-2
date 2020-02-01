using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using bank.Data;
using bank.Models;
using bank.Utilities;
using bankWithLogin.Attributes;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace bank.Controllers
{
    public class DisplayController : Controller
    {
        private readonly bankContext _context;

        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public DisplayController(bankContext context) => _context = context;

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> TransactionDisplay(int id) => View(await _context.Accounts.FindAsync(id));
        [HttpPost]
        public async Task<IActionResult> TransactionDisplay()
        {
            var customer = await _context.Customers.FindAsync(CustomerID);
            var pagedList = await _context.Accounts.Where(x => x.CustomerID == customer.CustomerID).
                ToPagedListAsync(1, 4);
            return View(pagedList);
        }
        public async Task<IActionResult> BillPayDisplay(int id) => View(await _context.Accounts.FindAsync(id));
        [HttpPost]
        public async Task<IActionResult> BillPayDisplay()
        {
            var customer = await _context.Customers.FindAsync(CustomerID);
            return View("BillPayDisplay");
        }
    }
}
