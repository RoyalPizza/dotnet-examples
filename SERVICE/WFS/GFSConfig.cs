using System.Text.Json.Serialization;

namespace WFS;

// TODO: This class is very brute force. Works for my purpose, but is a bad look.

/// <summary>
/// A class representing the config from 
/// https://open-meteo.com/en/docs/gfs-api
/// </summary>
[Obsolete]
internal class GFSConfig
{
    /// <summary>
    /// Geographical WGS84 coordinates of the location. 
    /// Multiple coordinates can be comma separated. 
    /// E.g. &latitude=52.52,48.85&longitude=13.41,2.35. 
    /// To return data for multiple locations the JSON output changes 
    /// to a list of structures. /// CSV and XLSX formats add a column location_id.
    /// </summary>
    [JsonPropertyName("latitude")]
    public float Lattitude { get; set; }

    /// <summary>
    /// Geographical WGS84 coordinates of the location. 
    /// Multiple coordinates can be comma separated. 
    /// E.g. &latitude=52.52,48.85&longitude=13.41,2.35. 
    /// To return data for multiple locations the JSON output changes 
    /// to a list of structures. /// CSV and XLSX formats add a column location_id.
    /// </summary>
    [JsonPropertyName("longitude")]
    public float Longitude { get; set; }

    /// <summary>
    /// The elevation used for statistical downscaling. 
    /// Per default, a 90 meter digital elevation model is used. 
    /// You can manually set the elevation to correctly match mountain peaks. 
    /// If &elevation=nan is specified, downscaling will be disabled and the API uses 
    /// the average grid-cell height.
    /// </summary>
    [JsonPropertyName("elevation")]
    public float? Elevation { get; set; }

    /// <summary>
    /// A list of weather variables which should be returned. Values can be comma separated, 
    /// or multiple &hourly= parameter in the URL can be used.
    /// </summary>
    [JsonPropertyName("hourly")]
    public string?[]? Hourly { get; set; }

    /// <summary>
    /// A list of daily weather variable aggregations which should be returned. 
    /// Values can be comma separated, or multiple &daily= parameter in the URL can be used. 
    /// If daily weather variables are specified, parameter timezone is required.
    /// </summary>
    [JsonPropertyName("daily")]
    public string?[]? Daily { get; set; }

    /// <summary>
    /// A list of weather variables to get current conditions.
    /// </summary>
    [JsonPropertyName("current")]
    public string?[]? Current { get; set; }

    /// <summary>
    /// If fahrenheit is set, all temperature values are converted to Fahrenheit.
    /// </summary>
    [JsonPropertyName("temperature_unit")]
    public string? TemperatureUnit { get; set; }

    /// <summary>
    /// Other wind speed speed units: ms, mph and kn
    /// </summary>
    [JsonPropertyName("wind_speed_unit")]
    public string? WindSPeedUnit { get; set; }

    /// <summary>
    /// Other precipitation amount units: inch
    /// </summary>
    [JsonPropertyName("precipitation_unit")]
    public string? PercipitationUnit { get; set; }

    /// <summary>
    /// If format unixtime is selected, all time values are returned in UNIX epoch 
    /// time in seconds. Please note that all timestamp are in GMT+0! For daily values 
    /// with unix timestamps, please apply utc_offset_seconds again to get the correct date.
    /// </summary>
    [JsonPropertyName("timeformat")]
    public string? TimeFormat { get; set; }

    /// <summary>
    /// If timezone is set, all timestamps are returned as local-time and data is 
    /// returned starting at 00:00 local-time. Any time zone name from the time 
    /// zone database is supported. If auto is set as a time zone, the coordinates 
    /// will be automatically resolved to the local time zone. For multiple 
    /// coordinates, a comma separated list of timezones can be specified.
    /// </summary>
    [JsonPropertyName("timezone")]
    public string? TimeZone { get; set; }

    /// <summary>
    /// If past_days is set, past weather data can be returned.
    /// </summary>
    [JsonPropertyName("past_days")]
    public int? PastDays { get; set; }

    /// <summary>
    /// Per default, only 7 days are returned. Up to 16 days of forecast are possible.
    /// </summary>
    /// <remarks>
    /// 0 - 16 only
    /// </remarks>
    [JsonPropertyName("forecast_days")]
    public int? ForcastDays { get; set; }

    /// <summary>
    /// Similar to forecast_days, the number of timesteps of hourly and 15-minutely 
    /// data can controlled. Instead of using the current day as a reference, the current 
    /// hour or the current 15-minute time-step is used.
    /// </summary>
    /// <remarks>
    /// > 0 only
    /// </remarks>
    [JsonPropertyName("forecast_hours")]
    public int? ForcastHours { get; set; }

    /// <summary>
    /// Similar to forecast_days, the number of timesteps of hourly and 15-minutely 
    /// data can controlled. Instead of using the current day as a reference, the current 
    /// hour or the current 15-minute time-step is used.
    /// </summary>
    /// <remarks>
    /// > 0 only
    /// </remarks>
    [JsonPropertyName("forecast_minutely_15")]
    public int? ForcastMinutely15 { get; set; }

    /// <summary>
    /// Similar to forecast_days, the number of timesteps of hourly and 15-minutely 
    /// data can controlled. Instead of using the current day as a reference, the current 
    /// hour or the current 15-minute time-step is used.
    /// </summary>
    /// <remarks>
    /// > 0 only
    /// </remarks>
    [JsonPropertyName("past_hours")]
    public int? PastHours { get; set; }

    /// <summary>
    /// Similar to forecast_days, the number of timesteps of hourly and 15-minutely 
    /// data can controlled. Instead of using the current day as a reference, the current 
    /// hour or the current 15-minute time-step is used.
    /// </summary>
    /// <remarks>
    /// > 0 only
    /// </remarks>
    [JsonPropertyName("past_minutely_15")]
    public int? PastMinutely15 { get; set; }

    /// <summary>
    /// The time interval to get weather data. A day must be specified as an 
    /// ISO8601 date (e.g. 2022-06-30).
    /// </summary>
    /// <remarks>
    /// (yyyy-mm-dd)
    /// </remarks>
    [JsonPropertyName("start_date")]
    public string? StartDate { get; set; }

    /// <summary>
    /// The time interval to get weather data. A day must be specified as an 
    /// ISO8601 date (e.g. 2022-06-30).
    /// </summary>
    /// <remarks>
    /// (yyyy-mm-dd)
    /// </remarks>
    [JsonPropertyName("end_date")]
    public string? EndDate { get; set; }

    /// <summary>
    /// The time interval to get weather data for hourly or 15 minutely data. 
    /// Time must be specified as an ISO8601 date (e.g. 2022-06-30T12:00).
    /// </summary>
    /// <remarks>
    /// (yyyy-mm-ddThh:mm)
    /// </remarks>
    [JsonPropertyName("start_hour")]
    public string? StartHour { get; set; }

    /// <summary>
    /// The time interval to get weather data for hourly or 15 minutely data. 
    /// Time must be specified as an ISO8601 date (e.g. 2022-06-30T12:00).
    /// </summary>
    /// <remarks>
    /// (yyyy-mm-ddThh:mm)
    /// </remarks>
    [JsonPropertyName("end_hour")]
    public string? EndHour { get; set; }

    /// <summary>
    /// The time interval to get weather data for hourly or 15 minutely data. 
    /// Time must be specified as an ISO8601 date (e.g. 2022-06-30T12:00).
    /// </summary>
    /// <remarks>
    /// (yyyy-mm-ddThh:mm)
    /// </remarks>
    [JsonPropertyName("start_minutely_15")]
    public string? StartMinutely15 { get; set; }

    /// <summary>
    /// The time interval to get weather data for hourly or 15 minutely data. 
    /// Time must be specified as an ISO8601 date (e.g. 2022-06-30T12:00).
    /// </summary>
    /// <remarks>
    /// (yyyy-mm-ddThh:mm)
    /// </remarks>
    [JsonPropertyName("end_minutely_15")]
    public string? EndMinutely15 { get; set; }

    /// <summary>
    /// Set a preference how grid-cells are selected. The default land 
    /// finds a suitable grid-cell on land with similar elevation to the 
    /// requested coordinates using a 90-meter digital elevation model. 
    /// sea prefers grid-cells on sea. nearest selects the nearest possible grid-cell.
    /// </summary>
    [JsonPropertyName("cell_selection")]
    public string? CellSelection { get; set; }

    /// <summary>
    /// Only required to commercial use to access reserved API resources for customers. 
    /// The server URL requires the prefix customer-. See pricing for more information.
    /// </summary>
    [JsonPropertyName("apikey")]
    public string? ApiKey { get; set; }
}
