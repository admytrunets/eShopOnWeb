using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EShopOnWeb.DeliveryOrderProcessor
{
    public static class TestFunction
    {
        [FunctionName("TestFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", "POST", Route = null)] HttpRequest req,
            ExecutionContext context,
            ILogger log)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            
            var value = configurationBuilder.GetConnectionStringOrSetting("CosmosDb:Uri");
            log.LogInformation($"CosmosDb:Uri = {value}");

            var value1 = configurationBuilder.GetConnectionStringOrSetting("CosmosDbUri");
            log.LogInformation($"CosmosDbUri = {value1}");

            return new OkObjectResult($"Hello, {value}");
        }
    }
}
