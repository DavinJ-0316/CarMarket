﻿using MongoDB.Entities;
namespace SearchService.Model;

public class Item : Entity
{
    public int ReservePrice { get; set; }

    public string Seller { get; set; }

    public string Winner { get; set; }

    public int SoldAmount { get; set; }

    public int CurrentHighBid { get; set; }

// UtcNow is world standard time and posgresql asks for UTC TIME    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    
    public DateTime AuctionEnd { get; set; }

    public String Status { get; set; }

    public string Make { get; set; }

    public string Model { get; set; }

    public int Year  { get; set; }

    public string Color { get; set; }

    public int Mileage { get; set; }

    public string ImageUrl { get; set; }
}
