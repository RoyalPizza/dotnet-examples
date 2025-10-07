using MEC.Core;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MEC.Views;

public class LogsViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public ObservableCollection<LogItem> Logs { get; init; }
    public ObservableCollection<LogFilename> Filenames { get; init; }

    private LogFilename? _selectedFilename;
    public LogFilename? SelectedFilename
    {
        get => _selectedFilename;
        set
        {
            _selectedFilename = value;
            UpdateLogs();
            OnPropertyChanged(nameof(SelectedFilename));
        }
    }

    private string _fileData;
    public string FileData
    {
        get => _fileData;
        set
        {
            _fileData = value;
            OnPropertyChanged(nameof(FileData));
        }
    }

    private ILogger<LogsViewModel> _logger;
    private LogParser _logParser;

    public LogsViewModel()
    {
        _logger = AppHost.shared.GetLogger<LogsViewModel>();
        Logs = new ObservableCollection<LogItem>();
        Filenames = new ObservableCollection<LogFilename>();
        _logParser = new LogParser(AppHost.shared.GetLogger<LogParser>());

        var filenames = _logParser.GetFilenames();
        foreach (var filename in filenames)
            Filenames.Add(filename);
    }

    public void Loaded()
    {
        SelectedFilename = Filenames.FirstOrDefault();
    }

    private async void UpdateLogs()
    {
        Logs.Clear();
        if (_selectedFilename == null)
            return;

        var logs = await _logParser.GetLogDataAsync(_selectedFilename);
        foreach (var log in logs)
            Logs.Add(log);

        FileData = await _logParser.GetLogTextAsync(_selectedFilename);
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
