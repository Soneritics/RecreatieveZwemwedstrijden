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

namespace Api.Functions.GeneratedMatchProgram;

public class UpdateGeneratedMatchProgram
{
    private readonly ILogger<global::Models.GeneratedMatchProgram> _logger;
    private readonly IRepository _repository;

    public UpdateGeneratedMatchProgram(ILogger<global::Models.GeneratedMatchProgram> log, IRepository repository)
    {
        _logger = log;
        _repository = repository;
    }

    [FunctionName(nameof(UpdateGeneratedMatchProgram))]
    [OpenApiOperation(operationId: nameof(Run))]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(global::Models.GeneratedMatchProgram), Description = "The GeneratedMatchProgram with the specified id.")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var matchProgram = JsonConvert.DeserializeObject<global::Models.GeneratedMatchProgram>(await req.ReadAsStringAsync());
        await _repository.UpdateAsync(matchProgram.Id, matchProgram);

        return new JsonResult(matchProgram);
    }
}