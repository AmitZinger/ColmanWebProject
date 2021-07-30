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
    public class CartsController : Controller
    {
        private readonly ColmanWebProjectContext _context;

        public CartsController(ColmanWebProjectContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int CartId)
        {
            var productsInCart = _context.ProductsCart
                .Include(pw => pw.Cart)
                .Include(pw => pw.Product)
                .Where(pw => pw.CartId == CartId).ToListAsync();

            return View(await productsInCart);
        }

        public async Task<IActionResult> AddToCartFromWishList(int CartId, int ProductId, int quantity)
        {
            var product = await _context.Product.FirstOrDefaultAsync(p => p.Id == ProductId);
            var Cart = await _context.Cart.FirstOrDefaultAsync(w => w.Id == CartId);
            var currPW = _context.ProductsCart
                .FirstOrDefault(pw => pw.ProductId == ProductId && pw.CartId == CartId);

            if (currPW != null)
            {
                currPW.Quantity += quantity;
                _context.Update(currPW);
            }
            else
            {
                ProductsCart newPW = new ProductsCart
                {
                    Product = product,
                    Cart = Cart,
                    Quantity = quantity
                };
                _context.Add(newPW);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { CartId = CartId });
        }

        public async Task<IActionResult> AddToCart(int CartId, int ProductId)
        {
            var product = await _context.Product.FirstOrDefaultAsync(p => p.Id == ProductId);
            var Cart = await _context.Cart.FirstOrDefaultAsync(w => w.Id == CartId);
            var currPW = _context.ProductsCart
                .FirstOrDefault(pw => pw.ProductId == ProductId && pw.CartId == CartId);

            if (currPW != null)
            {
                currPW.Quantity += 1;
                _context.Update(currPW);
            }
            else
            {
                ProductsCart newPW = new ProductsCart
                {
                    Product = product,
                    Cart = Cart,
                    Quantity = 1
                };
                _context.Add(newPW);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { CartId = CartId });
        }


        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? CartId, int? productId)
        {
            if (CartId == null || productId == null)
            {
                return NotFound();
            }

            var productsCarts = await _context.ProductsCart
                .Include(pw => pw.Cart)
                .Include(pw => pw.Product)
                .Where(pw => pw.CartId == CartId && pw.ProductId == productId).FirstOrDefaultAsync();

            if (productsCarts == null)
            {
                return NotFound();
            }

            _context.Remove(productsCarts);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { CartId = CartId });
        }

        private bool CartExists(int id)
        {
            return _context.Cart.Any(e => e.Id == id);
        }
    }
}
