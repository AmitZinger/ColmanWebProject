﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ColmanWebProject.Data;
using ColmanWebProject.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;

namespace ColmanWebProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly ColmanWebProjectContext _context;

        public CategoriesController(ColmanWebProjectContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Category.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,SubType,Description,ImageFile")] Category category)
        {
            if (ModelState.IsValid)
            {
                var checkExist = _context.Category.FirstOrDefault
                (cat =>
                    cat.Type.Equals(category.Type) && cat.SubType.Equals(category.SubType)
                );

                if (checkExist == null)
                {
                    if (category.ImageFile != null)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            category.ImageFile.CopyTo(ms);
                            category.Image = ms.ToArray();
                        }
                    }
                    _context.Add(category);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return Json(new
                    {
                        Html = "Catagory already exist; You can't create it again.",
                        Error = true
                    });
                }
            }
            else
            {
                return Json(new
                {
                    Html = "Couldn't create this catagory.",
                    Error = true
                });
            }

            return PartialView("CategoriesList", await _context.Category.ToListAsync());
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,SubType,Description,Image,ImageFile")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (category.ImageFile != null)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            category.ImageFile.CopyTo(ms);
                            category.Image = ms.ToArray();
                        }
                    }
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Category.FindAsync(id);
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SearchByTypeName(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                typeName = string.Empty;
            }

            IQueryable<Category> searchResult = from category in _context.Category
                                                where (category.Type.Contains(typeName))
                                                select category;
            return View("CategoriesList", await searchResult.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ProductsByCategories()
        {
            var productsByCategories = _context.Category
                .Join(
                _context.Product,
                            category => category.Id,
                            product => product.CategoryId,
                            (category, product) => new
                            {
                                type = category.Type,
                                productId = product.Id
                            }
                            )
                .GroupBy(cp => cp.type)
                .Select(cp => new
                {
                    name = cp.Key,
                    value = cp.Count()
                });

            var productsByCategoriesList = await productsByCategories.ToListAsync();
            return Ok(productsByCategoriesList);
        }
    }
}
