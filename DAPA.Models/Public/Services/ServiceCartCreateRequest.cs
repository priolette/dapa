﻿namespace DAPA.Models.Public.Services;

public class ServiceCartCreateRequest
{
    public int OrderId { get; set; }

    public int ServiceId { get; set; }

    public int StaffId { get; set; }

    public int Quantity { get; set; }
}