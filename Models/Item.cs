using System.ComponentModel.DataAnnotations;

namespace RPGItemsAPI.Models;
public class Item
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; }

    [Required]
    [Range(1, 5)]
    public int Rarity { get; set; }

    [Required]
    [Range(0.01, 9999.99)]
    public decimal Price { get; set; }
}
