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
            var productsInWishList = _context.ProductsWishList
                .Include(pw => pw.WishList)
                .Include(pw => pw.Product)
                .Where(pw => pw.WishListId == WishListId).ToListAsync();

            return View(await productsInWishList);
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
            return RedirectToAction(nameof(Index), new { WishListId = WishListId });
        }

        // GET: WishLists/Delete/5
        public async Task<IActionResult> Delete(int? wishListId, int? productId)
        {
            if (wishListId == null || productId == null)
            {
                return NotFound();
            }

            var productsWishLists = await _context.ProductsWishList
                .Include(pw => pw.WishList)
                .Include(pw => pw.Product)
                .Where(pw => pw.WishListId == wishListId && pw.ProductId == productId).FirstOrDefaultAsync();

            if (productsWishLists == null)
            {
                return NotFound();
            }

            _context.Remove(productsWishLists);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { WishListId = wishListId });
        }

        private bool WishListExists(int id)
        {
            return _context.WishList.Any(e => e.Id == id);
        }
    }
}
