using MEC.Core;
using MEC.Db;
using MEC.Sorters;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace MEC.Views;

public class CarrierViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public ObservableCollection<Carrier> Carriers { get; init; }
    public ICommand CancelPressedCommand { get; }
    public ICommand ConfirmPressedCommand { get; }
    public Window? Window { get; set; }

    private Carrier _selectedCarrier;
    public Carrier SelectedCarrier
    {
        get => _selectedCarrier;
        set
        {
            _selectedCarrier = value;
            OnPropertyChanged(nameof(SelectedCarrier));
        }
    }

    private readonly ILogger<TroubleViewModel> _logger;
    private readonly SorterService _sorterService;
    private readonly AppState _appState;

    public CarrierViewModel()
    {
        _logger = AppHost.shared.GetLogger<TroubleViewModel>();
        _sorterService = AppHost.shared.GetService<SorterService>();
        _appState = AppHost.shared.GetService<AppState>();

        Carriers = new ObservableCollection<Carrier>();
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
        _appState.Destination = _sorterService.GetCarrierDestination(SelectedCarrier);

        Window.DialogResult = true;
        Window.Close();
    }

    public void Loaded()
    {
        Carriers.Clear();
        foreach (var carrier in _sorterService.GetCarriers())
            Carriers.Add(carrier);
        OnPropertyChanged(nameof(Carriers));

    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}