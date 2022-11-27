using System;

namespace EShopOnWeb.DeliveryOrderProcessor.Models
{
    internal class ConfigSettings
    {
        private const string KeyNameCosmosDbUri = "CosmosDbUri";
        private const string KeyNamePrimaryKey = "CosmosDbPrimaryKey";
        private const string KeyNameDbName = "CosmosDbOrderDbName";
        private const string KeyNameContainerName = "CosmosDbOrderContainerName";

        public string CosmosDbUri { get; private set; }
        public string CosmosDbPrimaryKey { get; private set; }
        public string CosmosDbOrderDbName { get; private set; }
        public string CosmosDbOrderContainerName { get; private set; }

        public void Init()
        {
            CosmosDbUri = Environment.GetEnvironmentVariable(KeyNameCosmosDbUri);
            CosmosDbPrimaryKey = Environment.GetEnvironmentVariable(KeyNamePrimaryKey);
            CosmosDbOrderDbName = Environment.GetEnvironmentVariable(KeyNameDbName);
            CosmosDbOrderContainerName = Environment.GetEnvironmentVariable(KeyNameContainerName);
        }

        public void ValidateSettings()
        {
            if (string.IsNullOrWhiteSpace(CosmosDbUri))
            {
                throw new ArgumentNullException(nameof(CosmosDbUri));
            }

            if (string.IsNullOrWhiteSpace(CosmosDbPrimaryKey))
            {
                throw new ArgumentNullException(nameof(CosmosDbPrimaryKey));
            }

            if (string.IsNullOrWhiteSpace(CosmosDbOrderDbName))
            {
                throw new ArgumentNullException(nameof(CosmosDbOrderDbName));
            }

            if (string.IsNullOrWhiteSpace(CosmosDbOrderContainerName))
            { 
                throw new ArgumentNullException(nameof(CosmosDbOrderContainerName));
            }
        }
    }
}
