using System;
using System.IO;
using System.Threading.Tasks;
using EShopOnWeb.DeliveryOrderProcessor.Models;
using EShopOnWeb.DeliveryOrderProcessor.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EShopOnWeb.DeliveryOrderProcessor
{
    public static class DeliveryOrderProcessor
    {
        [FunctionName("DeliveryOrderProcessor")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", "POST", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("OrderItemsReserver.Run() started");

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                Order order = JsonConvert.DeserializeObject<Order>(requestBody);

                var configuration = new ConfigSettings();
                configuration.Init();
                configuration.ValidateSettings();

                var cosmosDbService = new DeliveryOrderService(
                    configuration.CosmosDbUri,
                    configuration.CosmosDbPrimaryKey,
                    configuration.CosmosDbOrderDbName,
                    configuration.CosmosDbOrderContainerName,
                    log);

                await cosmosDbService.SaveOrder(order);
            }
            catch (Exception e)
            {
                log.LogError($"OrderItemsReserver.Run() has failed. Exception details: {e}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return new OkObjectResult("Success");
        }
    }
}
