using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Services;

namespace OnlineStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly CartService _cartService;

        public ProductController(CartService cartService)
        {
            _cartService = cartService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Помилка: Некоректні дані товару.";
                return View(product);
            }

            var cartItem = new CartItem
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = 1
            };

            _cartService.AddToCart(cartItem);
            TempData["SuccessMessage"] = "Товар успішно додано до кошика!";
            return RedirectToAction("Create");
        }

        public IActionResult Cart()
        {
            var cart = _cartService.GetCart();
            return View(cart);
        }

        public IActionResult ClearCart()
        {
            _cartService.ClearCart();
            TempData["SuccessMessage"] = "Кошик очищено!";
            return RedirectToAction("Cart");
        }
    }
}



