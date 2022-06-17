using Microsoft.AspNetCore.Identity;

namespace DAL.Entities
{
    public class AppUser : IdentityUser
    {
        public List<Dish> Dishes { get; set; }
    }
}
