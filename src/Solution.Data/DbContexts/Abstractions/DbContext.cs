using Everest.Engineering.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Everest.Engineering.Data.DbContexts.Abstractions
{
    public abstract class DbContext<T> : IDbContext<T> where T: DbEntity
    {
        protected List<T> data { get; set; }

        public async virtual Task Add(T entity)
        {
            data.Add(entity);
            await Task.CompletedTask;
        }

        public async virtual Task AddRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                await Add(entity);
            }

            await Task.CompletedTask;
        }

        public async Task Clean()
        {
            this.data = new List<T>();
            await Task.CompletedTask;
        }

        public async virtual Task Delete(T entity)
        {
            var oldRecord = data.FirstOrDefault(x => x.Id == entity.Id);
            if (!(oldRecord is null))
            {
                var index = data.IndexOf(oldRecord);
                data.RemoveAt(index);
            }

            await Task.CompletedTask;
        }

        public async virtual Task DeleteRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                await Delete(entity);
            }

            await Task.CompletedTask;
        }

        public async virtual Task<T> Get(Guid entityId)
        {
            var entity = data.SingleOrDefault(x => x.Id == entityId);
            return await Task.FromResult(entity);
        }

        public async virtual Task<IEnumerable<T>> GetAll()
        {
            return await Task.FromResult(data);
        }

        public async virtual Task SeedData(IEnumerable<T> entities)
        {
            data = entities.ToList();
            await Task.CompletedTask;
        }

        public async virtual Task Update(T entity)
        {
            var oldRecord = data.FirstOrDefault(x => x.Id == entity.Id);
            if (!(oldRecord is null))
            {
                var index = data.IndexOf(oldRecord);
                data.RemoveAt(index);
                entity.LastModifiedAt = DateTime.Now;
                data.Add(entity);
            }

            await Task.CompletedTask;
        }

        public async virtual Task UpdateRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                await Update(entity);
            }

            await Task.CompletedTask;
        }
    }
}
