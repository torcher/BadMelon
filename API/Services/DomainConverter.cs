using BadMelon.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BadMelon.API.Services
{
    public static class DomainConverter
    {
        public static Recipe[] ConvertToDomain(this Data.Entities.Recipe[] recipes)
        {
            return recipes.Select(recipe => recipe.ConvertToDomain()).ToArray();
        }

        public static Recipe ConvertToDomain(this Data.Entities.Recipe recipe)
        {
            return new Recipe
            {
                ID = recipe.ID,
                Name = recipe.Name
            };
        }
    }
}
