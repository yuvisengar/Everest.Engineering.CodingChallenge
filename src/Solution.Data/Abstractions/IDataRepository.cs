using Everest.Engineering.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Everest.Engineering.Data.Abstractions
{
    public interface IDataRepository<T> where T : DbEntity
    {
        Task<T> Get(Guid id);
        Task<T> Find(Func<T, bool> predicate);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetWhere(Func<T, bool> predicate);
        Task Add(T entity);
        Task AddMany(IEnumerable<T> entities);
        Task Update(T entity);
        Task Update(IEnumerable<T> entities);
        Task Delete(T entity);
        Task Delete(IEnumerable<T> entities);
        Task Clean();
    }
}
