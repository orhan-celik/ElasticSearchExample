using ElasticSearchExample.MVC.Services;
using ElasticSearchExample.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearchExample.MVC.Controllers
{
    public class BlogController : Controller
    {

        private readonly BlogService _blogService;

        public BlogController(BlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _blogService.GetAllAsync();
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(BlogCreateViewModel model)
        {
            var result = await _blogService.SaveAsync(model);
            if (!result)
            {
                TempData["result"] = "Blog datası indexlenemedi.";
                return RedirectToAction(nameof(BlogController.Create), model);
            }

            TempData["result"] = "Blog datası indexlendi.";

            return RedirectToAction(nameof(BlogController.Index));
        }

        [HttpPost]
        public async Task<IActionResult> Search(string searchText)
        {
            var result = await _blogService.SearchAsync(searchText);
            ViewBag.SearchText = searchText;
            return View(nameof(BlogController.Index), result);
        }

        [HttpGet]
        [Route("Blog/Details/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            var result = await _blogService.GetById(id);
            return View(result);
        }

    }
}
