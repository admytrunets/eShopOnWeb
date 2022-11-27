using Newtonsoft.Json;

namespace EShopOnWeb.DeliveryOrderProcessor.Models
{
    public sealed class CatalogItemOrdered
    {
        public CatalogItemOrdered(int catalogItemId, string productName, string pictureUri)
        {
            CatalogItemId = catalogItemId;
            ProductName = productName;
            PictureUri = pictureUri;
        }

        public CatalogItemOrdered()
        {
        }

        [JsonProperty("catalogItemId")]
        public int CatalogItemId { get; /*private*/ set; }

        [JsonProperty("productName")]
        public string ProductName { get; /*private*/ set; }

        [JsonProperty("pictureUri")]
        public string PictureUri { get; /*private*/ set; }
    }
}
