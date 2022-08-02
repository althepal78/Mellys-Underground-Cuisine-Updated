using DAL.Context;
using Mellys_Underground_Cuisine.Models;
using Mellys_Underground_Cuisine.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Mellys_Underground_Cuisine.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _db;

        public HomeController(ILogger<HomeController> logger, AppDbContext _db)
        {
            _logger = logger;
            this._db = _db;
        }


        public IActionResult Index()
        {
            MenuVM menuVM = new MenuVM();

            var getMenu = _db.Dishes.ToList();

            menuVM.dishes = getMenu;
           
            return View("Index",  menuVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}