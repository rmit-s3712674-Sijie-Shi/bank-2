using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using bank.Data;
using bank.Models;
using SimpleHashing;
using System.Timers;
using System;

namespace bank.Controllers
{
    [Route("/bank/SecureLogin")]
    public class LoginController : Controller
    {
        private readonly bankContext _context;


        public LoginController(bankContext context) => _context = context;
        public IActionResult Login() => View();
        /*
        Timer timer = new Timer();
        public DateTime startTime, endTime;
        public void printa(object sender, ElapsedEventArgs e)
        {
            endTime = DateTime.Now;
            TimeSpan ts = endTime.Subtract(startTime);
            if (ts.TotalSeconds >= 10)
            {
                Logout();
            }
        }
        public void MyThread()
        {
            startTime = DateTime.Now;
            timer.Enabled = true;
            timer.Elapsed += new ElapsedEventHandler(printa);
            timer.Interval = 1000;
        }
        */
        [HttpPost]
        public async Task<IActionResult> Login(string loginID, string password)
        {
            var login = await _context.Logins.FindAsync(loginID);
            if (loginID == "admin" && password == "admin")
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (login == null || !PBKDF2.Verify(login.PasswordHash, password))
            {
                ModelState.AddModelError("LoginFailed", "Login failed, please try again.");
                login.LockNum++;
                if (login.LockNum == 3)
                {
                    login.Status = 1;
                }
                _context.SaveChanges();
                return View(new Login { LoginID = loginID });
            }
            else if (login.Status == 1)
            {
                // Login customer.
                HttpContext.Session.SetInt32(nameof(Customer.CustomerID), login.CustomerID);
                HttpContext.Session.SetString(nameof(Customer.Name), login.Customer.Name);
                login.LockNum = 0;
                _context.SaveChanges();
                return RedirectToAction("IndexLock", "Customer");
            }
            else 
            {
                // Login customer.
                HttpContext.Session.SetInt32(nameof(Customer.CustomerID), login.CustomerID);
                HttpContext.Session.SetString(nameof(Customer.Name), login.Customer.Name);
                login.LockNum = 0;
                _context.SaveChanges();
                return RedirectToAction("Index", "Customer");
            }
        }

        [Route("LogoutNow")]
        public IActionResult Logout()
        {
            // Logout customer.
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
