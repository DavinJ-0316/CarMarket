namespace ActionService.Entities;

public class Auction
{
    public Guid Id { get; set; }

    public int ReservePrice { get; set; } = 0;

    public string Seller { get; set; }

    public string Winner { get; set; }

    public int? SoldAmount { get; set; }

    public int? CurrentHighBid { get; set; }

// UtcNow is world standard time and posgresql asks for UTC TIME    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime AuctionEnd { get; set; } = DateTime.UtcNow;

    public Status Status { get; set; }
  // nav properties ===> "foreign key" 1 to 1 relationship between auction and item
    public Item Item { get; set; }

}
