using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OnlineStore.Models;
using System;
using System.Collections.Generic;

namespace OnlineStore.Services
{
    public class CartService
    {
        private readonly ISession _session;
        private const string CartKey = "Cart";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _session = httpContextAccessor.HttpContext?.Session 
                       ?? throw new ArgumentNullException(nameof(httpContextAccessor), "Session is not available.");
        }

        // Получить корзину текущего пользователя
        public List<CartItem> GetCart()
        {
            var cartJson = _session.GetString(CartKey);
            return cartJson == null 
                ? new List<CartItem>() 
                : JsonConvert.DeserializeObject<List<CartItem>>(cartJson) ?? new List<CartItem>();
        }

        // Добавить товар в корзину
        public void AddToCart(CartItem item)
        {
            var cart = GetCart();
            var userId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("Користувач не авторизований!");
            }

            item.UserId = userId; // Привязываем товар к пользователю

            var existingItem = cart.Find(i => i.ProductId == item.ProductId && i.UserId == userId);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                cart.Add(item);
            }

            SaveCart(cart);
        }

        // Изменить количество товара в корзине
        public void ModifyCart(int productId, int newQuantity)
        {
            var cart = GetCart();
            var userId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("Користувач не авторизований!");
            }

            var item = cart.Find(i => i.ProductId == productId && i.UserId == userId);

            if (item == null)
            {
                throw new KeyNotFoundException("Товар не знайдено у кошику!");
            }

            if (item.UserId != userId)
            {
                throw new UnauthorizedAccessException("Цей товар не належить поточному користувачеві!");
            }

            if (newQuantity <= 0)
            {
                cart.Remove(item);
            }
            else
            {
                item.Quantity = newQuantity;
            }

            SaveCart(cart);
        }

        // Очистить корзину
        public void ClearCart()
        {
            _session.Remove(CartKey);
        }

        // Сохранить корзину в сессии
        private void SaveCart(List<CartItem> cart)
        {
            _session.SetString(CartKey, JsonConvert.SerializeObject(cart));
        }
    }
}

