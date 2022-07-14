using DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<DishIngredient> _dishIngredients { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<MenuDish> MenuDishes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            // setting up the dish and ingredient table _dishIngredient
            builder.Entity<DishIngredient>().HasKey(di => new { di.DishId, di.IngredientsId });

            builder.Entity<DishIngredient>()
                .HasOne(i => i.Ingredients)
                .WithMany(i => i.DishIngredient).HasForeignKey(fk => fk.IngredientsId);


            builder.Entity<DishIngredient>()
                .HasOne(d => d.Dish)
                .WithMany(i => i.DishIngredient).HasForeignKey(fk => fk.DishId);


            //setting up the Menu and dish table MenuDish
            builder.Entity<MenuDish>().HasKey(di => new { di.DishId, di.MenuId });

            builder.Entity<MenuDish>()
                .HasOne(i => i.Menu)
                .WithMany(i => i.MenuDish).HasForeignKey(fk => fk.MenuId);

            builder.Entity<MenuDish>()
                .HasOne(d => d.Dish)
                .WithMany(i => i.MenuDish).HasForeignKey(fk => fk.DishId);

            //fixing entities so they won't be maxed
            builder.Entity<AppUser>(u =>
            {
                u.Property(user => user.PhoneNumber)
                        .HasMaxLength(15);

                u.Property(user => user.PasswordHash)
                        .HasMaxLength(128);

                u.Property(user => user.ConcurrencyStamp)
                        .HasMaxLength(36);

                u.Property(user => user.SecurityStamp)
                        .HasMaxLength(36);

            });
        }
    }
}
