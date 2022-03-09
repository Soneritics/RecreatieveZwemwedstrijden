using System;
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
using Newtonsoft.Json;

namespace Api.Functions.Match;

public class AddMatch
{
    private readonly ILogger<AddMatch> _logger;
    private readonly IRepository _repository;

    public AddMatch(ILogger<AddMatch> log, IRepository repository)
    {
        _logger = log;
        _repository = repository;
    }

    [FunctionName(nameof(AddMatch))]
    [OpenApiOperation(operationId: "Run", tags: new[] { "matchId" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter(name: "matchId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **matchId** parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(global::Models.Match), Description = "The Match with the specified id.")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var match = JsonConvert.DeserializeObject<global::Models.Match>(await req.ReadAsStringAsync());

        match.Id = Guid.NewGuid().ToString();
        match.CleanPrograms();
        await _repository.InsertAsync(match.Id, match);

        return new JsonResult(match);
    }
}