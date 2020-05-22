using BadMelon.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BadMelon.Data
{
    public class BadMelonDataContext : DbContext
    {
        public BadMelonDataContext()
        {
        }

        public BadMelonDataContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Recipe> Recipes { get; set; }
        public virtual DbSet<IngredientType> IngredientTypes { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }
        public virtual DbSet<Step> Steps { get; set; }
    }
}