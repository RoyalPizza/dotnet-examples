namespace MEC.Provider;

/// <summary>
/// Interface for PLC provider operations.
/// </summary>
public interface IProvider
{
    /// <summary>
    /// Enables the station.
    /// </summary>
    /// <exception cref="ProviderFailedException">Thrown when enabling fails.</exception>
    Task EnableStationAsync();

    /// <summary>
    /// Disables the station.
    /// </summary>
    /// <exception cref="ProviderFailedException">Thrown when disabling fails.</exception>
    Task DisableStationAsync();

    /// <summary>
    /// Dispatches a bag with specified parameters.
    /// </summary>
    /// <exception cref="ProviderFailedException">Thrown when dispatch fails.</exception>
    Task DispatchAsync(string pseudo, string iata, int destination);
}