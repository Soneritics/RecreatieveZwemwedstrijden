using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Api.Functions;

public class BuildNumber
{
    private readonly ILogger<BuildNumber> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public BuildNumber(ILogger<BuildNumber> log, IHttpClientFactory httpClientFactory)
    {
        _logger = log;
        _httpClientFactory = httpClientFactory;
    }

    [FunctionName(nameof(BuildNumber))]
    [OpenApiOperation(operationId: nameof(BuildNumber))]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response with the build number")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
    {
        _logger.LogInformation($"{nameof(BuildNumber)} triggered.");

        var responseMessage = "beta";
        var githubUrl = "https://api.github.com/repos/Soneritics/RecreatieveZwemwedstrijden/releases/latest";

        var client = _httpClientFactory.CreateClient(nameof(BuildNumber));
        client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(nameof(Api), "1.0"));

        try
        {
            var getResult = await client.GetFromJsonAsync<BuildNumberApiResponse>(githubUrl);

            if (getResult?.TagName != default)
                responseMessage = getResult.TagName;
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error while fetching build nr: {ex.Message}", ex);
        }
        
        return new OkObjectResult(responseMessage);
    }
}