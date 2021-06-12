using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ColmanWebProject.Data;
using ColmanWebProject.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace ColmanWebProject.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ColmanWebProjectContext _context;

        public CustomersController(ColmanWebProjectContext context)
        {
            _context = context;
        }


        // GET: Customers/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Customers/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,Email,Password,Name,LastName,Phone")] Customer customer)
        {
            if (ModelState.IsValid)
            {

                var checkExist = _context.Customer.FirstOrDefault(c => c.Email == customer.Email);

                if (checkExist == null)
                {
                    _context.Add(customer);
                    await _context.SaveChangesAsync();

                    var customerInfo = _context.Customer.FirstOrDefault(c => c.Email == customer.Email
                    && c.Password == customer.Password);
                    Signin(customerInfo);

                    return RedirectToAction(nameof(Index), "Home");
                }
                else
                {
                    ViewData["Error"] = "Email already exist; cannot register this user.";
                }
            }
            return View(customer);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        // GET: Users/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Users/Login
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Id,Email,Password")] Customer customer)
        {
            bool validEmail = ModelState["Email"].ValidationState.ToString().Equals("Valid");
            bool validPassword = ModelState["Password"].ValidationState.ToString().Equals("Valid");

            if (validEmail && validPassword)
            {
                var customerExist = from c in _context.Customer
                                    where c.Email == customer.Email && c.Password == customer.Password
                                    select c;

                if (customerExist.Count() > 0)
                {

                    Signin(customerExist.First());

                    return RedirectToAction(nameof(Index), "Home");
                }
                else
                {
                    ViewData["Error"] = "Email and/or password are incorrect.";
                }
            }
            return View(customer);
        }

        private async void Signin(Customer account)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, account.Email),
                    new Claim(ClaimTypes.Role, account.Role.ToString()),
                };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        public async Task<IActionResult> Logout()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customer.ToListAsync());
        }

        //// GET: Customers/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var customer = await _context.Customer
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(customer);
        //}

        //// GET: Customers/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Customers/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Email,Password,Name,LastName,Phone,Role")] Customer customer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(customer);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(customer);
        //}

        // GET: Customers/EditRole/5
        [Authorize]
        public async Task<IActionResult> EditRole(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.FindAsync(id);

            var identity = (System.Security.Claims.ClaimsIdentity)HttpContext.User.Identity;
            string signUserRole = null;
            if (identity.Claims.Count() > 0)
            {
                signUserRole = identity.Claims.FirstOrDefault(c => c.Type.Contains("role")).Value;

                if (customer == null || (customer != null && signUserRole != UserType.Admin.ToString()))
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
            return View(customer);
        }

        // GET: Customers/Edit/email
        [Authorize]
        [HttpGet]
        [Route("Customers/Edit/{Email}")]
        public async Task<IActionResult> Edit(string Email)
        {
            if (Email == null )
            {
                return NotFound();
            }

            var customerExist = from c in _context.Customer
                                where c.Email == Email 
                                select c;
            var customer = customerExist.First();
            
            var identity = (System.Security.Claims.ClaimsIdentity)HttpContext.User.Identity;
            string email = null;
            if (identity.Claims.Count() > 0)
            {
                email = identity.Claims.FirstOrDefault(c => c.Type.Contains("email")).Value;

                if (customer == null || (customer != null && customer.Email != email))
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
               
            return View(customer);
        }

            // POST: Customers/Edit/5
            // To protect from overposting attacks, enable the specific properties you want to bind to, for 
            // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Password,Name,LastName,Phone,Role")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), "Home");
            }
            return View(customer);
        }

        // POST: Customers/EditRole/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> EditRole(int id, [Bind("Id,Email,Password,Name,LastName,Phone,Role")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                var password = from c in _context.Customer
                               where c.Id== id
                               select c.Password;
                customer.Password = password.First();
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            return View(customer);
        }
        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customer.FindAsync(id);
            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeToAdmin(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            customer.Role = UserType.Admin;

            try
            {
                _context.Update(customer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(customer.Id))
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
            
    }
}
