using FMS.Db;
using FMS.Repositories;
using FMS.Repositories.EF;
using FMS.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FMS
{
    public class Program
    {
        const string COMPANY_NAME = "RoyalPizza";
        const string PRODUCT_NAME = "FMS ASP.NET MVC";

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
            builder.Services.AddControllersWithViews();
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
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
