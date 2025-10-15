using MEC.Core;
using MEC.Settings;
using Microsoft.Extensions.Logging;

namespace MEC.Provider;

public class SimulatorProvider : IProvider, IService
{
    private readonly ILogger<SimulatorProvider> _logger;
    private readonly ProviderSettings _providerSettings;
    private readonly AppState _appState;

    private CancellationTokenSource? _cts;

    private bool _connected = false;
    private bool _heartbeat;
    private string _lastPseudo = "";

    private DateTime _nextHeatBeat = DateTime.MinValue;
    private DateTime _nextClearedPseudo = DateTime.MinValue;
    private DateTime _nextSimulatedPseudo = DateTime.MinValue;

    public SimulatorProvider(ILogger<SimulatorProvider> logger, AppSettings appSettings, AppState appState)
    {
        _logger = logger;
        _providerSettings = appSettings.ProviderSettings;
        _appState = appState;
    }

    /// <inheritdoc/>
    public async Task EnableStationAsync()
    {
        try
        {
            await SimulatedWriteAsync(_providerSettings.EnabledTagName, true, _cts.Token);
            _appState.Enabled = true;
            _nextSimulatedPseudo = DateTime.UtcNow.AddSeconds(3);
        }
        catch (Exception ex)
        {
            throw new ProviderFailedException("Failed to enable station.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task DisableStationAsync()
    {
        try
        {
            await SimulatedWriteAsync(_providerSettings.EnabledTagName, false, _cts.Token);
            _appState.Enabled = false;
        }
        catch (Exception ex)
        {
            throw new ProviderFailedException("Failed to disable station.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task DispatchAsync(string pseudo, string iata, int destination)
    {
        // Note:
        // We require parameters instead of using the app state only so that it
        // is clear these values MUST be set prior to dispatching.

        if (string.IsNullOrWhiteSpace(pseudo))
            throw new ArgumentNullException(nameof(pseudo));
        if (iata is null)
            throw new InvalidOperationException($"{nameof(iata)} cannot be null or empty.");
        if (iata.Length > 10)
            throw new InvalidOperationException($"{nameof(iata)} code cannot exceed 10 characters.");
        if (destination <= 0)
            throw new InvalidOperationException($"{nameof(destination)} must be greater than 1.");

        try
        {
            await SimulatedWriteAsync(_providerSettings.DispatchTagName, true, _cts.Token);

            // just auto simulate a new pseudo in 3 seconds
            _nextClearedPseudo = DateTime.UtcNow.AddMilliseconds(200);
            _nextSimulatedPseudo = DateTime.UtcNow.AddSeconds(3);
        }
        catch (Exception ex)
        {
            throw new ProviderFailedException("Failed to dispatch bag.", ex);
        }
    }

    /// <inheritdoc/>
    public void Start()
    {
        if (_cts != null)
            throw new Exception("Cannot start the simulator if it has not been stopped prior.");

        _cts = new CancellationTokenSource();
        _ = Task.Run(() => Execute(_cts.Token));
    }

    /// <inheritdoc/>
    public async Task StopAsync()
    {
        if (_cts == null)
            return;

        await _cts.CancelAsync();

        _cts.Dispose();
        _cts = null;
    }

    /// <inheritdoc/>
    public async Task Execute(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            if (!_connected)
            {
                await ConnectAsync(ct);
                _nextHeatBeat = DateTime.UtcNow;
                await Task.Delay(100, ct);
                continue;
            }

            if (_nextHeatBeat > DateTime.UtcNow)
                await SendHeartbeatAsync(ct);

            await ReadPseudoAsync(ct);

            await Task.Delay(100, ct);
        }
    }

    /// <summary>
    /// An internal function for connecting to the PLC.
    /// </summary>
    /// <returns></returns>
    private async Task ConnectAsync(CancellationToken ct)
    {
        try
        {
            await Task.Delay(500, ct);
            _connected = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to connect");
        }
    }

    /// <summary>
    /// An internal function for disonnecting from the PLC.
    /// </summary>
    private void Disonnect()
    {
        _connected = false;
    }

    /// <summary>
    /// An internal function for handling heartbeat comms
    /// </summary>
    /// <returns></returns>
    private async Task SendHeartbeatAsync(CancellationToken ct)
    {
        try
        {
            await SimulatedWriteAsync(_providerSettings.HBTagName, !_heartbeat, ct);
            _heartbeat = !_heartbeat;
            _nextHeatBeat = DateTime.UtcNow.AddSeconds(3);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send heartbeat");
            // TODO: inform app about this
        }
    }

    /// <summary>
    /// An internal function for reading the pseudo and processing its value
    /// </summary>
    /// <returns></returns>
    private async Task ReadPseudoAsync(CancellationToken ct)
    {
        try
        {
            await SimulatedReadAsync(_providerSettings.PseudoTagname, ct);
            var value = SimulatedGetValue<string>(_providerSettings.PseudoTagname);
            if (value != _lastPseudo)
            {
                _lastPseudo = value;
                _appState.Pseudo = value;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to read pseudo");
        }
    }

    /// <summary>
    /// A simulation function to mock the PLC Comms & PLC.
    /// </summary>
    /// <param name="tagName"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <remarks>
    /// This layer emulates libplctag, which is the primary interface we are mocking.
    /// For async functions, they rely on exception throwing. So This simulator does too.
    /// </remarks>
    private async Task SimulatedWriteAsync(string tagName, object value, CancellationToken ct)
    {
        await Task.Delay(100, ct);
    }

    /// <summary>
    /// A simulation function to mock the PLC Comms & PLC.
    /// </summary>
    /// <param name="tagName"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <remarks>
    /// This layer emulates libplctag, which is the primary interface we are mocking.
    /// For async functions, they rely on exception throwing. So This simulator does too.
    /// </remarks>
    private async Task SimulatedReadAsync(string tagName, CancellationToken ct)
    {
        await Task.Delay(100, ct);
    }

    /// <summary>
    /// A simulation function to mock the PLC Comms & PLC.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tagName"></param>
    /// <returns></returns>
    /// <remarks>
    /// This layer emulates libplctag, which is the primary interface we are mocking.
    /// For async functions, they rely on exception throwing. So This simulator does too.
    /// </remarks>
    private T SimulatedGetValue<T>(string tagName)
    {
        // An horrible function indeed.

        if (tagName == _providerSettings.PseudoTagname)
        {
            if (_nextClearedPseudo != DateTime.MinValue && DateTime.UtcNow > _nextClearedPseudo)
            {
                _nextClearedPseudo = DateTime.MinValue;
                return (T)(object)"";
            }

            if (_nextSimulatedPseudo != DateTime.MinValue && DateTime.UtcNow > _nextSimulatedPseudo)
            {
                var rnd = new Random();
                var number = rnd.Next(100000, 1000000);
                _nextSimulatedPseudo = DateTime.MinValue;
                return (T)(object)number.ToString();
            }

            return (T)(object)_lastPseudo;
        }

        if (tagName == _providerSettings.CanDispatchTagName)
            return (T)(object)string.IsNullOrEmpty("");

        return default;
    }
}