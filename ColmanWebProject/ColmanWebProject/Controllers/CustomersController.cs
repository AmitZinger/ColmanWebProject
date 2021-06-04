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
    }
}