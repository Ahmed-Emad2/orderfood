using Microsoft.EntityFrameworkCore;

namespace orderfood.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<FoodItem> FoodItems { get; set; }
    }

    public class FoodItem
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int ItemId { get; set; }

        
        public string RestaurantName { get; set; }

        public string Name { get; set; }
        public string Components { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }
}