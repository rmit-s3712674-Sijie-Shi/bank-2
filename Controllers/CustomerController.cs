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

namespace bank.Controllers
{
    [AuthorizeCustomer]
    public class CustomerController : Controller
    {
        private readonly bankContext _context;

        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public CustomerController(bankContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            // Lazy loading.
            var customer = await _context.Customers.FindAsync(CustomerID);
            
            return View(customer);
        }
        // Check customer exist
        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerID == id);
        }
    }


}

