using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OnlineStore.Models;

namespace OnlineStore.Services
{
    public class CartService
    {
        private readonly ISession _session;
        private const string CartKey = "Cart";

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext?.Session 
                    ?? throw new ArgumentNullException(nameof(httpContextAccessor), "Session is not available.");
        }


        public List<CartItem> GetCart()
        {
            var cartJson = _session.GetString(CartKey);
            return cartJson == null ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cartJson) ?? new List<CartItem>();
        }

        public void AddToCart(CartItem item)
        {
            var cart = GetCart();
            var existingItem = cart.Find(i => i.ProductId == item.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                cart.Add(item);
            }

            _session.SetString(CartKey, JsonConvert.SerializeObject(cart));
        }

        public void ClearCart()
        {
            _session.Remove(CartKey);
        }
    }
}
