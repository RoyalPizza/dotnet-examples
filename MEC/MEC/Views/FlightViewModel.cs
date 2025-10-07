using MEC.Core;
using MEC.Db;
using MEC.Sorters;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace MEC.Views;

public class FlightViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public ObservableCollection<Flight> Flights { get; init; }
    public ICommand CancelPressedCommand { get; }
    public ICommand ConfirmPressedCommand { get; }
    public Window? Window { get; set; }

    private Flight _selectedFlight;
    public Flight SelectedFlight
    {
        get => _selectedFlight;
        set
        {
            _selectedFlight = value;
            OnPropertyChanged(nameof(SelectedFlight));
        }
    }

    private readonly ILogger<TroubleViewModel> _logger;
    private readonly SorterService _sorterService;
    private readonly AppState _appState;

    public FlightViewModel()
    {
        _logger = AppHost.shared.GetLogger<TroubleViewModel>();
        _sorterService = AppHost.shared.GetService<SorterService>();
        _appState = AppHost.shared.GetService<AppState>();

        Flights = new ObservableCollection<Flight>();
        CancelPressedCommand = new RelayCommand(CancelPressed);
        ConfirmPressedCommand = new RelayCommand(ConfirmPressed);
    }


    private void CancelPressed()
    {
        if (Window == null)
            return;

        Window.DialogResult = false;
        Window.Close();
    }

    private void ConfirmPressed()
    {
        if (Window == null)
            return;

        _appState.IATA = "";
        _appState.Destination = _sorterService.GetFlightDestination(SelectedFlight);

        Window.DialogResult = true;
        Window.Close();
    }

    public void Loaded()
    {
        Flights.Clear();
        foreach (var flight in _sorterService.GetFlights())
            Flights.Add(flight);

        OnPropertyChanged(nameof(Flights));
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}