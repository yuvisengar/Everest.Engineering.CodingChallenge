using Everest.Engineering.Data.DbContexts.Abstractions;
using Everest.Engineering.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Everest.Engineering.Data.Abstractions
{
    public abstract class DataRepository<T> : IDataRepository<T> where T : DbEntity
    {
        protected IDbContext<T> dbContext;

        public async virtual Task Add(T entity)
        {
            await dbContext.Add(entity);
            await Task.CompletedTask;
        }

        public async virtual Task AddMany(IEnumerable<T> entities)
        {
            await dbContext.AddRange(entities);
            await Task.CompletedTask;
        }

        public async virtual Task Clean()
        {
            await dbContext.Clean();
            await Task.CompletedTask;
        }

        public async virtual Task Delete(T entity)
        {
            await dbContext.Delete(entity);
            await Task.CompletedTask;
        }

        public async virtual Task Delete(IEnumerable<T> entities)
        {
            await dbContext.DeleteRange(entities);
            await Task.CompletedTask;
        }

        public async virtual Task<T> Find(Func<T, bool> predicate)
        {
            return (await dbContext.GetAll()).ToList().FirstOrDefault(predicate);
        }

        public async virtual Task<T> Get(Guid id)
        {
            return await dbContext.Get(id);
        }

        public async virtual Task<IEnumerable<T>> GetAll()
        {
            return await dbContext.GetAll();
        }

        public async virtual Task<IEnumerable<T>> GetWhere(Func<T, bool> predicate)
        {
            return (await dbContext.GetAll()).ToList().Where(predicate).ToList();
        }

        public async virtual Task Update(T entity)
        {
            await dbContext.Update(entity);
            await Task.CompletedTask;
        }

        public async virtual Task Update(IEnumerable<T> entities)
        {
            await dbContext.UpdateRange(entities);
            await Task.CompletedTask;
        }
    }
}
