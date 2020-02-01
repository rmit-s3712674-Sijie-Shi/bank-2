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
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace bank.Controllers
{
    public class AccountController : Controller
    {
        private readonly bankContext _context;

        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public AccountController(bankContext context) => _context = context;
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Deposit(int id) => View(await _context.Accounts.FindAsync(id));

        [HttpPost]
        // Deposit from account
        public async Task<IActionResult> Deposit(int id, decimal amount)
        {
            var account = await _context.Accounts.FindAsync(id);
            // Check amount number
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

            return RedirectToAction("Index", "Customer");
        }
        //Withdraw
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
            return RedirectToAction("Index", "Customer");
        }
        public async Task<IActionResult> Transfer1to2(int id) => View(await _context.Accounts.FindAsync(id));
        [HttpPost]
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
            await Deposit(AccountNumber, amount);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Customer");
        }
        public async Task<IActionResult> BillPay(int id) => View(await _context.Accounts.FindAsync(id));
        [HttpPost]
        public async Task<IActionResult> BillPay(int id, int PayeeID, decimal amount, DateTime ScheduleDate, string Period)
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
            account.BillPay.Add(
                new BillPay
                {
                    PayeeID = PayeeID,
                    Amount = amount,
                    ScheduleDate = ScheduleDate,
                    Period = Period,
                    BillPayTimeUtc = DateTime.UtcNow
                });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Customer");
        }
        public void Timer(int ID, int PayeeID, decimal amount, DateTime ScheduleDate)
        {

            while (DateTime.UtcNow < ScheduleDate)
            {
                System.Threading.Thread.Sleep(600000);
                if (ScheduleDate <= DateTime.UtcNow)
                {
                    try
                    {
                        Deposit(PayeeID, amount);
                        Withdraw(ID, amount);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }
    }
}
