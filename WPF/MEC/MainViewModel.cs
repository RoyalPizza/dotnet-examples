using MEC.Core;
using MEC.Settings;
using MEC.Views;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace MEC;

internal class MainViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private UIElement? _currentView;
    public UIElement? CurrentView
    {
        get => _currentView;
        set
        {
            if (_currentView != value)
            {
                _currentView = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentView)));
            }
        }
    }


    private string _stationName = "MEC-00";
    public string StationName
    {
        get => _stationName;
        set
        {
            if (_stationName != value)
            {
                _stationName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StationName)));
            }
        }
    }

    private string _statusBarClock = "MM/dd/yyyy HH:mm";
    public string StatusBarClock
    {
        get => _statusBarClock;
        set
        {
            if (_statusBarClock != value)
            {
                _statusBarClock = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StatusBarClock)));
            }
        }
    }

    private readonly AppState _appState;
    private readonly ILogger _logger;
    private readonly DispatcherTimer _clockTimer;

    public MainViewModel()
    {
        _logger = AppHost.shared.GetLogger<MainViewModel>();
        _appState = AppHost.shared.GetService<AppState>();

        _appState.PropertyChanged += AppState_PropertyChanged;

        var settings = AppHost.shared.GetService<AppSettings>();
        _stationName = settings.GeneralSettings.StationName;

        // Initialize clock
        _statusBarClock = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
        _clockTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(5)
        };
        _clockTimer.Tick += (s, e) => StatusBarClock = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
        _clockTimer.Start();
    }

    private void AppState_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AppState.Enabled))
            CurrentView = _appState.Enabled ? new EnabledView() : new DisabledView();
    }

    public void Loaded()
    {
        _appState.Enabled = false;
        CurrentView = new DisabledView();
    }

    public void Unloaded()
    {
        _appState.PropertyChanged -= AppState_PropertyChanged;
    }
}