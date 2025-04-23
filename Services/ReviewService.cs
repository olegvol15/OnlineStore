using OnlineStore.Models;
using System.Collections.Generic;
using System.Linq;

namespace OnlineStore.Services
{
    public class ReviewService
    {
        private static readonly List<Review> _reviews = new();

        public void AddReview(Review review)
        {
            _reviews.Add(review);
        }

        public List<Review> GetReviewsForProduct(int productId)
        {
            return _reviews.Where(r => r.ProductId == productId).ToList();
        }
    }
}
