
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineStore.Services
{
    public class Cart
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; } = default!;
        public string Status { get; set; } = "Активний";
        public DateTime? ClosedDate { get; set; }
        public List<CartItem> Items { get; set; } = new();
    }

    public class CartService
    {
        private readonly ISession _session;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CartKeyPrefix = "Cart_";

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _session = httpContextAccessor.HttpContext?.Session 
                ?? throw new ArgumentNullException(nameof(httpContextAccessor), "Session is not available.");
        }

        private string GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("Користувач не авторизований!");
            return userId;
        }

        private string GetCartKey(string userId) => $"{CartKeyPrefix}{userId}";

        public Cart GetCart(string userId)
        {
            var cartJson = _session.GetString(GetCartKey(userId));
            if (string.IsNullOrEmpty(cartJson))
            {
                return new Cart
                {
                    UserId = userId,
                    Status = "Активний",
                    Items = new List<CartItem>()
                };
            }

            var cart = JsonConvert.DeserializeObject<Cart>(cartJson);
            if (cart == null)
            {
                return new Cart
                {
                    UserId = userId,
                    Status = "Активний",
                    Items = new List<CartItem>()
                };
            }

            return cart;
        }

        private void SaveCart(Cart cart)
        {
            _session.SetString(GetCartKey(cart.UserId), JsonConvert.SerializeObject(cart));
        }

        public void AddToCart(CartItem item)
        {
            var userId = GetUserId();
            var cart = GetCart(userId);

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                item.UserId = userId;
                cart.Items.Add(item);
            }

            SaveCart(cart);
        }

        public void ModifyCart(int productId, int newQuantity, string userId)
        {
            var cart = GetCart(userId);
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
                throw new KeyNotFoundException("Товар не знайдено у кошику!");

            if (item.UserId != userId)
                throw new UnauthorizedAccessException("Цей товар не належить поточному користувачеві!");

            if (newQuantity <= 0)
            {
                cart.Items.Remove(item);
            }
            else
            {
                item.Quantity = newQuantity;
            }

            SaveCart(cart);
        }

        public void ClearCart(string userId)
        {
            _session.Remove(GetCartKey(userId));
        }

        public void CloseCart(string userId, string status)
        {
            var cart = GetCart(userId);
            if (cart.Status != "Активний") return;

            cart.Status = status;
            cart.ClosedDate = DateTime.Now;

            SaveCart(cart);
        }

        public void RepeatCart(Guid cartId, string userId)
        {
            var closedCart = GetCart(userId);
            if (closedCart.Id != cartId || closedCart.Status == "Активний")
                return;

            var newCart = GetCart(userId);
            if (newCart.Status != "Активний")
                newCart = new Cart { UserId = userId };

            foreach (var item in closedCart.Items)
            {
                var existing = newCart.Items.FirstOrDefault(i => i.ProductId == item.ProductId);
                if (existing != null)
                {
                    existing.Quantity += item.Quantity;
                }
                else
                {
                    newCart.Items.Add(new CartItem
                    {
                        ProductId = item.ProductId,
                        Name = item.Name,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        UserId = userId
                    });
                }
            }

            newCart.Status = "Активний";
            newCart.ClosedDate = null;
            SaveCart(newCart);
        }
    }
}


