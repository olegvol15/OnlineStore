using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Services;

namespace OnlineStore.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ReviewService _reviewService;

        public ReviewController(ReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public IActionResult Create(int productId)
        {
            ViewBag.ProductId = productId;
            return View();
        }

        [HttpPost]
        public IActionResult Create(int productId, Review review)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ProductId = productId;
                return View(review);
            }

            review.ProductId = productId;
            _reviewService.AddReview(review);

            TempData["SuccessMessage"] = "Відгук успішно додано!";
            return RedirectToAction("Details", "Product", new { id = productId });
        }
    }
}


