using Microsoft.EntityFrameworkCore;
using RPGItemsAPI.Models;

namespace RPGItemsAPI.Data;

public class RPGContext : DbContext
{
    public RPGContext(DbContextOptions<RPGContext> options) : base(options) { }

    public DbSet<Item> Items { get; set; }
}
