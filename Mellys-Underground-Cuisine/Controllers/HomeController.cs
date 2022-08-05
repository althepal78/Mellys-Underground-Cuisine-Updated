using DAL.Context;
using Hangfire;
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
        private readonly IBackgroundJobClient _backgroundJobClient;

        public HomeController(ILogger<HomeController> logger, AppDbContext db, IBackgroundJobClient backgroundJobClient)
        {
            _logger = logger;
            _db = db;
            _backgroundJobClient = backgroundJobClient;
        }

      

        public IActionResult Index()
        {
            MenuVM menuVM = new MenuVM();

            var dishes = _db.Dishes.ToList();
            var menus = _db.Menu.ToList();

            menuVM.Menu = menus;
            menuVM.dishes = dishes;
            return View(menuVM);
        }

      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}