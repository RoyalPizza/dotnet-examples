namespace MEC.Settings;

public sealed class ProviderSettings : ISetting
{
    /// <summary>
    ///     A friendly name
    /// </summary>
    public string Name { get; init; } = "Default Provider";

    // TODO: This varies greatly based on comms protocol and library. This is not a good solution.
    /// <summary>
    ///     The route to use for the plc communication. ex. 192.168.0.1,1,0
    /// </summary>
    public string Route { get; init; } = "0.0.0.0,1,0";

    /// <summary>
    ///     The rate at which the app whill poll the PLC in milliseconds
    /// </summary>
    public uint PollRate { get; init; } = 500;

    /// <summary>
    ///     The rate at which the app will toggle its hearbeat in milliseconds
    /// </summary>
    public uint HBRate { get; init; } = 3000;

    /// <summary>
    ///     Tag name for the Heartbeat tag
    /// </summary>
    public string HBTagName { get; init; } = "MEC.HB";

    /// <summary>
    ///     Tag name for the Enabled tag
    /// </summary>
    public string EnabledTagName { get; init; } = "MEC.Enabled";

    /// <summary>
    ///     Tag name for the Pseudo tag
    /// </summary>
    public string PseudoTagname { get; init; } = "MEC.Pseudo";

    /// <summary>
    ///     Tag name for the can dispatch tag
    /// </summary>
    public string CanDispatchTagName { get; init; } = "MEC.CanDispatch";

    /// <summary>
    ///     Tag name for the IATA tag
    /// </summary>
    public string IATATagName { get; init; } = "MEC.IATA";

    /// <summary>
    ///     Tag name for the dispatch tag
    /// </summary>
    public string DestinationTagName { get; init; } = "MEC.Destination";

    /// <summary>
    ///     Tag name for the Dispatch tag
    /// </summary>
    public string DispatchTagName { get; init; } = "MEC.Dispatch";

    /// <summary>
    ///     Checks the configuration is by ensuring all tagnames are set and rates are valid.
    /// </summary>
    /// <returns></returns>
    public bool IsValid()
    {
        var stringsValid = !string.IsNullOrEmpty(HBTagName)
                           && !string.IsNullOrEmpty(EnabledTagName)
                           && !string.IsNullOrEmpty(PseudoTagname)
                           && !string.IsNullOrEmpty(CanDispatchTagName)
                           && !string.IsNullOrEmpty(IATATagName)
                           && !string.IsNullOrEmpty(DestinationTagName)
                           && !string.IsNullOrEmpty(DispatchTagName);

        return PollRate > 0 && HBRate > 0 && stringsValid;
    }
}