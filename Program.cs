using RPGItemsAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddDbContext<RPGContext>(options =>
    options.UseSqlite("Data Source=rpg.db"));

builder.Services.AddDbContext<RPGContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline
app.MapControllers();
app.Run();
