using System.Net;
using System.Threading.Tasks;
using Api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Models;

namespace Api.Functions;

public class MatchExists
{
    private readonly ILogger<MatchExists> _logger;
    private readonly IRepository _repository;

    public MatchExists(ILogger<MatchExists> log, IRepository repository)
    {
        _logger = log;
        _repository = repository;
    }

    [FunctionName(nameof(MatchExists))]
    [OpenApiOperation(operationId: "Run", tags: new[] { "matchId" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter(name: "matchId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **matchId** parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response. 1 = true, 0 = false.")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        string matchId = req.Query["matchId"];
        var result = false;

        if (!string.IsNullOrEmpty(matchId))
        {
            result = await _repository.ExistsAsync<Match>(matchId);
        }

        return new OkObjectResult(result ? "1" : "0");
    }
}