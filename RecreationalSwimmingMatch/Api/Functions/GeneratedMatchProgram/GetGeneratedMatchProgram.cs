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

public class GetGeneratedMatchProgram
{
    private readonly ILogger<GetGeneratedMatchProgram> _logger;
    private readonly IRepository _repository;

    public GetGeneratedMatchProgram(ILogger<GetGeneratedMatchProgram> log, IRepository repository)
    {
        _logger = log;
        _repository = repository;
    }

    [FunctionName(nameof(GetGeneratedMatchProgram))]
    [OpenApiOperation(operationId: "Run", tags: new[] { "matchId" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter(name: "matchId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **matchId** parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(global::Models.GeneratedMatchProgram), Description = "The GeneratedProgram for the specified match (id).")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        string matchId = req.Query["matchId"];
        var result = (await _repository
            .GetListAsync<global::Models.GeneratedMatchProgram>(r => r.MatchId.Equals(matchId)))
            ?.Single();

        return new JsonResult(result);
    }
}