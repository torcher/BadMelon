using BadMelon.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BadMelon.Data
{
    public class BadMelonDataContext : DbContext
    {
        public BadMelonDataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<IngredientType> IngredientTypes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Step> Steps { get; set; }
    }
}