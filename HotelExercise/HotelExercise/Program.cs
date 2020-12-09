using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

//Habe leider nicht mehr als die Tables geschafft, da ich sehr große Probleme mit meiner Datenbank
//und dem erstellen der Tables gehabt habe obwohl ich das bei den anderen Programmen schnell 
//geschafft habe. Ich habe natürlich auch zu spät angefangen aber da ich sonst jede HÜ gemacht habe
//hoffe ich, dass es kein Problem ist, dass ich dieses mal nur die Tables gemacht habe.

var factory = new HotelContextFactory();
using var dbContext = factory.CreateDbContext();



#region Model
enum Special        
{
    Spa,
    Sauna,
    DogFriendly,
    IndoorPool,
    OutdoorPool,
    BikeRental,
    ECarChargingStation,
    VegetarianCuisine,
    OrganicFood
}
class Hotel
{
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; } = "";
    [MaxLength(100)]
    public string Address { get; set; } = "";
    public List<HotelSpecial> Special { get; set; } = new();
    public List<RoomType> RoomTypes { get; set; } = new();
}
class HotelSpecial
{
    public int Id { get; set; }
    public Special Special { get; set; }
    public Hotel? Hotel { get; set; }
}
class RoomType
{
    public int Id { get; set; }
    [MaxLength(75)]
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Size { get; set; }
    public bool IsDisabilityAccessible { get; set; }
    public int NummberOfAvailableRooms { get; set; }
    public Hotel? Hotel { get; set; }
    public int HotelId { get; set; }
    public Price? Price { get; set; }
}
class Price
{
    public int Id { get; set; }
    public RoomType? RoomType { get; set; }
    public int RoomTypeId { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    [Column(TypeName = "decimal(8, 2)")]
    public decimal PriceEUR { get; set; }
}
#endregion

#region Context
class HotelContext : DbContext
{
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<HotelSpecial> HotelSpecial { get; set; }
    public DbSet<RoomType> RoomTypes { get; set; }
    public DbSet<Price> Prices { get; set; }
#pragma warning disable CS8618
    public HotelContext(DbContextOptions<HotelContext> options) : base(options) { }
#pragma warning restore CS8618
}

class HotelContextFactory : IDesignTimeDbContextFactory<HotelContext>
{
    public HotelContext CreateDbContext(string[]? args = null)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        var optionsBuilder = new DbContextOptionsBuilder<HotelContext>();
        optionsBuilder
            // Uncomment the following line if you want to print generated
            // SQL statements on the console.
            //.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
            .UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);

        return new HotelContext(optionsBuilder.Options);
    }
}
#endregion