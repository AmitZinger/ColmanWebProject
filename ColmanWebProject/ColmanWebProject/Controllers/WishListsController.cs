using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ColmanWebProject.Data;
using ColmanWebProject.Models;

namespace ColmanWebProject.Controllers
{
    public class WishListsController : Controller
    {
        private readonly ColmanWebProjectContext _context;

        public WishListsController(ColmanWebProjectContext context)
        {
            _context = context;
        }

        // GET: WishLists
        public async Task<IActionResult> Index(int WishListId)
        {
            var wishList = _context.WishList.Include(w => w.ProductsWishList)
                .ThenInclude(pw => pw.Product).Where(w => w.Id == WishListId); 
            return View(await wishList.SelectMany(w => w.ProductsWishList.Select(pw => pw.Product)).ToListAsync());
        }

        // GET: WishLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishList = await _context.WishList
                .Include(w => w.ProductsWishList).ThenInclude(pw => pw.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wishList == null)
            {
                return NotFound();
            }

            return View(wishList);
        }

        // GET: WishLists/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Description");
            return View();
        }

        // POST: WishLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] WishList wishList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wishList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Description", wishList.ProductId);
            return View(wishList);
        }

        public async Task<IActionResult> AddToWishList(int WishListId, int ProductId)
        {
            var product = await _context.Product.FirstOrDefaultAsync(p => p.Id == ProductId);
            var wishList = await _context.WishList.FirstOrDefaultAsync(w => w.Id == WishListId);
            var currPW = _context.ProductsWishList
                .FirstOrDefault(pw => pw.ProductId == ProductId && pw.WishListId == WishListId);
            
            if(currPW != null)
            {
                currPW.Quantity += 1;
                _context.Update(currPW);
            } else {
                ProductsWishList newPW = new ProductsWishList
                {
                    Product = product,
                    WishList = wishList,
                    Quantity = 1
                };
                _context.Add(newPW);
            }

            await _context.SaveChangesAsync();
            return await Index(WishListId);
        }


        // GET: WishLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishList = await _context.WishList.FindAsync(id);
            if (wishList == null)
            {
                return NotFound();
            }
           // ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Description", wishList.ProductId);
            return View(wishList);
        }

        // POST: WishLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] WishList wishList)
        {
            if (id != wishList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wishList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WishListExists(wishList.Id))
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
          //  ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Description", wishList.ProductId);
            return View(wishList);
        }

        // GET: WishLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishList = await _context.WishList
                .Include(w => w.ProductsWishList).ThenInclude(pw => pw.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wishList == null)
            {
                return NotFound();
            }

            return View(wishList);
        }

        // POST: WishLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wishList = await _context.WishList.FindAsync(id);
            _context.WishList.Remove(wishList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WishListExists(int id)
        {
            return _context.WishList.Any(e => e.Id == id);
        }
    }
}
