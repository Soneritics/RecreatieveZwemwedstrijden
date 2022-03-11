using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Models;
using Newtonsoft.Json;

namespace Api.Repositories;

public class StorageRepository : IRepository
{
    private const string ContainerName = "matchdata";

    private readonly BlobContainerClient _containerClient;

    public StorageRepository(string connectionString)
    {
        var blobServiceClient = new BlobServiceClient(
            connectionString,
            new BlobClientOptions(BlobClientOptions.ServiceVersion.V2019_12_12));

        _containerClient = blobServiceClient.GetBlobContainerClient(ContainerName);
        _containerClient.CreateIfNotExists();
    }

    public async Task<bool> ExistsAsync<T>(string id)
    {
        var blobClient = _containerClient.GetBlobClient(GetFilename<T>(id));
        return await blobClient.ExistsAsync();
    }

    public async Task InsertAsync<T>(string id, T entity)
    {
        var blobClient = _containerClient.GetBlobClient(GetFilename<T>(entity));
        if (await blobClient.ExistsAsync())
            throw new Exception("Document already exists");

        var document = JsonConvert.SerializeObject(entity);
        await blobClient.UploadAsync(
            await new StringContent(document).ReadAsStreamAsync());
    }

    public async Task UpdateAsync<T>(string id, T entity)
    {
        var blobClient = _containerClient.GetBlobClient(GetFilename<T>(entity));
        if (!await blobClient.ExistsAsync())
            throw new Exception("Document does not exist");

        var document = JsonConvert.SerializeObject(entity);
        await blobClient.UploadAsync(
            await new StringContent(document).ReadAsStreamAsync(),
            true);
    }

    public async Task<T> GetAsync<T>(string id)
    {
        return await GetFromBlobNameAsync<T>(GetFilename<T>(id));
    }

    public async Task<List<T>> GetListAsync<T>(Func<T, bool> predicate)
    {
        var completeList = new List<T>();

        var blobPath = $"{typeof(T).Name}";
        var blobs = _containerClient.GetBlobsAsync(BlobTraits.None, BlobStates.None, blobPath);
        await foreach (var blob in blobs)
        {
            completeList.Add(
                await GetFromBlobNameAsync<T>(blob.Name));
        }

        return completeList
            .Where(predicate)
            .ToList();
    }

    private async Task<T> GetFromBlobNameAsync<T>(string blobName)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);
        return (await blobClient.DownloadContentAsync())
            .Value
            .Content
            .ToObjectFromJson<T>();
    }

    private string GetFilename<T>(object document)
    {
        var result = typeof(T).Name;

        if (document is string)
        {
            return $"{result}/{(string)document}.json";
        }
        else if (document is T)
        {
            switch (typeof(T).Name)
            {
                case nameof(Match):
                    var match = (Match) document;
                    result = $"{result}/{match.Id}";
                    break;

                case nameof(Registrations):
                    var registrations = (Registrations) document;
                    result = $"{result}/{registrations.Id}";
                    break;

                default:
                    throw new InvalidOperationException($"Cannot save type {typeof(T).Name}");
            }

            return $"{result}.json";
        }

        throw new InvalidOperationException($"Cannot save document with type {typeof(T).Name}");
    }
}