using OnlineStore.Models;
using System.Collections.Generic;

namespace OnlineStore.Models.ViewModels
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; } = new();
        public List<Review> Reviews { get; set; } = new();
    }
}
