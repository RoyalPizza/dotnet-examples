using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.RegularExpressions;

namespace MEC.Core;

public class LogParser
{
    private readonly ILogger<LogParser> _logger;
    private const string LOG_FOLDER = "logs";

    public LogParser(ILogger<LogParser> logger)
    {
        _logger = logger;
    }

    public List<LogFilename> GetFilenames()
    {
        var filenames = new List<LogFilename>();
        try
        {
            if (!Directory.Exists(LOG_FOLDER))
            {
                _logger.LogWarning("Logs folder does not exist: {Folder}", LOG_FOLDER);
                return filenames;
            }

            foreach (var file in Directory.GetFiles(LOG_FOLDER, "log*.txt"))
            {
                // TODO: simplify this garbage
                var filename = Path.GetFileName(file);
                string dirtyFileName = Path.GetFileNameWithoutExtension(file).Replace("log", "");
                bool filenameParseResult = DateOnly.TryParseExact(dirtyFileName, "yyyyMMdd", out var date);
                if (filenameParseResult)
                    filenames.Add(new LogFilename { Filename = filename, Date = date });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading log filenames");
        }
        return filenames;
    }

    public async Task<List<LogItem>> GetLogDataAsync(LogFilename logFilename)
    {
        var logItems = new List<LogItem>();
        var filePath = Path.Combine(LOG_FOLDER, logFilename.Filename);

        try
        {
            if (!File.Exists(filePath))
            {
                _logger.LogWarning("Log file does not exist: {File}", filePath);
                return logItems;
            }

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(stream))
            {
                var lines = await reader.ReadToEndAsync();
                var regex = new Regex(@"^(.*?)\s\[(\w+)\]\s(.+)$");

                foreach (var line in lines.Split('\n'))
                {
                    var match = regex.Match(line);
                    if (match.Success && DateTime.TryParse(match.Groups[1].Value, out var timestamp))
                    {
                        logItems.Add(new LogItem
                        {
                            Timestamp = timestamp,
                            Level = match.Groups[2].Value,
                            Message = match.Groups[3].Value
                        });
                    }
                    else if (!string.IsNullOrWhiteSpace(line))
                    {
                        _logger.LogWarning("Failed to parse log line: {Line}", line);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading log file: {File}", filePath);
        }
        return logItems;
    }

    public async Task<string> GetLogTextAsync(LogFilename logFilename)
    {
        string lines = "";
        var filePath = Path.Combine(LOG_FOLDER, logFilename.Filename);

        try
        {
            if (!File.Exists(filePath))
            {
                _logger.LogWarning("Log file does not exist: {File}", filePath);
                return "";
            }

            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(stream);
            lines = await reader.ReadToEndAsync();
            //using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            //using (var reader = new StreamReader(stream))
            //{
            //    lines = await reader.ReadToEndAsync();
            //}
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading log file: {File}", filePath);
        }
        return lines;
    }
}
