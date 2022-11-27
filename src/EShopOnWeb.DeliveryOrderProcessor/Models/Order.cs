using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace EShopOnWeb.DeliveryOrderProcessor.Models;

public sealed class Order
{
    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonProperty("buyerId")]
    public string BuyerId { get; set; }

    [JsonProperty("orderDate")]
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

    [JsonProperty("shipToAddress")]
    public Address ShipToAddress { get; set; }

    [JsonProperty("orderItems")]
    public IList<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [JsonProperty("totalCost")]
    public decimal TotalCost
    {
        get
        {
            decimal totalCost = 0;

            foreach (var item in OrderItems)
            {
                totalCost += item.UnitPrice * item.Units;
            }

            return totalCost;
        }
    }

    public Order()
    {
    }

    public Order(string buyerId, Address shipToAddress, List<OrderItem> items)
    {
        BuyerId = buyerId;
        ShipToAddress = shipToAddress;
        OrderItems = items;
    }
}
