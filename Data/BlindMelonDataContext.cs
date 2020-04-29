using BadMelon.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BadMelon.Data
{
    public class BadMelonDataContext : DbContext
    {
        public BadMelonDataContext(DbContextOptions options) : base(options) { }

        public DbSet<Recipe> Recipes { get; set; }
    }
}
