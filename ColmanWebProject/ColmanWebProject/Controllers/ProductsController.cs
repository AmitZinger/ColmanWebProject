﻿using System;
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

namespace ColmanWebProject.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ColmanWebProjectContext _context;

        public ProductsController(ColmanWebProjectContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(String Catagory, string SubCatagory)
        {
                var data = _context.Product.Include(product => product.Categories)
                .Where(product => product.Categories.Any(catagory => catagory.Type.Equals(Catagory)
                && (catagory.SubType.Equals(SubCatagory) || catagory.SubType.Equals(null))));
            return View(await data.ToListAsync());
        }

        public async Task<IActionResult> SearchWithPartialView(string queryTitle)
        {
            IQueryable<Product> searchResult = SearchResult(queryTitle);
            return PartialView("Index", await searchResult.ToListAsync());
        }
        
        public async Task<IActionResult> SearchWithFullView(string queryTitle)
        {
            IQueryable<Product> searchResult = SearchResult(queryTitle);
            return View("Index", await searchResult.ToListAsync());
        }

        private IQueryable<Product> SearchResult(string queryTitle)
        {
            return from product in _context.Product.Include(product => product.Categories)
            where (product.Name.Contains(queryTitle) ||
                   product.Categories.Any(
                       catagory => catagory.Type.Contains(queryTitle) ||
                                   catagory.SubType.Contains(queryTitle)))
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
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Stock,Description")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
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
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Stock,Description")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
                return RedirectToAction(nameof(Index));
            }
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
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}