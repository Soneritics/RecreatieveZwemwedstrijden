using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Models;

namespace Api.Repositories;

public class CosmosRepository : IRepository
{
    private readonly Container _container;

    public CosmosRepository(string connectionString, string databaseName, string containerName)
    {
        var cosmosClient = new CosmosClient(connectionString);
        var database = cosmosClient.GetDatabase(databaseName);
        _container = database.GetContainer(containerName);
    }

    public async Task<bool> ExistsAsync<T>(string id)
    {
        try
        {
            await GetAsync<T>(id);
            return true;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }
    }

    public async Task InsertAsync<T>(string id, T entity)
    {
        var doc = new CosmosDocument<T>()
        {
            Id = id,
            Data = entity
        };

        await _container.UpsertItemAsync(doc, new PartitionKey(id));
    }

    public async Task UpdateAsync<T>(string id, T entity)
    {
        var doc = new CosmosDocument<T>()
        {
            Id = id,
            Data = entity
        };

        await _container.UpsertItemAsync(doc, new PartitionKey(id));
    }

    public async Task<T> GetAsync<T>(string id)
    {
        var result = await _container
            .ReadItemAsync<CosmosDocument<T>>(id, new PartitionKey(id));

        return result.Resource.Data;
    }

    public async Task DeleteAsync<T>(string id)
    {
        await _container.DeleteItemAsync<CosmosDocument<T>>(id, new PartitionKey(id));
    }

    public async Task<List<T>> GetListAsync<T>(Func<T, bool> predicate)
    {
        return _container.GetItemLinqQueryable<CosmosDocument<T>>(true)
            .Where(r => r.Type.Equals(typeof(T).Name))
            .Select(r => r.Data)
            .Where(predicate)
            .ToList();
    }
}