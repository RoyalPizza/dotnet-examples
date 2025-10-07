using MEC.Core;
using MEC.Provider;
using MEC.Sorters;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace MEC.Views;

public class DispatchViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public ICommand DispatchPressedCommand { get; }

    private string _pseudo = "";
    public string Pseudo
    {
        get => _pseudo;
        set
        {
            if (_pseudo != value)
            {
                _pseudo = value;
                OnPropertyChanged(nameof(Pseudo));
            }
        }
    }

    public bool _dispatchEnabled = false;
    public bool DispatchEnabled
    {
        get => _dispatchEnabled;
        set
        {
            if (_dispatchEnabled != value)
            {
                _dispatchEnabled = value;
                OnPropertyChanged(nameof(DispatchEnabled));
            }
        }
    }

    private string _dispatchMessage = "";
    public string DispatchMessage
    {
        get => _dispatchMessage;
        set
        {
            if (_dispatchMessage != value)
            {
                _dispatchMessage = value;
                OnPropertyChanged(nameof(DispatchMessage));
            }
        }
    }

    private readonly AppState _appState;
    private readonly IProvider _provder;
    private readonly SorterService _sorterService;

    public DispatchViewModel()
    {
        _sorterService = AppHost.shared.GetService<SorterService>();
        _provder = AppHost.shared.GetService<IProvider>();
        _appState = AppHost.shared.GetService<AppState>();
        _appState.PropertyChanged += AppState_PropertyChanged;

        DispatchPressedCommand = new AsyncRelayCommand(DispatchPressed);
    }

    public void Unloaded()
    {
        _appState.PropertyChanged -= AppState_PropertyChanged;
    }

    private void AppState_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        Pseudo = _appState.Pseudo;
        DispatchEnabled = _appState.Destination != 0;

        if (string.IsNullOrEmpty(Pseudo) || Pseudo == "0")
        {
            DispatchMessage = "";
        }
        else if (_appState.Destination == 0)
        {
            DispatchMessage = "Bag at station. Please choose a destination.";
        }
        else if (_appState.Destination > 0)
        {
            string destinationName = _sorterService.GetDestinationName(_appState.Destination);
            DispatchMessage = $"Bag ready to dispatch to {destinationName}";
        }
    }

    private async Task DispatchPressed()
    {
        try
        {
            await _provder.DispatchAsync(_appState.Pseudo, _appState.IATA, _appState.Destination);

            string destinationName = _sorterService.GetDestinationName(_appState.Destination);
            var history = new DispatchHistory()
            {
                Pseudo = _appState.Pseudo,
                IATA = _appState.IATA,
                Destination = destinationName,
                DispatchType = _appState.DispatchMode.ToString()
            };
            _appState.DispatchHistories.Insert(0, history);
        }
        catch (ProviderFailedException ex)
        {
            _ = MessageBox.Show(ex.InnerException?.Message ?? ex.Message, "Dispatch Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}