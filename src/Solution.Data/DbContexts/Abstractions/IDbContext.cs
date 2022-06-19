using Everest.Engineering.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Everest.Engineering.Data.DbContexts.Abstractions
{
    public interface IDbContext<T> where T: DbEntity
    {
        Task SeedData(IEnumerable<T> entities);

        Task<IEnumerable<T>> GetAll();

        Task<T> Get(Guid Id);

        Task Add(T entity);

        Task AddRange(IEnumerable<T> entities);

        Task Update(T entity);

        Task UpdateRange(IEnumerable<T> entities);

        Task Delete(T entity);

        Task DeleteRange(IEnumerable<T> entities);
        Task Clean();
    }
}
