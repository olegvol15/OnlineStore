using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    public class ReviewController : Controller
    {
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Review review)
        {
            if (!ModelState.IsValid)
            {
                return View(review);
            }

            TempData["SuccessMessage"] = "Відгук успішно додано!";
            return RedirectToAction("Create");
        }
    }
}
