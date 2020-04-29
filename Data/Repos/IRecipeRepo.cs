using BadMelon.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BadMelon.Data.Repos
{
    public interface IRecipeRepo
    {
        Task<Recipe[]> GetAll();
        Task<Recipe> GetOne(Guid ID);
    }
}
