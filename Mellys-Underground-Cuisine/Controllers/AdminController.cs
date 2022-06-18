using AutoMapper;
using DAL.Context;
using DAL.Entities;
using Mellys_Underground_Cuisine.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Mellys_Underground_Cuisine.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(AppDbContext Db,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment)
        {
            _db = Db;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Dish> exists = _db.Dishes
                .Include(di => di.DishIngredient)
                .ThenInclude(ing => ing.Ingredients).ToList();

            return View(exists);

        }

        public IActionResult SidePanel()
        {
            return View();
        }

        public IActionResult AddDish()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddDish(DishVM dishVM)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("ModelFailure", "failed Model State");
                return View(dishVM);
            }


            if (dishVM.FoodImage.ContentType != "image/jpeg" && dishVM.FoodImage.ContentType != "image/png" && dishVM.FoodImage.ContentType != "image/svg+xml")
            {
                ModelState.AddModelError("File Type Error", "You're only allowed png, jpeg, or svg type files");
                return View(dishVM);
            }

            if (dishVM.FoodImage != null)
            {
                //creating string to where the folders of the images will be
                string folder = "images/foodimages/";

                // create the path name
                folder += Guid.NewGuid().ToString() + "_" + dishVM.FoodImage.FileName;

                // combine paths to create the path
                string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

                // make it the filepath So i can link to it 
                dishVM.FilePath = "/" + folder;

                // create the connection
                await dishVM.FoodImage.CopyToAsync(new FileStream(serverFolder, FileMode.Create));


            }

            var newDish = _mapper.Map<Dish>(dishVM);

            await _db.Dishes.AddAsync(newDish);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index", "Admin");
        }


        public IActionResult AddIngredient(Guid id)
        {
            
            var dishIngredients = _db._dishIngredients.Where(x => x.DishId == id).Include(ing => ing.Ingredients).Select(x => x.Ingredients).ToList();

            AddIngredientVM vm = new AddIngredientVM
            {
                DishId = id,
                Ingredient = dishIngredients
            };

            return View(vm);
        }

        void AddToIngredients() { 
            

        }


        [HttpPost]
        public async Task<IActionResult> AddIngredient(AddIngredientVM vm)
        {
            Ingredient ingredient = new();


            var trimName = vm.Name.Replace(",", "").Trim();
            var normalizeTrimName = trimName.ToUpper();
            var exists = await _db.Ingredients.FirstOrDefaultAsync(ing => ing.NormalizeName == normalizeTrimName);
                       
            if (exists is null)
            {                
                ingredient.Name = trimName;
                ingredient.NormalizeName = normalizeTrimName;
                await _db.Ingredients.AddAsync(ingredient);
                await _db.SaveChangesAsync();
                Console.WriteLine("ingredient ID: " + ingredient.ID);
            }
            else
            {
                
                ingredient.ID = exists.ID;
            }

           
            if (ingredient.ID == Guid.Empty)
            {
                ModelState.AddModelError("EmptyID", "Unable to save the igredient to the db");
                return View(vm);
            }



            var ingInDish = _db._dishIngredients.FirstOrDefaultAsync(dishId => dishId.DishId == vm.DishId && dishId.IngredientsId == ingredient.ID);

            if(ingInDish != null)
            {
                ModelState.AddModelError("InDishIngre", "Already In Dish");

                return View(vm);
            }
          
                DishIngredient dishIngredient = new()
                {
                    DishId = vm.DishId,
                    IngredientsId = ingredient.ID,
                };

                await _db._dishIngredients.AddAsync(dishIngredient);
                await _db.SaveChangesAsync();

                return RedirectToAction("AddIngredient");
            

        }


        [HttpPost]
        public async Task<IActionResult> DeleteIngredient(Guid dishID, Guid IngID)
        {
            var ingredientsInDish = _db._dishIngredients.Where(gu => gu.DishId == dishID).Include(ingd => ingd.Ingredients).ToList();


            if (ingredientsInDish is null)
            {
                Console.WriteLine("dish is null");
            }
            else
            {

                foreach (var ingd in ingredientsInDish)
                {
                    if (ingd.IngredientsId == IngID)
                    {
                        _db._dishIngredients.Remove(ingd);
                        await _db.SaveChangesAsync();
                    }
                }

            }

            return RedirectToAction("AddIngredient", new { id = dishID.ToString() });
        }


        public IActionResult EditDish()
        {
            return View();
        }


    }

}
