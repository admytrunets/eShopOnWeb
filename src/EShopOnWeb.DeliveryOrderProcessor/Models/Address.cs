using Newtonsoft.Json;

namespace EShopOnWeb.DeliveryOrderProcessor.Models
{
    public sealed class Address
    {
        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("zipCode")]
        public string ZipCode { get; set; }

        public Address()
        {
        }
    }
}
