using AutoMapper;
using DAL.Context;
using DAL.Entities;
using Mellys_Underground_Cuisine.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;

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
      Console.WriteLine("Adding the Ingredient Get View Dish ID: " + id);
      var dishIngredients = _db._dishIngredients.Where(x => x.DishId == id).Include(ing => ing.Ingredients).Select(x => x.Ingredients).ToList();

      AddIngredientVM vm = new AddIngredientVM
      {
        DishId = id,
        Ingredient = dishIngredients
      };

      return View(vm);
    }


    [HttpPost]
    public async Task<IActionResult> AddIngredient(AddIngredientVM vm)
    {
      var trimName = vm.Name.Trim();
      var normalizeTrimName = trimName.ToUpper();
      var exists = await _db.Ingredients.FirstOrDefaultAsync(ing => ing.NormalizeName == normalizeTrimName);

      if (exists is null)
      {
        Ingredient ingredient = new Ingredient()
        {
          Name = trimName,
          NormalizeName = normalizeTrimName,
        };

        await _db.Ingredients.AddAsync(ingredient);
        await _db.SaveChangesAsync();

        Console.WriteLine("shit don't exist but we created one :) ");
      }
      else
      {
        Console.WriteLine(exists + " ****************************");
      }

      return RedirectToAction("Index");
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
      //var dish = new DishVM();
      var dish = _db.Dishes.Where(x => x.Id == Guid.Parse("dcaa6957-7711-4aa6-1f74-08da50ddf38f")).FirstOrDefault();
      DishVM vm = new DishVM
      {
        FilePath = dish.FilePath
      };
      var test = BuildFilePath(vm);
      return View();
    }

    #region Helper Methods
    public static string BuildFilePath(DishVM vm)
    {

      var assemblyPath = Directory.GetCurrentDirectory(); /*Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)*/;
      string serverFolder = "";
      if (vm.FilePath is null)
      {
        string folder = @"wwwroot\images\foodimages\";

        // combine paths to create the path
        serverFolder = Path.Combine(assemblyPath, folder);
        return serverFolder;
      }

      serverFolder = Path.Combine(assemblyPath, vm.FilePath.Replace("/", @"\").TrimStart('\\'));
      return serverFolder;
    }
    #endregion

  }

}
