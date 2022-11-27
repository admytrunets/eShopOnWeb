using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace EShopOnWeb.DeliveryOrderProcessor.Models
{
    public sealed class OrderItem
    {
        [JsonPropertyName("id")]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("itemOrdered")]
        public CatalogItemOrdered ItemOrdered { get; set; }

        [JsonProperty("unitPrice")]
        public decimal UnitPrice { get; set; }

        [JsonProperty("units")]
        public int Units { get; set; }

        public OrderItem()
        {
        }

        public OrderItem(CatalogItemOrdered itemOrdered, decimal unitPrice, int units)
        {
            ItemOrdered = itemOrdered;
            UnitPrice = unitPrice;
            Units = units;
        }
    }
}
