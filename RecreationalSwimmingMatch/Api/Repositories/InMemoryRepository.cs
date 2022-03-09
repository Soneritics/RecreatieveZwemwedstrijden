using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Repositories;

public class InMemoryRepository : IRepository
{
    private Dictionary<string, object> _entities;

    public InMemoryRepository()
    {
        _entities = new Dictionary<string, object>();
    }

    public async Task<bool> ExistsAsync<T>(string id)
    {
        return _entities.ContainsKey(id);
    }

    public async Task InsertAsync<T>(string id, T entity)
    {
        if (await ExistsAsync<T>(id))
            throw new Exception("Document already exists");

        _entities[id] = entity;
    }

    public async Task UpdateAsync<T>(string id, T entity)
    {
        if (!await ExistsAsync<T>(id))
            throw new Exception("Document does not exist");

        _entities[id] = entity;
    }

    public async Task<T> GetAsync<T>(string id)
    {
        return (T)_entities[id];
    }

    public async Task<List<T>> GetListAsync<T>(Func<T, bool> predicate)
    {
        return _entities
            .Values
            .OfType<T>()
            .Where(predicate)
            .ToList();
    }
}