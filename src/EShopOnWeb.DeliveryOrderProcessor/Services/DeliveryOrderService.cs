using System;
using System.Threading.Tasks;
using EShopOnWeb.DeliveryOrderProcessor.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace EShopOnWeb.DeliveryOrderProcessor.Services
{
    internal sealed class DeliveryOrderService
    {
        private readonly string _endpointUri;
        private readonly string _primaryKey;
        private readonly string _databaseId;
        private readonly string _containerId;
        private readonly ILogger _log;

        private CosmosClient _cosmosClient;
        private Database _database;
        private Container _container;

        public DeliveryOrderService(string endpointUri, string primaryKey, string databaseId, string containerId, ILogger log)
        {
            _endpointUri = endpointUri;
            _primaryKey = primaryKey;
            _databaseId = databaseId;
            _containerId = containerId;
            _log = log;
        }

        public async Task SaveOrder(Order order)
        {
            try
            {
                await InitCosmosContainer();
                await AddItemToContainerAsync(order);
            }
            catch (CosmosException cosmosException)
            {
                _log.LogError($"Cosmos Exception with Status {cosmosException.StatusCode} : {cosmosException}");
            }
            catch (Exception e)
            {
                _log.LogError($"SaveOrder() has failed. Exception details: {e}");
            }
            finally
            {
                Cleanup();
            }
        }

        private async Task InitCosmosContainer()
        {
            _cosmosClient = new CosmosClient(_endpointUri, _primaryKey);

            _database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(_databaseId);
            _container = await _database.CreateContainerIfNotExistsAsync(_containerId, "/buyerId");
        }

        private async Task AddItemToContainerAsync(Order order)
        {
            ItemResponse<Order> orderResponse = await _container.CreateItemAsync<Order>(order, new PartitionKey(order.BuyerId));
            _log.LogInformation($"Created (orderId = {order.Id}, BuyerId = {order.BuyerId}) order in database with id: {orderResponse.Resource.Id}.");
        }

        private void Cleanup()
        {
            if (_cosmosClient != null)
            {
                _cosmosClient.Dispose();
                _cosmosClient = null;
                _container = null;
            }
        }
    }
}
