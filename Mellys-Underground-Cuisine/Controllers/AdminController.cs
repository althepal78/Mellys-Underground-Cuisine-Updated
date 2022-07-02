using AutoMapper;
using DAL.Context;
using DAL.Entities;
using Mellys_Underground_Cuisine.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        #region Helper Methods
        public static string BuildFilePath(DishVM vm)
        {
            var assemblyPath = Directory.GetCurrentDirectory();
            string serverFolder = "";
            if (vm.FilePath is null)
            {
                string folder = @"wwwroot\images\foodimages\";

                // combine paths to create the path
                serverFolder = Path.Combine(assemblyPath, folder);
                return serverFolder;
            }

            serverFolder = Path.Combine(assemblyPath, "wwwroot", vm.FilePath.Replace("/", @"\").TrimStart('\\'));
            return serverFolder;
        }
        #endregion

        public IActionResult Index()
        {

            if (_db.Dishes.ToList() != null)
            {
                List<Dish> exists = _db.Dishes
                     .Include(di => di.DishIngredient)
                     .ThenInclude(ing => ing.Ingredients).ToList();

                return View(exists);
            }

            return View();
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

            var newDish = _mapper.Map<Dish>(dishVM);

            var exists = await _db.Dishes.Where(di => di.Name == newDish.Name).FirstOrDefaultAsync();
            if (exists != null)
            {
                ModelState.AddModelError("InDB", "Dish already exists in this database");
                return View(dishVM);
            }
            if (dishVM.FoodImage != null)
            {
                if (dishVM.FoodImage.ContentType != "image/jpeg" && dishVM.FoodImage.ContentType != "image/png" && dishVM.FoodImage.ContentType != "image/svg+xml")
                {
                    //ToDo: Add Logging
                    ModelState.AddModelError("InDB", "Not a valid image type");
                    return View(dishVM);
                }

                var filePath = BuildFilePath(dishVM);
                var fileName = Guid.NewGuid().ToString() + "_" + dishVM.FoodImage.FileName;
                var absolutePath = filePath + fileName;
                var basicPath = $"/images/foodimages/{fileName}";
                newDish.FilePath = basicPath;

                //save the file to disk
                using (var fs = new FileStream(absolutePath, FileMode.Create))
                {
                    dishVM.FoodImage.CopyTo(fs);
                }
            }
            else
            {
                newDish.FilePath = $"/images/foodimages/Image-Coming-Soon.png";
                newDish.IsDefaulting = true;
            }
            await _db.Dishes.AddAsync(newDish);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public IActionResult AddIngredient(Guid id)
        {

            var dishIngredients = _db._dishIngredients.Where(x => x.DishId == id).Include(ing => ing.Ingredients).Select(x => x.Ingredients).ToList();

            AddIngredientVM addIngredient = new AddIngredientVM
            {
                DishId = id,
                Ingredient = dishIngredients
            };

            return View(addIngredient);
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

            var ingInDish = await _db._dishIngredients.Where(dishId => dishId.DishId == vm.DishId).FirstOrDefaultAsync(ing => ing.IngredientsId == ingredient.ID);

            if (ingInDish != null)
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

            return RedirectToAction(nameof(AddIngredient), new { id = vm.DishId });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteIngredient(Guid dishID, Guid ingID)
        {
            var ingredientsInDish = _db._dishIngredients.Where(gu => gu.DishId == dishID).Include(ingd => ingd.Ingredients).ToList();

            if (ingredientsInDish is null)
            {
                Console.WriteLine("dish is null**********************************************************************************************");
            }
            else
            {
                foreach (var ingd in ingredientsInDish)
                {
                    if (ingd.IngredientsId == ingID)
                    {
                        _db._dishIngredients.Remove(ingd);
                        await _db.SaveChangesAsync();
                    }
                }
            }

            return RedirectToAction(nameof(AddIngredient), new { id = dishID });
        }

        [HttpGet]
        public IActionResult EditDish(Guid id)
        {
            var exists = _db.Dishes.Where(di => di.Id == id).FirstOrDefault();

            if (exists is null)
            {
                ModelState.AddModelError("Null", "Dish Is Null");
                return View(new { id = id });
            }

            DishVM dish = _mapper.Map<DishVM>(exists);


            return View(dish);
        }


        [HttpPost]
        public async Task<IActionResult> EditDish(DishVM dishVM)
        {
            var exists = await _db.Dishes.Where(di => di.Id == dishVM.Id).FirstOrDefaultAsync();

            if (exists is null)
            {
                ModelState.AddModelError("Null", "Dish Is Null");
                return View(dishVM);
            }
            dishVM.FilePath = exists.FilePath;
            dishVM.DishType = exists.DishType;
            _db.Entry(exists).State = EntityState.Detached;


            var dish = _mapper.Map<Dish>(dishVM);


            if (dishVM.FoodImage is null)
            {
                _db.Dishes.Update(dish);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Admin");
            }

            if (dishVM.FoodImage.ContentType != "image/jpeg" && dishVM.FoodImage.ContentType != "image/png" && dishVM.FoodImage.ContentType != "image/svg+xml")
            {
                //ToDo: Add Logging
                ModelState.AddModelError("InDB", "Not a valid image type");
                return View(dishVM);
            }

            var currentFilePath = BuildFilePath(new DishVM { FilePath = exists.FilePath });
            if (System.IO.File.Exists(currentFilePath))
            {
                System.IO.File.Delete(currentFilePath);
            }
            dishVM.FilePath = null;
            var filePath = BuildFilePath(dishVM);
            var fileName = Guid.NewGuid().ToString() + "_" + dishVM.FoodImage.FileName;
            var absolutePath = filePath + fileName;
            var basicPath = $"/images/foodimages/{fileName}";
            dish.FilePath = basicPath;

            //save the file to disk
            using (var fs = new FileStream(absolutePath, FileMode.Create))
            {
                dishVM.FoodImage.CopyTo(fs);
            }

            _db.Dishes.Update(dish);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteDish(Guid id)
        {
            var deleteDish = await _db.Dishes.Include(dish => dish.DishIngredient).Where(di => di.Id == id).FirstOrDefaultAsync();
            if (deleteDish is null)
            {
                ModelState.AddModelError("NotHere", "Dish is already Deleted");
                return RedirectToAction(nameof(Index));
            }
            _db.Dishes.Remove(deleteDish);
            _db.SaveChanges();


            return RedirectToAction(nameof(Index));
        }

        public IActionResult CreateMenu()
        {
            var dishlist = new MenuVM
            {
                DishList = _db.Dishes.ToList()
            };

            return View(dishlist);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMenu([FromBody] DishVM vm)
        {
            var exists = await _db.Dishes.Where(id => id.Id == vm.Id).FirstOrDefaultAsync();
            if (exists is null) { return View(vm); }

            exists.Quantity = vm.Quantity;

            var InMenu = _db.Menu.Any(di => di.MenuDish.Any(d => d.Id == vm.Id));
            
            if (InMenu)
            {
                var isChecked = await _db.Menu.Where(di => di.MenuDish.Any(id => id.Id == exists.Id)).FirstOrDefaultAsync();
                isChecked.IsChecked = true;
                Console.WriteLine(isChecked.ToString());
                _db.Menu.Update(isChecked);
                await _db.SaveChangesAsync();
                return Ok();
            }

            Menu menu = new()
            {
                IsChecked = true
            };
            menu.MenuDish.Add(exists);

            await _db.Menu.AddAsync(menu);
            await _db.SaveChangesAsync();
            return Ok();
        }

    }

}