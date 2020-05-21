﻿using BadMelon.API.DTOs;
using System;
using System.Threading.Tasks;

namespace BadMelon.API.Services
{
    public interface IRecipeService
    {
        Task<Recipe[]> GetRecipes();

        Task<Recipe> GetRecipeByID(Guid ID);

        Task<Recipe> AddRecipe(Recipe recipe);
    }
}