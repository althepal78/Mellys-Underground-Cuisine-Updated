using AutoMapper;
using DAL.Entities;
using Mellys_Underground_Cuisine.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Mellys_Underground_Cuisine.Controllers
{

    public class AccountsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountsController(IMapper mapper,
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AppUserVM appUserVM)
        {
            if (!ModelState.IsValid)
            {
                return View(appUserVM);
            }

            var isUserInDatabase = await _userManager.FindByEmailAsync(appUserVM.Email);
            if (isUserInDatabase != null)
            {
                ModelState.AddModelError("Email", "Email already exist bruh");
                return View(appUserVM);
            }

            var newUser = _mapper.Map<AppUser>(appUserVM);


            var result = await _userManager.CreateAsync(newUser, appUserVM.Password);
            if (result.Succeeded)
            {
                var res = await _userManager.AddToRoleAsync(newUser, "User");
                if (!res.Succeeded)
                {
                    Console.WriteLine("Not workign");
                    ModelState.AddModelError(String.Empty, "Unable to complete the registration");
                    return View(appUserVM);
                }
                return RedirectToAction("Menu", "Menu");
            }
            else
            {

                foreach (var err in result.Errors)
                {

                    ModelState.AddModelError(err.Code, err.Description);

                }
                return View(appUserVM);
            }

        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserVM loginUser)
        {
            if (!ModelState.IsValid)
            {
                return View(loginUser); 
            }

            AppUser isUserInDb = await _userManager.FindByEmailAsync(loginUser.Email);

            if (isUserInDb == null)
            {
                ModelState.AddModelError("Uesr", "User Doesn't exist");
                return View(loginUser);
            }

            var validPassword = await _userManager.CheckPasswordAsync(isUserInDb, loginUser.Password);
            if (validPassword != true)
            {
                ModelState.AddModelError("Password", "Bad Password!!!");
                return View(loginUser);
            }

            await _signInManager.SignInAsync(isUserInDb, validPassword);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

