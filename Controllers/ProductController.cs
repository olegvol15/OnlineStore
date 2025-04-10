using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Services;
using System;
using System.Collections.Generic;

namespace OnlineStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly CartService _cartService;

        public ProductController(CartService cartService)
        {
            _cartService = cartService;
        }

        // GET: Форма додавання товару
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Додавання товару в кошик
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

        // GET: Перегляд кошика
        [HttpGet]
        public IActionResult Cart()
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var cart = _cartService.GetCart(userId);
            return View(cart);
        }

        // POST: Зміна кількості товару
        [HttpPost]
        public IActionResult ModifyCart(int productId, int quantity)
        {
            try
            {
                _cartService.ModifyCart(productId, quantity, User.Identity?.Name!);
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

        // POST: Очищення кошика
        [HttpPost]
        public IActionResult ClearCart()
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            _cartService.ClearCart(userId);
            TempData["SuccessMessage"] = "Кошик очищено!";
            return RedirectToAction("Cart");
        }

        // POST: Оплата кошика
        [HttpPost]
        public IActionResult Pay()
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            _cartService.CloseCart(userId, "Оплачений");
            TempData["SuccessMessage"] = "Кошик оплачено!";
            return RedirectToAction("Cart");
        }

        // POST: Скасування кошика
        [HttpPost]
        public IActionResult Cancel()
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            _cartService.CloseCart(userId, "Скасований");
            TempData["SuccessMessage"] = "Кошик скасовано!";
            return RedirectToAction("Cart");
        }

        // POST: Повторити замовлення
        [HttpPost]
        public IActionResult Repeat(Guid cartId)
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            _cartService.RepeatCart(cartId, userId);
            TempData["SuccessMessage"] = "Замовлення повторено!";
            return RedirectToAction("Cart");
        }
    }
}






