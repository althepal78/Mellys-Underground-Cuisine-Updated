using Microsoft.AspNetCore.Mvc;

namespace Mellys_Underground_Cuisine.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Menu()
        {
            return View();
        }
    }
}
