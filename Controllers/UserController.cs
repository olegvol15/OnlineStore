using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineStore.Controllers
{
    public class UserController : Controller
    {
        private List<Order> GetOrders(string username)
        {
            return new List<Order>
            {
                new Order { ProductName = "Іграшковий кіт", PurchaseDate = DateTime.Now.AddDays(-3) },
                new Order { ProductName = "Плюшевий ведмідь", PurchaseDate = DateTime.Now.AddDays(-10) }
            };
        }

        public IActionResult Profile(string username)
        {
            var currentUser = User.Identity?.Name;
            var isOwner = currentUser == username;

            var model = new UserProfileViewModel
            {
                Username = username,
                Email = GetEmail(username),
                Orders = isOwner ? GetOrders(username) : new List<Order>()
            };

            return View(model);
        }

        // Заглушка: отримаємо email користувача
        private string GetEmail(string username)
        {
            return $"{username}@example.com";
        }
    }
}