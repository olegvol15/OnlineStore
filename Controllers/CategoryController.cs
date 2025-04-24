using Microsoft.AspNetCore.Mvc;
using OnlineStore.Services;

namespace OnlineStore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DataAccessor _dataAccessor;

        public CategoryController(DataAccessor dataAccessor)
        {
            _dataAccessor = dataAccessor;
        }

        [HttpGet]
        public IActionResult List()
        {
            var categories = _dataAccessor.CategoriesList();
            return View(categories);
        }
    }
}
