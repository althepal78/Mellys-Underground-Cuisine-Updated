using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace DAL.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public DbInitializer(AppDbContext db,
          RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task InitializeAsync()
        {
            if (_db.Roles.Any(r => r.Name == "User")) return;
            await _roleManager.CreateAsync(new IdentityRole("User"));
            await _roleManager.CreateAsync(new IdentityRole("Admin"));

            var password = "!$Food78$$";
            var user = new AppUser()
            {
                UserName = "MellyMels",
                Email = "melysundergroundcuisine@gmail.com",

            };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                Console.WriteLine("it's successful");
                var res = await _userManager.AddToRoleAsync(user, "Admin");
                await _db.SaveChangesAsync();
                if (res.Succeeded)
                {
                    Console.WriteLine(" is succesful in res and  result");
                }
            }
            else
            {
                Console.WriteLine("Shit didn't work");
            }
        }
    }
}
