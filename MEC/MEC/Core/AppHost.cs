using MEC.Db;
using MEC.Provider;
using MEC.Settings;
using MEC.Sorters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;

namespace MEC.Core;

internal sealed class AppHost : IDisposable
{
    public static readonly AppHost shared = new();
    private readonly ILogger<AppHost> _logger;

    private readonly ILoggerFactory _loggerFactory;
    private readonly Dictionary<Type, object> _services;

    public AppHost()
    {
        var serilogLogger = new LoggerConfiguration()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day, shared: true)
            .CreateLogger();

        _loggerFactory = LoggerFactory.Create(builder => { builder.AddSerilog(serilogLogger, true); });
        _logger = _loggerFactory.CreateLogger<AppHost>();

        try
        {
            var appSettings = SettingsLoader.LoadOrCreate<AppSettings>("appsettings.json");
            _services = new Dictionary<Type, object>();
            _services.Add(typeof(AppSettings), appSettings);
            _services.Add(typeof(AppState), new AppState());
            _services.Add(typeof(AppDbContext), new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseSqlite("Data Source=mec.db").Options));
            _services.Add(typeof(SimulatorProvider), new SimulatorProvider(GetLogger<SimulatorProvider>(), GetService<AppSettings>(), GetService<AppState>()));
            _services.Add(typeof(SorterService), new SorterService(GetLogger<SorterService>(), GetService<AppDbContext>()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AppHost failed to initialized");
            System.Windows.Application.Current.Shutdown();
        }
    }

    public void Dispose()
    {
        _loggerFactory?.Dispose();

        foreach (var obj in _services.Values)
            if (obj is IDisposable disposable)
                disposable.Dispose();
    }

    public void Startup()
    {
        _logger.LogInformation("App Started");
        foreach (var service in _services.Values)
            if (service is IService _service)
                _service.Start();
    }

    public void Shutdown()
    {
        _logger.LogInformation("App Shutdown");
        foreach (var service in _services.Values)
            if (service is IService _service)
                _service.StopAsync();
        Dispose();
    }

    public T GetService<T>()
    {
        foreach (var obj in _services.Values)
            if (obj is T castedObject)
                return castedObject;

        // so technically this should never occur or we got problems. And I dont want that MSFT null warning everywhere.
        throw new InvalidOperationException($"Service of type {typeof(T)} not found.");
    }

    public ILogger<T> GetLogger<T>()
    {
        return _loggerFactory.CreateLogger<T>();
    }
}