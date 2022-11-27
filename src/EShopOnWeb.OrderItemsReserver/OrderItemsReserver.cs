using System;
using System.IO;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace EShopOnWeb.OrderItemsReserver
{
    public sealed class OrderItemsReserver
    {
        [FunctionName("order-items-reserver")]
        public void Run(
            [ServiceBusTrigger("eshop-orders", Connection = "ServiceBusConnection")] string message,
            string messageId,
            int deliveryCount,
            DateTime enqueuedTimeUtc,
            [Blob("eshop-orders/{messageId}", FileAccess.Write)] Stream imageMedium,
            ILogger log)
        {
            log.LogInformation($"order-items-reserver(message: {message}, messageId: {messageId}, deliveryCount: {deliveryCount}");

            var bytes = Encoding.UTF8.GetBytes(message);

            try
            {
                imageMedium.Write(bytes);
            }
            catch (Exception e)
            {
                log.LogError($"Cannot save order in storage. Exception details: {e}");
                throw;
            }
        }
    }
}
