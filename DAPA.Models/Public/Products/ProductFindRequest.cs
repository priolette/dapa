﻿namespace DAPA.Models.Public.Products;

public class ProductFindRequest
{
    public int? Id { get; set; }

    public string? Title { get; set; }

    public float? Price { get; set; }

    public string? Description { get; set; }

    public int? DiscountId { get; set; }

    public string? Category { get; set; }
}