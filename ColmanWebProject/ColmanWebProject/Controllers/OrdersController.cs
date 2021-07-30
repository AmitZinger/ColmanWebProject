using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ColmanWebProject.Data;
using ColmanWebProject.Models;
using Microsoft.AspNetCore.Authorization;

namespace ColmanWebProject.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ColmanWebProjectContext _context;

        public OrdersController(ColmanWebProjectContext context)
        {
            _context = context;
        }

        // GET: Orders
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var colmanWebProjectContext = _context.Order.Include(o => o.Customer);
            ViewData["ShowGraph"] = true;
            return View(await colmanWebProjectContext.ToListAsync());
        }

        public async Task<IActionResult> MyOrders()
        {
            var identity = (System.Security.Claims.ClaimsIdentity)HttpContext.User.Identity;
            string signUserEmail;
            ViewData["ShowGraph"] = false;
            if (identity.Claims.Count() > 0)
            {
                signUserEmail = identity.Claims.FirstOrDefault(c => c.Type.Contains("email")).Value;
                var orders = _context.Order.Include(o => o.Customer).Where(oc => oc.Customer.Email.Equals(signUserEmail));
                return View(nameof(Index), await orders.ToListAsync());
            }

            return NotFound();

        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.ProductsOrder
                .Include(po => po.Product)
                .Include(po => po.Order)
                .ThenInclude(o => o.Customer)
                .Where(po => po.OrderId == id)
                .ToListAsync();

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create(int price)
        {
            ViewData["Price"] = price;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ShippingAddressCity,ShippingAddressStreet,ShippingAddressHomeNum,Date,CustomerId")] Order order)
        {
            var identity = (System.Security.Claims.ClaimsIdentity)HttpContext.User.Identity;
            string signUserEmail;
            if (identity.Claims.Count() > 0)
            {
                signUserEmail = identity.Claims.FirstOrDefault(c => c.Type.Contains("email")).Value;

                var customerInfo = _context.Customer.FirstOrDefault(c => c.Email == signUserEmail);
                order.Customer = customerInfo;
                order.CustomerId = customerInfo.Id;

                var productCart = await _context.ProductsCart
                .Include(pw => pw.Product)
                .Where(pw => pw.CartId == customerInfo.CartId).ToListAsync();

                List<Product> productsInOrder = new List<Product>();
                if (ModelState.IsValid)
                {
                    foreach (ProductsCart pc in productCart)
                    {
                        var product = await _context.Product.FirstOrDefaultAsync(p => p.Id == pc.ProductId);
                        if (pc.Quantity > product.Stock)
                        {
                            ViewData["Error"] = "Not enough " + product.Name + " in stock";
                            return View(order);
                        }

                        product.Stock -= pc.Quantity;
                        productsInOrder.Add(product);
                        ProductsOrder newPO = new ProductsOrder
                        {
                            Product = product,
                            Order = order,
                            Quantity = pc.Quantity
                        };

                        order.Price += pc.Quantity * product.Price;
                        order.productsOrders.Add(newPO);
                    }

                    _context.RemoveRange(productCart);
                    _context.Add(order);
                    _context.UpdateRange(productsInOrder);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(MyOrders));
                }

                return View(order);
            }
            else
            {
                return NotFound();
            }
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> OrdersPricesPerMonth()
        {
            var ordersPricesByMonth = _context.Order
                .GroupBy(x => new
                {
                    month = x.Date.Month,
                    year = x.Date.Year
                }).Select(x => new
                {
                    date = x.Key.month + "/" + x.Key.year,
                    price = (float)System.Math.Round(x.Average(p => p.Price), 3)
                });

            var ordersPricesByMonthList = await ordersPricesByMonth.ToListAsync();
            return Ok(ordersPricesByMonthList);
        }

        public async Task<IActionResult> SearchWithMulti(string name, string city, int priceFrom, int priceTo)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = string.Empty;
            }
            if (string.IsNullOrEmpty(city))
            {
                city = string.Empty;
            }
            if (priceTo == 0)
            {
                priceTo = int.MaxValue;
            }

            IQueryable<Order> orders = _context.Order.Include(o => o.Customer)
                .Where(o => (o.Customer.Name.Contains(name) || o.Customer.LastName.Contains(name))
                && o.ShippingAddressCity.Contains(city)
                && o.Price >= priceFrom && o.Price <= priceTo);

            var ordersList = await orders.ToListAsync();

            return PartialView("OrdersList", ordersList);
        }

    }
}
