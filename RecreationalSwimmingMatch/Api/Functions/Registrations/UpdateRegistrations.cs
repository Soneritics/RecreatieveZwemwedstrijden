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

namespace Api.Functions.Registrations;

public class UpdateRegistrations
{
    private readonly ILogger<UpdateRegistrations> _logger;
    private readonly IRepository _repository;

    public UpdateRegistrations(ILogger<UpdateRegistrations> log, IRepository repository)
    {
        _logger = log;
        _repository = repository;
    }

    [FunctionName(nameof(UpdateRegistrations))]
    [OpenApiOperation(operationId: nameof(Run))]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(global::Models.Registrations), Description = "Status code indicating the result of the operation.")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var registrations = JsonConvert.DeserializeObject<global::Models.Registrations>(await req.ReadAsStringAsync());
        registrations.CleanRegistrationList();
        await _repository.UpdateAsync(registrations.Id, registrations);

        return new JsonResult(registrations);
    }
}