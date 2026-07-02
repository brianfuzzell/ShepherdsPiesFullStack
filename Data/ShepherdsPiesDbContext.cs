using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShepherdsPiesControllers.Models;

namespace ShepherdsPiesControllers.Data;

public class ShepherdsPiesDbContext : IdentityDbContext<Employee>
{
    public ShepherdsPiesDbContext(DbContextOptions<ShepherdsPiesDbContext> options) : base(options)
    {
    }

    public DbSet<Size> Sizes { get; set; }
    public DbSet<CheeseOption> CheeseOptions { get; set; }
    public DbSet<SauceOption> SauceOptions { get; set; }
    public DbSet<Topping> Toppings { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Pizza> Pizzas { get; set; }
    public DbSet<PizzaTopping> PizzaToppings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Employee)
            .WithMany()
            .HasForeignKey(o => o.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.DeliveryEmployee)
            .WithMany()
            .HasForeignKey(o => o.DeliveryEmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Size>().HasData(
            new Size { Id = 1, Name = "Small", Price = 10.00m },
            new Size { Id = 2, Name = "Medium", Price = 12.00m },
            new Size { Id = 3, Name = "Large", Price = 15.00m }
        );

        modelBuilder.Entity<CheeseOption>().HasData(
            new CheeseOption { Id = 1, Name = "Buffalo Mozzarella" },
            new CheeseOption { Id = 2, Name = "Four Cheese" },
            new CheeseOption { Id = 3, Name = "Vegan" },
            new CheeseOption { Id = 4, Name = "None" }
        );

        modelBuilder.Entity<SauceOption>().HasData(
            new SauceOption { Id = 1, Name = "Marinara" },
            new SauceOption { Id = 2, Name = "Arrabbiata" },
            new SauceOption { Id = 3, Name = "Garlic White" },
            new SauceOption { Id = 4, Name = "None" }
        );

        modelBuilder.Entity<Topping>().HasData(
            new Topping { Id = 1, Name = "Sausage", Price = 0.50m },
            new Topping { Id = 2, Name = "Pepperoni", Price = 0.50m },
            new Topping { Id = 3, Name = "Mushroom", Price = 0.50m },
            new Topping { Id = 4, Name = "Onion", Price = 0.50m },
            new Topping { Id = 5, Name = "Green Pepper", Price = 0.50m },
            new Topping { Id = 6, Name = "Black Olive", Price = 0.50m },
            new Topping { Id = 7, Name = "Basil", Price = 0.50m },
            new Topping { Id = 8, Name = "Extra Cheese", Price = 0.50m }
        );
    }
}