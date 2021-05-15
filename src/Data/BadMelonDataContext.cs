using BadMelon.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BadMelon.Data
{
    public class BadMelonDataContext : IdentityDbContext<User>
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

        public async Task Seed()
        {
            if (!await Users.AnyAsync())
            {
                var data = new DataSamples();
                await Users.AddRangeAsync(data.Users.Select(u => u.Item1).ToArray());
                await IngredientTypes.AddRangeAsync(data.IngredientTypes);
                await Recipes.AddRangeAsync(data.Recipes);
                await SaveChangesAsync();
            }
        }
    }
}