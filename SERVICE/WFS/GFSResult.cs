using System.Text.Json.Serialization;

namespace WFS;

internal class GFSResult
{
    /// <summary>
    /// WGS84 of the center of the weather grid-cell which was used 
    /// to generate this forecast. This coordinate might be a 
    /// few kilometres away from the requested coordinate.
    /// </summary>
    [JsonPropertyName("latitude")]
    public float Lattitude { get; init; }

    /// <summary>
    /// WGS84 of the center of the weather grid-cell which was used 
    /// to generate this forecast. This coordinate might be a 
    /// few kilometres away from the requested coordinate.
    /// </summary>
    [JsonPropertyName("longitude")]
    public float Longitude { get; init; }

    /// <summary>
    /// The elevation from a 90 meter digital elevation model. 
    /// This effects which grid-cell is selected (see parameter cell_selection). 
    /// Statistical downscaling is used to adapt weather conditions for this elevation. 
    /// This elevation can also be controlled with the query parameter elevation. 
    /// If &elevation=nan is specified, all downscaling is disabled and the averge 
    /// grid-cell elevation is used.
    /// </summary>
    [JsonPropertyName("elevation")]
    public float Elevation { get; init; }

    /// <summary>
    /// Generation time of the weather forecast in milliseconds. 
    /// This is mainly used for performance monitoring and improvements.
    /// </summary>
    [JsonPropertyName("generationtime_ms")]
    public float GenerationTimeMS { get; init; }

    /// <summary>
    /// Applied timezone offset from the &timezone= parameter.
    /// </summary>
    [JsonPropertyName("utc_offset_seconds")]
    public float UtcOffsetSeconds { get; init; }

    /// <summary>
    /// Timezone identifier (e.g. Europe/Berlin) and abbreviation (e.g. CEST)
    /// </summary>
    [JsonPropertyName("timezone")]
    public string Timezone { get; init; } = "";

    /// <summary>
    /// Timezone identifier (e.g. Europe/Berlin) and abbreviation (e.g. CEST)
    /// </summary>
    [JsonPropertyName("timezone_abbreviation")]
    public string TimezoneAbbreviation { get; init; } = "";

    /// <summary>
    /// For each selected weather variable, data will be returned as a floating point array. 
    /// Additionally a time array will be returned with ISO8601 timestamps.
    /// </summary>
    [JsonPropertyName("hourly")]
    public GFSResult_Hourly? Hourly { get; init; }

    /// <summary>
    /// For each selected weather variable, the unit will be listed here.
    /// </summary>
    [JsonPropertyName("hourly_units")]
    public GFSResult_HourlyUnits? HourlyUnits { get; init; }

    // TODO: replace with type
    [JsonPropertyName("daily")]
    public object? Daily { get; init; }

    // TODO: replace with type
    [JsonPropertyName("daily_units")]
    public object? DailyUnits { get; init; }
}

// TODO: complete this class structure
internal class GFSResult_HourlyUnits
{

    [JsonPropertyName("time")]
    public string Time { get; init; } = "";

    [JsonPropertyName("temperature_2m")]
    public string Temperature2M { get; init; } = "";
}

// TODO: complete this class structure
internal class GFSResult_Hourly
{

    [JsonPropertyName("time")]
    public float[] Time { get; init; }

    [JsonPropertyName("temperature_2m")]
    public float[] Temperature2M { get; init; }
}