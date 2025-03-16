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

        // Форма добавления товара
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Добавление товара в корзину
        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Помилка: Некоректні дані товару.";
                return View(product);
            }

            var userId = User.Identity?.Name; // Получаем ID авторизованного пользователя

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
                UserId = userId // Привязываем товар к владельцу
            };

            _cartService.AddToCart(cartItem);
            TempData["SuccessMessage"] = "Товар успішно додано до кошика!";
            return RedirectToAction("Cart");
        }

        // Отображение корзины
        [HttpGet]
        public IActionResult Cart()
        {
            var cart = _cartService.GetCart();
            return View(cart);
        }

        // Изменение количества товара в корзине
        [HttpPost]
        public IActionResult ModifyCart(int productId, int quantity)
        {
            try
            {
                _cartService.ModifyCart(productId, quantity);
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


        // Очистка корзины
        [HttpPost]
        public IActionResult ClearCart()
        {
            _cartService.ClearCart();
            TempData["SuccessMessage"] = "Кошик очищено!";
            return RedirectToAction("Cart");
        }
    }
}





