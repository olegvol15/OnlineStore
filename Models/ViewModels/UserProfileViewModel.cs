using System;
using System.Collections.Generic;

namespace OnlineStore.Models.ViewModels
{
    public class UserProfileViewModel
    {
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public List<Order> Orders { get; set; } = new();
    }

    public class Order
    {
        public string ProductName { get; set; } = default!;
        public DateTime PurchaseDate { get; set; }
    }
}
