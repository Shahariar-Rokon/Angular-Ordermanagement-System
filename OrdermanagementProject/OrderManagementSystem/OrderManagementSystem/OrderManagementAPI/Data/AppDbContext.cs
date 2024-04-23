using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrderManagementClassLibrary.Models;


namespace OrderManagementAPI.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<CustomerHeader> CustomerHeaders { get; set; }
        public DbSet<DailyMenu> DailyMenus { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeItem> RecipeItems { get; set; }
        public DbSet<Requisition> Requisitions { get; set; }
        public DbSet<CustomersMenu> DailyMenuCustomerRecords { get;set; }
        public DbSet<Transaction>Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Item>().HasData(
                new Item { ItemId = 1, Name = "Rice", Unit = "KG", Type = "Non-perishable"},
                new Item { ItemId = 2, Name = "Salt", Unit = "KG", Type = "Perishable" },
                new Item { ItemId = 3, Name = "Soyabean Oil", Unit = "Litter", Type = "Perishable" },
                new Item { ItemId = 4, Name = "Mustard Oil", Unit = "Litter", Type = "Perishable" },
                new Item { ItemId = 5, Name = "Bashmoti Rice", Unit = "KG", Type = "Non-perishable" },
                new Item { ItemId = 6, Name = "Mutton", Unit = "KG", Type = "Perishable" },
                new Item { ItemId = 7, Name = "Beef", Unit = "KG", Type = "Perishable" },
                new Item { ItemId = 8, Name = "Chili", Unit = "KG", Type = "Perishable" },
                new Item { ItemId = 9, Name = "Turmeric Powder", Unit = "KG", Type = "Non-perishable" },
                new Item { ItemId = 10, Name = "Ginger", Unit = "KG", Type = "Perishable" },
                new Item { ItemId = 11, Name = "Onion", Unit = "KG", Type = "Perishable" },
                new Item { ItemId = 12, Name = "Garlic", Unit = "KG", Type = "Perishable" },
                new Item { ItemId = 13, Name = "Sugar", Unit = "KG", Type = "Non-perishable" },
                new Item { ItemId = 14, Name = "Yogurt", Unit = "Litter", Type = "Perishable" },
                new Item { ItemId = 15, Name = "Milk", Unit = "Litter", Type = "Perishable" },
                new Item { ItemId = 16, Name = "Ketchup", Unit = "Litter", Type = "Perishable" },
                new Item { ItemId = 17, Name = "Raisin", Unit = "KG", Type = "Non-perishable" },
                new Item { ItemId = 18, Name = "Bay Leaves", Unit = "KG", Type = "Non-perishable" },
                new Item { ItemId = 19, Name = "Green Candamon", Unit = "KG", Type = "Non-perishable" },
                new Item { ItemId = 20, Name = "Cloves", Unit = "KG", Type = "Non-perishable" },
                new Item { ItemId = 21, Name = "Nut", Unit = "KG", Type = "Non-perishable" },
                new Item { ItemId = 22, Name = "Lentils", Unit = "KG", Type = "Non-perishable" },
                new Item { ItemId = 23, Name = "Capsicum", Unit = "KG", Type = "Perishable" },
                new Item { ItemId = 24, Name = "Peas", Unit = "KG", Type = "Non-perishable" },
                new Item { ItemId = 25, Name = "Butter", Unit = "KG", Type = "Perishable" },
                new Item { ItemId = 26, Name = "Egg", Unit = "Piece", Type = "Perishable" },
                new Item { ItemId = 27, Name = "Chicken", Unit = "KG", Type = "Perishable" },
                new Item { ItemId = 28, Name = "Corn flour", Unit = "KG", Type = "Non-perishable" },
                new Item { ItemId = 29, Name = "Tomato", Unit = "KG", Type = "Perishable" },
                new Item { ItemId = 30, Name = "Chili Powder", Unit = "KG", Type = "Non-perishable" },
                new Item { ItemId = 31, Name = "Garam Masala Powder", Unit = "KG", Type = "Non-perishable" },
                new Item { ItemId = 32, Name = "Cotton Fish", Unit = "KG", Type = "Perishable" },
                new Item { ItemId = 33, Name = "Shrimp Fish", Unit = "KG", Type = "Perishable" },
                new Item { ItemId = 34, Name = "Katla Fish", Unit = "KG", Type = "Perishable" },
                new Item { ItemId = 35, Name = "Hilsa Fish", Unit = "KG", Type = "Perishable" },
                new Item { ItemId = 36, Name = "Mustard", Unit = "KG", Type = "Non-perishable" },
                new Item { ItemId = 37, Name = "Coconut", Unit = "Piece", Type = "Perishable" },
                new Item { ItemId = 38, Name = "Ghee", Unit = "Litter", Type = "Perishable" },
                new Item { ItemId = 39, Name = "Cheery", Unit = "KG", Type = "Perishable" },
                new Item { ItemId = 40, Name = "Zafran", Unit = "KG", Type = "Non-perishable" }
                );

            builder.Entity<Recipe>().HasData(
                new Recipe { RecipeId = 1, RecipeName = "Plain Rice" },
                new Recipe { RecipeId = 2, RecipeName = "Kachchi" },
                new Recipe { RecipeId = 3, RecipeName = "Polao" },
                new Recipe { RecipeId = 4, RecipeName = "Biriyani" },
                new Recipe { RecipeId = 5, RecipeName = "Khicuri" },
                new Recipe { RecipeId = 6, RecipeName = "Fried Rice" },
                new Recipe { RecipeId = 7, RecipeName = "Beef Rezala" },
                new Recipe { RecipeId = 8, RecipeName = "Chicken Roast" },
                new Recipe { RecipeId = 9, RecipeName = "Spicy Dry Chicken" },
                new Recipe { RecipeId = 10, RecipeName = "Fried Chicken" },
                new Recipe { RecipeId = 11, RecipeName = "Mutton Curry" },
                new Recipe { RecipeId = 12, RecipeName = "Korma" },
                new Recipe { RecipeId = 13, RecipeName = "Kalia" },
                new Recipe { RecipeId = 14, RecipeName = "Shorshe Ilish" },
                new Recipe { RecipeId = 15, RecipeName = "Chingri Malai Curry" },
                new Recipe { RecipeId = 16, RecipeName = "Kala Bhuna" },
                new Recipe { RecipeId = 17, RecipeName = "Fish Curry" },
                new Recipe { RecipeId = 18, RecipeName = "Haleem" },
                new Recipe { RecipeId = 19, RecipeName = "Jorda" },
                new Recipe { RecipeId = 20, RecipeName = "Firni" }
                );

            builder.Entity<RecipeItem>().HasData(
                new RecipeItem { RecipeItemId = 1, RecipeId = 1, ItemId = 1, Quantity = 1},
                new RecipeItem { RecipeItemId = 2, RecipeId = 2, ItemId = 5, Quantity = 1 },
                new RecipeItem { RecipeItemId = 3, RecipeId = 2, ItemId = 3, Quantity = 1 },
                new RecipeItem { RecipeItemId = 4, RecipeId = 2, ItemId = 6, Quantity = 1 },
                new RecipeItem { RecipeItemId = 5, RecipeId = 2, ItemId = 9, Quantity = 1 },
                new RecipeItem { RecipeItemId = 6, RecipeId = 2, ItemId = 10, Quantity = 1 },
                new RecipeItem { RecipeItemId = 7, RecipeId = 2, ItemId = 11, Quantity = 1 },
                new RecipeItem { RecipeItemId = 8, RecipeId = 2, ItemId = 12, Quantity = 1 },
                new RecipeItem { RecipeItemId = 9, RecipeId = 2, ItemId = 13, Quantity = 1 },
                new RecipeItem { RecipeItemId = 10, RecipeId = 2, ItemId = 14, Quantity = 1 },
                new RecipeItem { RecipeItemId = 11, RecipeId = 2, ItemId = 16, Quantity = 1 },
                new RecipeItem { RecipeItemId = 12, RecipeId = 2, ItemId = 17, Quantity = 1 },
                new RecipeItem { RecipeItemId = 13, RecipeId = 2, ItemId = 18, Quantity = 1 },
                new RecipeItem { RecipeItemId = 14, RecipeId = 2, ItemId = 19, Quantity = 1 },
                new RecipeItem { RecipeItemId = 15, RecipeId = 2, ItemId = 20, Quantity = 1 },
                new RecipeItem { RecipeItemId = 16, RecipeId = 2, ItemId = 21, Quantity = 1 },
                new RecipeItem { RecipeItemId = 17, RecipeId = 2, ItemId = 30, Quantity = 1 },
                new RecipeItem { RecipeItemId = 18, RecipeId = 2, ItemId = 31, Quantity = 1 },
                new RecipeItem { RecipeItemId = 19, RecipeId = 2, ItemId = 38, Quantity = 1 },
                new RecipeItem { RecipeItemId = 20, RecipeId = 2, ItemId = 40, Quantity = 1 }
                );
        }
        public DbSet<OrderManagementClassLibrary.Models.SaleHeader> SaleHeader { get; set; } = default!;
    }
}
