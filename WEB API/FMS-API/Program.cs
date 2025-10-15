
using FMS_API.Db;
using FMS_API.Repositories;
using FMS_API.Repositories.EF;
using FMS_API.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;

namespace FMS_API
{
    public class Program
    {
        const string COMPANY_NAME = "RoyalPizza";
        const string PRODUCT_NAME = "FMS ASP.NET WEB API";

        public static void Main(string[] args)
        {
#if DEBUG
            string dataPath = AppDomain.CurrentDomain.BaseDirectory;
#else
            string dataPath = @$"C:\ProgramData\{COMPANY_NAME}\{PRODUCT_NAME}";
            // TODO: implement user for this app, and ensure it only has access to the data path
            // TODO: ensure data path exists
#endif

            string logsPath = Path.Combine(dataPath, "logs/logs.txt");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File(logsPath, rollingInterval: RollingInterval.Day, shared: true)
                .Enrich.FromLogContext()
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            builder.Host.UseSerilog();

            // Register DbContext with dynamic connection string
            string connectionString = @$"Data Source={dataPath}/fms.db";
            builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
            {
                options.UseSqlite(connectionString);
            });

            // Register repositories
            builder.Services.AddScoped<IRepository<Carrier>, EFCarrierRepository>();
            builder.Services.AddScoped<IRepository<Pier>, EFPierRepository>();
            builder.Services.AddScoped<IRepository<Flight>, EFFlightRepository>();

            // Register services
            builder.Services.AddScoped<IService<Carrier>, CarrierService>();
            builder.Services.AddScoped<IService<Pier>, PierService>();
            builder.Services.AddScoped<IService<Flight>, FlightService>();

            var app = builder.Build();

            // Ensure database is created
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.EnsureCreated();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(options =>
                {
                    options.WithTheme(ScalarTheme.BluePlanet);
                });
            }

            app.UseHttpsRedirection();

            //app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
