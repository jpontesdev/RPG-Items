using Microsoft.EntityFrameworkCore;
using RPG_Items.Core.Models;

namespace RPG_Items.Core.Data;
public class RPGContext : DbContext
{
    public RPGContext(DbContextOptions<RPGContext> options) : base(options) { }

    public DbSet<Item> Items { get; set; }
}
