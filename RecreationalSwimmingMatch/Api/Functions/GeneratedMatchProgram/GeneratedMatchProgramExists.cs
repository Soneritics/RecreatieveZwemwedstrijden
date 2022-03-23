using System.Linq;
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

namespace Api.Functions.GeneratedMatchProgram;

public class GeneratedMatchProgramExists
{
    private readonly ILogger<global::Models.GeneratedMatchProgram> _logger;
    private readonly IRepository _repository;

    public GeneratedMatchProgramExists(ILogger<global::Models.GeneratedMatchProgram> log, IRepository repository)
    {
        _logger = log;
        _repository = repository;
    }

    [FunctionName(nameof(GeneratedMatchProgramExists))]
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
            result = (await _repository
                .GetListAsync<global::Models.GeneratedMatchProgram>(r => r.MatchId?.Equals(matchId) == true))
                ?.Any() == true;
        }

        return new OkObjectResult(result ? "1" : "0");
    }
}