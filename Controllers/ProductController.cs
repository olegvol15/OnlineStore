using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Models.ViewModels;
using OnlineStore.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly CartService _cartService;
        private readonly ReviewService _reviewService;
        private readonly AppDbContext _context;

        public ProductController(CartService cartService, ReviewService reviewService, AppDbContext context)
        {
            _cartService = cartService;
            _reviewService = reviewService;
            _context = context;
        }

        [HttpGet]
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

            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "Необхідна авторизація для додавання товару!";
                return RedirectToAction("Create");
            }

            var cartItem = new CartItem
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = 1,
                UserId = userId
            };

            _cartService.AddToCart(cartItem);
            TempData["SuccessMessage"] = "Товар успішно додано до кошика!";
            return RedirectToAction("Cart");
        }

        [HttpGet]
        public IActionResult Cart()
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var cart = _cartService.GetCart(userId);
            return View(cart);
        }

        [HttpPost]
        public IActionResult ModifyCart(int productId, int quantity)
        {
            try
            {
                var userId = User.Identity?.Name;
                if (string.IsNullOrEmpty(userId)) return Unauthorized();

                _cartService.ModifyCart(productId, quantity, userId);
                TempData["SuccessMessage"] = "Кількість товару у кошику оновлено!";
            }
            catch (UnauthorizedAccessException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Помилка: {ex.Message}";
            }

            return RedirectToAction("Cart");
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            _cartService.ClearCart(userId);
            TempData["SuccessMessage"] = "Кошик очищено!";
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public IActionResult Pay()
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            _cartService.CloseCart(userId, "Оплачений");
            TempData["SuccessMessage"] = "Кошик оплачено!";
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public IActionResult Cancel()
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            _cartService.CloseCart(userId, "Скасований");
            TempData["SuccessMessage"] = "Кошик скасовано!";
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public IActionResult Repeat(Guid cartId)
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            _cartService.RepeatCart(cartId, userId);
            TempData["SuccessMessage"] = "Замовлення повторено!";
            return RedirectToAction("Cart");
        }

        public IActionResult Details(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            var reviews = _reviewService.GetReviewsForProduct(id);

            var promotions = _context.ProductPromotions
                .Where(pp => pp.ProductId == id)
                .Select(pp => pp.Promotion)
                .Where(p => p.StartDate <= DateTime.Today && p.EndDate >= DateTime.Today)
                .ToList();

            var vm = new ProductDetailsViewModel
            {
                Product = product,
                Reviews = reviews,
                Promotions = promotions
            };

            return View(vm);
        }
    }
}






