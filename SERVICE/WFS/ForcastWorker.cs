using System.Text.Json;

namespace WFS;

public class ForcastWorker : BackgroundService
{
    private readonly ILogger<ForcastWorker> _logger;
    private readonly HttpClient _httpClient;
    private int _workerDelay = (int)TimeSpan.FromSeconds(30).TotalMilliseconds;

    // ROUTE
    // https://api.open-meteo.com/v1/
    // forecast?
    // latitude=43.47997127581969
    // &longitude=-110.76185560670017
    // &hourly=temperature_2m
    // &models=gfs_seamless
    // &forecast_days=1
    // &timeformat=unixtime
    // &wind_speed_unit=mph
    // &precipitation_unit=inch
    // &temperature_unit=fahrenheit

    // TODO: Dont do this. Just doing it for now to easily move forward.
    private const string BASE_ADDRESS = "https://api.open-meteo.com/v1/";
    private const string API_ROUTE = "forecast?latitude=43.47997127581969&longitude=-110.76185560670017&hourly=temperature_2m&models=gfs_seamless&forecast_days=1&timeformat=unixtime&wind_speed_unit=mph&precipitation_unit=inch&temperature_unit=fahrenheit";

    public ForcastWorker(ILogger<ForcastWorker> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri(BASE_ADDRESS);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("{name} started at {time}", nameof(ForcastWorker), DateTimeOffset.Now);
        _logger.LogInformation("HttpClient BaseAddress: {baseAddress}", _httpClient.BaseAddress?.ToString());
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var response = await _httpClient.GetAsync(API_ROUTE, stoppingToken);
                response.EnsureSuccessStatusCode();
                await using var stream = await response.Content.ReadAsStreamAsync(stoppingToken);
                var result = await JsonSerializer.DeserializeAsync<GFSResult>(stream);
                _logger.LogInformation("{name} fetched api data {time}", nameof(ForcastWorker), DateTimeOffset.Now);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to fetch forecast data");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse JSON response");
            }


            try
            {
                await Task.Delay(_workerDelay, stoppingToken);
            }
            catch (TaskCanceledException ex) { }
        }
        _logger.LogInformation("{name} stopped at {time}", nameof(ForcastWorker), DateTimeOffset.Now);
    }
}
