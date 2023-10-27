using System.ComponentModel.DataAnnotations.Schema;

namespace ActionService.Entities;

[Table("Items")]
public class Item
{
    public Guid id { get; set; }

    public string Make { get; set; }

    public string Model { get; set; }

    public int Year  { get; set; }

    public string Color { get; set; }

    public int Mileage { get; set; }

    public string ImageUrl { get; set; }

    // nav properties ===> "foreign key" 1 to 1 relationship between auction and item

    public Auction Auction { get; set; }

    public Guid AuctionId { get; set; }
}
