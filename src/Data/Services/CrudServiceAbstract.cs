using BadMelon.Data.Entities;
using BadMelon.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BadMelon.Data.Services
{
    public abstract class CrudServiceAbstract<T> : ICrudService<T> where T : Entity
    {
        protected readonly BadMelonDataContext _db;

        public CrudServiceAbstract(BadMelonDataContext db)
        {
            _db = db;
        }

        public virtual async Task<T> GetOne(Guid id)
        {
            var entity = await _db.Set<T>().SingleOrDefaultAsync(t => t.ID == id);
            if (entity == default(T))
                throw new EntityNotFoundException(typeof(T));
            return entity;
        }

        public async Task AddMany(ICollection<T> models)
        {
            await _db.Set<T>().AddRangeAsync(models);
            await _db.SaveChangesAsync();
        }

        public async Task AddOne(T model)
        {
            await _db.Set<T>().AddAsync(model);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(T model)
        {
            _db.Set<T>().Remove(model);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var entity = await GetOne(id);
            await Delete(entity);
        }

        public async Task<T[]> GetAll()
        {
            return await _db.Set<T>().ToArrayAsync();
        }

        //TODO: Check if this works
        public async Task<T> Update(T model)
        {
            _db.Set<T>().Update(model);
            await _db.SaveChangesAsync();
            return model;
        }
    }
}