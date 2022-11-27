using System;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;

namespace Microsoft.eShopWeb.ApplicationCore.Services
{
    public sealed class OrderItemsReserverService : IOrderItemsReserverService
    {
        private readonly string ServiceBusConnectionString;
        private readonly string ServiceBusQueueName;
        private readonly string DeliveryOrderProcessorUrl;

        private readonly ILogger<OrderItemsReserverService> _logger;

        public OrderItemsReserverService(IConfiguration configuration, ILogger<OrderItemsReserverService> logger)
        {
            if (!string.IsNullOrWhiteSpace(configuration["ServiceBusConnectionString"]))
            {
                ServiceBusConnectionString = configuration["ServiceBusConnectionString"];
            }

            if (!string.IsNullOrWhiteSpace(configuration["ServiceBusQueueName"]))
            {
                ServiceBusQueueName = configuration["ServiceBusQueueName"];
            }

            if (!string.IsNullOrWhiteSpace(configuration["DeliveryOrderProcessorUrl"]))
            {
                DeliveryOrderProcessorUrl = configuration["DeliveryOrderProcessorUrl"];
            }

            _logger = logger;
        }

        public async Task SendOrderMessageAsync(string order)
        {
            await using var client = new ServiceBusClient(ServiceBusConnectionString);
            await using var sender = client.CreateSender(ServiceBusQueueName);
            
            try
            {
                var messageBody = order;
                var message = new ServiceBusMessage(messageBody);

                _logger.LogInformation($"Sending order: {messageBody}");

                await sender.SendMessageAsync(message);
            }
            catch (Exception exception)
            {
                _logger.LogError($"SendOrderMessageAsync has failed. Exception details: {exception.Message}");
            }
        }

        public async Task NotifyOrderDeliveryService(Order order)
        {
            var orderJson = JsonSerializer.Serialize(order);
            var content = new StringContent(orderJson, Encoding.UTF8, "applicaiton/json");
            
            using (var httpClient = new HttpClient())
            {
                await httpClient.PostAsync(DeliveryOrderProcessorUrl, content);
            }
        }
    }
}
