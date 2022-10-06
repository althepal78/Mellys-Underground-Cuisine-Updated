using DAL.Context;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mellys_Underground_Cuisine.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _db;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public CartController(ILogger<HomeController> logger,
            AppDbContext db, IBackgroundJobClient backgroundJobClient)
        {
            _logger = logger;
            _db = db;
            _backgroundJobClient = backgroundJobClient;
        }

        
        public async Task<IActionResult> AddToCart( Guid dishid, Guid menuid)
        {
            if(menuid == Guid.Empty && dishid == Guid.Empty)
            {
                return NotFound("Shit is not found");
            }

            var md = await _db.MenuDishes.Where(dm => dm.DishId == dishid && dm.MenuId == menuid).FirstOrDefaultAsync(); ;

            if(md == null)
            {
                return BadRequest("MD is null fucker lol");
            }
            Console.WriteLine($"The servings for {md.Dish} is {md.Servings}!");
            md.Servings = md.Servings -= 1;
            Console.WriteLine($"The servings after ordered for {md.Dish} is {md.Servings}!");
            _db.MenuDishes.Update(md);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index), "Home");
        }

    }
}
