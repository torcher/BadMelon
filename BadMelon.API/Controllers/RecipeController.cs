using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BadMelon.API.Models;
using Microsoft.AspNetCore.Mvc;


namespace BadMelon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class RecipeController : ControllerBase
    {
        private static readonly Recipe[] Recipes = new Recipe[]
            {
                new Recipe{ ID = System.Guid.NewGuid(), Name = "Potato Soup", Ingredients = $"Potatos{Environment.NewLine}Soup", Steps = $"Mix and serve"},
                new Recipe{ ID = System.Guid.NewGuid(), Name = "Bacon", Ingredients = $"Bacon", Steps = $"Cook bacon."},
                new Recipe{ ID = System.Guid.NewGuid(), Name = "Sandwich", Ingredients = $"Bread{Environment.NewLine}Innards", Steps = $"Mix ad serve"}
            };

        [HttpGet]
        public IEnumerable<Recipe> Get()
        {
            return Recipes;
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public Recipe Get(Guid id)
        {
            return Recipes.SingleOrDefault(recipe => recipe.ID == id);
        }
    }
}
