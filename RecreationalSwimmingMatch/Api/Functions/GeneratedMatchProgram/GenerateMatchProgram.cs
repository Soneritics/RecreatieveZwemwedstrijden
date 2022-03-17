using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Api.Generator;
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

public class GenerateMatchProgram
{
    private readonly ILogger<GenerateMatchProgram> _logger;
    private readonly IRepository _repository;

    public GenerateMatchProgram(ILogger<GenerateMatchProgram> log, IRepository repository)
    {
        _logger = log;
        _repository = repository;
    }

    [FunctionName(nameof(GenerateMatchProgram))]
    [OpenApiOperation(operationId: "Run", tags: new[] { "matchId" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter(name: "matchId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **matchId** parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The id of the document.")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        // Require a match id
        string matchId = req.Query["matchId"];
        if (string.IsNullOrEmpty(matchId))
        {
            return new BadRequestResult();
        }

        // First delete any existing
        var existing =
            (await _repository.GetListAsync<global::Models.GeneratedMatchProgram>(r => r.MatchId.Equals(matchId)));

        if (existing.Any())
        {
            foreach (var item in existing)
            {
                await _repository.DeleteAsync<global::Models.GeneratedMatchProgram>(item.Id);
            }
        }

        // Then fetch the necessary data
        var match = await _repository.GetAsync<global::Models.Match>(matchId);
        var registrations = await _repository
            .GetListAsync<global::Models.Registrations>(r => r.MatchId.Equals(matchId));

        // Then create a new one
        var generatedMatchProgramGenerator = new MatchProgramGenerator(
            match,
            registrations);

        var generatedMatchProgram = generatedMatchProgramGenerator.Generate();

        await _repository.InsertAsync(generatedMatchProgram.Id, generatedMatchProgram);

        // Return the id of the newly created program
        return new OkObjectResult(generatedMatchProgram.Id);
    }
}