using System.Net;
using Api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Api.Functions.Layout;

public class GetRepositoryType
{
    private readonly ILogger<BuildNumber> _logger;
    private readonly IRepository _repository;

    public GetRepositoryType(ILogger<BuildNumber> logger, IRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [FunctionName(nameof(GetRepositoryType))]
    [OpenApiOperation(operationId: nameof(GetRepositoryType))]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response with the name of the repository type")]
    public IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
    {
        _logger.LogInformation($"{nameof(BuildNumber)} triggered.");
        var repositoryName = _repository.GetType().Name;
        return new OkObjectResult(repositoryName);
    }
}