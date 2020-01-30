using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using bank.Data;
using bank.Models;
using bank.Utilities;
using bankWithLogin.Attributes;

namespace bank.Controllers
{
    [AuthorizeCustomer]
    public class CustomerController : Controller
    {
        private readonly bankContext _context;

        // ReSharper disable once PossibleInvalidOperationException
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public CustomerController(bankContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            // Lazy loading.
            // The Customer.Accounts property will be lazy loaded upon demand.
            var customer = await _context.Customers.FindAsync(CustomerID);

            return View(customer);
        }

        public async Task<IActionResult> Deposit(int id) => View(await _context.Accounts.FindAsync(id));

        [HttpPost]
        public async Task<IActionResult> Deposit(int id, decimal amount)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (amount <= 0)
                ModelState.AddModelError(nameof(amount), "Amount must be positive.");
            if (amount.HasMoreThanTwoDecimalPlaces())
                ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
            if (!ModelState.IsValid)
            {
                ViewBag.Amount = amount;
                return View(account);
            }
            account.Balance += amount;
            account.Transactions.Add(
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    Amount = amount,
                    TransactionTimeUtc = DateTime.UtcNow
                });

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Withdraw(int id) => View(await _context.Accounts.FindAsync(id));

        [HttpPost]
        public async Task<IActionResult> Withdraw(int id, decimal amount)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (amount <= 0)
                ModelState.AddModelError(nameof(amount), "Amount must be positive.");
            if (amount.HasMoreThanTwoDecimalPlaces())
                ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
            if (!ModelState.IsValid)
            {
                ViewBag.Amount = amount;
                return View(account);
            }
            if (account.Balance < amount)
            {
                ModelState.AddModelError(nameof(amount), "Insufficient balance.");
            }
            account.Balance = account.Balance - amount;
            account.Transactions.Add(
                new Transaction
                {
                    TransactionType = TransactionType.Withdraw,
                    Amount = amount,
                    TransactionTimeUtc = DateTime.UtcNow
                });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> WTransfer1to2(int id) => View(await _context.Accounts.FindAsync(id));

        public async Task<IActionResult> Transfer1to2(int id, int AccountNumber, decimal amount, string Commment)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (amount <= 0)
                ModelState.AddModelError(nameof(amount), "Amount must be positive.");
            if (amount.HasMoreThanTwoDecimalPlaces())
                ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
            if (!ModelState.IsValid)
            {
                ViewBag.Amount = amount;
                return View(account);
            }
            if (account.Balance < amount)
            {
                ModelState.AddModelError(nameof(amount), "Insufficient balance.");
            }
            account.Balance = account.Balance - amount;
            account.Transactions.Add(
                new Transaction
                {
                    TransactionType = TransactionType.Transfer,
                    Amount = amount,
                    DestinationAccountNumber = AccountNumber,
                    Comment = Commment,
                    TransactionTimeUtc = DateTime.UtcNow
                }); 
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> MyProfile (int id) => View(await _context.Accounts.FindAsync(id));
        public async Task<IActionResult> MyProfile(int id, int AccountNumber, decimal amount, string Date) {
            var account = await _context.Accounts.FindAsync(id);
            return View(account);
        }
    }
}
