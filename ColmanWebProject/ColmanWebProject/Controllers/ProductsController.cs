using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ColmanWebProject.Data;
using ColmanWebProject.Models;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace ColmanWebProject.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ColmanWebProjectContext _context;

        public ProductsController(ColmanWebProjectContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ManageProducts()
        {
            var checkDbContext = _context.Product.Include(p => p.Category);
            return View(await checkDbContext.ToListAsync());
        }

        // GET: Products
        public async Task<IActionResult> Index(string Catagory, string SubCatagory)
        {
            IQueryable<Product> data;
            if (SubCatagory.Equals("General"))
            {
                data = _context.Product.Include(product => product.Category)
               .Where(product => product.Category.Type.Equals(Catagory));

            }
            else
            {
                data = _context.Product.Include(product => product.Category)
                .Where(product => product.Category.Type.Equals(Catagory) &&
                product.Category.SubType.Equals(SubCatagory));
            }

            return View(await data.ToListAsync());
        }

        public async Task<IActionResult> SearchWithPartialView(string queryTitle)
        {
            IQueryable<Product> searchResult = SearchResult(queryTitle);
            return PartialView("ProductsList", await searchResult.ToListAsync());
        }

        public async Task<IActionResult> SearchWithMulti(string name, string category, int priceFrom, int priceTo)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = string.Empty;
            }
            if (string.IsNullOrEmpty(category))
            {
                category = string.Empty;
            }
            if (priceTo == 0)
            {
                priceTo = int.MaxValue;
            }

            IQueryable<Product> searchResult = from product in _context.Product.Include(product => product.Category)
                                               where (product.Name.Contains(name) &&
                                                      product.Category.Type.Contains(category) &&
                                                      product.Price >= priceFrom && product.Price <= priceTo)
                                               select product;
            return PartialView("ProductsList", await searchResult.ToListAsync());
        }

        public async Task<IActionResult> SearchWithFullView(string queryTitle)
        {
            IQueryable<Product> searchResult = SearchResult(queryTitle);
            return View("Index", await searchResult.ToListAsync());
        }

        private IQueryable<Product> SearchResult(string queryTitle)
        {
            return from product in _context.Product.Include(product => product.Category)
                   where (product.Name.Contains(queryTitle) ||
                          product.Category.Type.Contains(queryTitle) ||
                          product.Category.SubType.Contains(queryTitle))
                   select product;
        }


        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            var categories = _context.Category.Select(s => new
            {
                Text = s.Type + ", " + s.SubType,
                Value = s.Id
            }).ToList();

            ViewData["CategoryId"] = new SelectList(categories, "Value", "Text");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Stock,Description,CategoryId,ImageFile")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ImageFile != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        product.ImageFile.CopyTo(ms);
                        product.Image = ms.ToArray();
                    }
                }

                _context.Add(product);
                await _context.SaveChangesAsync();
            }
            return PartialView("ManageProductsList", await _context.Product.Include(p => p.Category).ToListAsync());
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var categories = _context.Category.Select(s => new
            {
                Text = s.Type + ", " + s.SubType,
                Value = s.Id
            }).ToList();

            ViewData["CategoryId"] = new SelectList(categories, "Value", "Text");
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Stock,Description,CategoryId,ImageFile")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (product.ImageFile != null)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            product.ImageFile.CopyTo(ms);
                            product.Image = ms.ToArray();
                        }
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ManageProducts");
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Description", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageProducts");
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SearchByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = string.Empty;
            }

            IQueryable<Product> searchResult = from product in _context.Product
                                               where (product.Name.Contains(name))
                                               select product;
            return View("ManageProductsList", await searchResult.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MostOrdered()
        {
            var productsOrders = _context.ProductsOrder
                    .Join(
                            _context.Product,
                            productsOrders => productsOrders.ProductId,
                            product => product.Id,
                            (productsOrders, product) => new
                            {
                                name = product.Name,
                                quantity = productsOrders.Quantity,
                            }
                         )
                    .GroupBy(product => product.name)
                    .Select(po => new
                    {
                        name = po.Key,
                        value = po.Sum(s => s.quantity)
                    });
            var productsList = await productsOrders.ToListAsync();
            return Ok(productsList);
        }
    }
}
