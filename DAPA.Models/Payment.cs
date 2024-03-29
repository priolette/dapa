﻿using System.Text.Json.Serialization;

namespace DAPA.Models;

public class Payment
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    [JsonIgnore] public Order Order { get; set; } = null!;

    public DateTime Date { get; set; }

    public Method Method { get; set; }

    public float? Amount { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Method
{
    Card,
    Cash,
    Coupon
}