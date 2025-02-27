﻿using Shared.Models;

namespace Shared.DTOs;

public class FarmCreationDto
{
    public string Name { get; set; }
    public Address Address { get; set; }
    public string Phone { get; set; }
    
    public int DeliveryDistance { get; set; }
    
    public string? FarmStatus { get; set; }

    public string FarmerUsername { get; set; }
    
    public Farmer Farmer { get; set; }
    
    /// <summary>
    /// File name of the image (for example: "default.png")
    /// </summary>
    public string? FarmIconFileName { get; set; }
}