using MEC.Core;
using MEC.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MEC.Sorters;

public class SorterService : IService
{
    public List<Flight> GetFlights() => _flights;
    public List<Carrier> GetCarriers() => _carriers.Values.ToList();
    public List<Pier> GetPiers() => _piers.Values.ToList();

    private List<Flight> _flights { get; }
    private Dictionary<string, Carrier> _carriers { get; init; }
    private Dictionary<int, Pier> _piers { get; init; }

    private readonly object _carriersLock = new();
    private readonly object _flightsLock = new();
    private readonly object _piersLock = new();

    private readonly AppDbContext _context;
    private readonly CancellationTokenSource _cts;
    private readonly ILogger<SorterService> _logger;
    private readonly TimeSpan _refreshDelay = TimeSpan.FromMinutes(15);

    public SorterService(ILogger<SorterService> logger, AppDbContext context)
    {
        _logger = logger;
        _cts = new CancellationTokenSource();

        _flights = new List<Flight>();
        _carriers = new Dictionary<string, Carrier>();
        _piers = new Dictionary<int, Pier>();
        _context = context;
    }

    public void Start()
    {
        _logger.LogInformation("Sorter started");
        _ = ExecuteAsync(_cts.Token);
    }

    public Task StopAsync()
    {
        _logger.LogInformation("Sorter stopped");
        _cts.Cancel();
        return Task.CompletedTask;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await _context.Database.EnsureCreatedAsync();

        while (!cancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation("Syncing Sorter");

            await LoadFlights();
            await LoadCarriers();
            await LoadPiers();

            await Task.Delay(_refreshDelay, _cts.Token);
        }
    }

    private async Task LoadFlights()
    {
        try
        {
            var a = await _context.Flights.ToListAsync();
        }
        catch (Exception ex)
        {

        }

        var flights = await _context.Flights.ToListAsync();
        lock (_flightsLock)
        {
            _flights.Clear();
            _flights.AddRange(flights);
        }

        await Task.Delay(300);
    }

    private async Task LoadCarriers()
    {
        var carriers = await _context.Carriers.ToListAsync();
        lock (_carriersLock)
        {
            _carriers.Clear();
            foreach (var carrier in carriers)
                try
                {
                    _carriers[carrier.Code] = carrier;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to add carrier");
                }
        }

        await Task.Delay(300);
    }

    private async Task LoadPiers()
    {
        var piers = await _context.Piers.ToListAsync();
        lock (_piersLock)
        {
            _piers.Clear();
            foreach (var pier in piers)
                try
                {
                    _piers[pier.Code] = pier;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to add pier");
                }
        }

        await Task.Delay(300);
    }

    public bool IsValidIATA(string iata)
    {
        if (iata.Length != 10)
            return false;

        // we do not validate IATA exist in our DB. The sorter simply checks
        // the rules on dispatch to use the IATA, carrier, etc...

        return true;
    }

    public int GetSortDestination(string iata)
    {
        // TODO: Implementing this requires a flight schedule.
        return 1;
    }

    public int GetFlightDestination(Flight flight)
    {
        // TODO: Implementing this requires a flight schedule.
        return 1;
    }

    public int GetCarrierDestination(Carrier carrier)
    {
        // TODO: Implementing this requires a flight schedule.
        return 1;
    }

    public int GetPierDestination(Pier pier)
    {
        // TODO: Implementing this requires a flight schedule.
        return pier.Code;
    }

    public int GetTroubleDestination()
    {
        // TODO: Implementing this requires a flight schedule.
        return 1;
    }

    public string GetDestinationName(int destinationCode)
    {
        _piers.TryGetValue(destinationCode, out var pier);
        return pier?.Name ?? "";
    }
}