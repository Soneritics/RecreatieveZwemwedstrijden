using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Repositories;

public interface IRepository
{
    Task<bool> ExistsAsync<T>(string id);
    Task InsertAsync<T>(string id, T entity);
    Task UpdateAsync<T>(string id, T entity);
    Task<T> GetAsync<T>(string id);
    Task<List<T>> GetListAsync<T>(Func<T, bool> predicate);
}