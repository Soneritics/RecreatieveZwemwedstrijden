using System;
using Api.Repositories;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Api.Startup))]

namespace Api;

internal class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddHttpClient();

        switch (Environment.GetEnvironmentVariable("Repository")?.ToLowerInvariant() ?? string.Empty)
        {
            case "storage":
                builder.Services.AddSingleton<IRepository>(sp =>
                    new StorageRepository(Environment.GetEnvironmentVariable("AzureWebJobsStorage")));
                break;

            default:
                builder.Services.AddSingleton<IRepository, InMemoryRepository>();
                break;
        }
    }
}
