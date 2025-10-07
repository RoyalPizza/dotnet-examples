using Microsoft.EntityFrameworkCore;

namespace MEC.Db;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Flight> Flights { get; set; }
    public DbSet<Carrier> Carriers { get; set; }
    public DbSet<Pier> Piers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Carrier>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(3);
            entity.HasIndex(e => e.Code).IsUnique();
        });

        modelBuilder.Entity<Pier>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Code).IsRequired();
            entity.HasIndex(e => e.Code).IsUnique();
        });

        modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.FlightNumber).IsRequired().HasMaxLength(12);
            entity.Property(e => e.AirportCode).IsRequired().HasMaxLength(3);
            entity.Property(e => e.DepartTime).IsRequired();
            entity.HasOne<Carrier>()
                .WithMany()
                .HasForeignKey(e => e.CarrierKey)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<Pier>()
                .WithMany()
                .HasForeignKey(e => e.PierKey)
                .OnDelete(DeleteBehavior.Restrict);
        });

        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        var carriers = new[]
        {
            new Carrier { Id = 1, Name = "Delta Air Lines", Code = "DL" },
            new Carrier { Id = 2, Name = "United Airlines", Code = "UA" },
            new Carrier { Id = 3, Name = "American Airlines", Code = "AA" },
            new Carrier { Id = 4, Name = "Southwest Airlines", Code = "WN" },
            new Carrier { Id = 5, Name = "Alaska Airlines", Code = "AS" },
            new Carrier { Id = 6, Name = "JetBlue Airways", Code = "B6" }
        };
        modelBuilder.Entity<Carrier>().HasData(carriers);

        var piers = new[]
        {
            new Pier { Id = 1, Name = "Pier A", Code = 1 },
            new Pier { Id = 2, Name = "Pier B", Code = 2 },
            new Pier { Id = 3, Name = "Pier C", Code = 3 },
            new Pier { Id = 4, Name = "Pier D", Code = 4 },
            new Pier { Id = 5, Name = "Pier E", Code = 5 },
            new Pier { Id = 6, Name = "Pier F", Code = 6 }
        };
        modelBuilder.Entity<Pier>().HasData(piers);

        var flights = new List<Flight>();
        var airportCodes = new[] { "ATL", "ORD", "DFW", "LAX", "JFK", "SFO" };
        var random = new Random(42); // Consistent seed for reproducibility
        var baseDate = DateTime.Today;

        for (var i = 1; i <= 200; i++)
        {
            var carrierId = random.Next(1, carriers.Last().Id);
            var pierId = random.Next(1, piers.Last().Id);
            var airportCode = airportCodes[random.Next(airportCodes.Length)];
            var carrierCode = carriers.First(c => c.Id == carrierId).Code;
            var flightNumber = $"{carrierCode}{random.Next(100, 9999):D4}";
            var departTime = baseDate.AddHours(random.Next(0, 20));

            flights.Add(new Flight
            {
                Id = i,
                FlightNumber = flightNumber,
                CarrierKey = carrierId,
                AirportCode = airportCode,
                PierKey = pierId,
                DepartTime = departTime
            });
        }

        modelBuilder.Entity<Flight>().HasData(flights);
    }
}