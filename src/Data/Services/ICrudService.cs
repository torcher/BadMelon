using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BadMelon.Data.Services
{
    public interface ICrudService<T>
    {
        Task<T> GetOne(Guid id);

        Task<T[]> GetAll();

        Task AddOne(T model);

        Task AddMany(ICollection<T> models);

        Task<T> Update(T model);

        Task Delete(T model);

        Task Delete(Guid id);
    }
}