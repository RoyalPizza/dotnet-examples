using MEC.Core;
using MEC.Provider;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace MEC.Views;

public class EnabledViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public ICommand DisablePressedCommand { get; }
    public ICommand LogsPressedCommand { get; }
    public ICommand FlightPressedCommand { get; }
    public ICommand CarrierPressedCommand { get; }
    public ICommand PierPressedCommand { get; }
    public ICommand TroublePressedCommand { get; }

    private readonly AppState _appState;
    private readonly ILogger<DisabledViewModel> _logger;
    private readonly IProvider _provider;

    public EnabledViewModel()
    {
        _logger = AppHost.shared.GetLogger<DisabledViewModel>();
        _appState = AppHost.shared.GetService<AppState>();
        _provider = AppHost.shared.GetService<IProvider>();

        DisablePressedCommand = new AsyncRelayCommand(DisablePressed);
        LogsPressedCommand = new RelayCommand(LogsPressed);
        FlightPressedCommand = new RelayCommand(FlightPressed);
        CarrierPressedCommand = new RelayCommand(CarrierPressed);
        PierPressedCommand = new RelayCommand(PierPressed);
        TroublePressedCommand = new RelayCommand(TroublePressed);
    }

    private async Task DisablePressed()
    {
        try
        {
            await _provider.DisableStationAsync();
        }
        catch (ProviderFailedException ex)
        {
            _ = MessageBox.Show(ex.InnerException?.Message ?? ex.Message, "Disable Station Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void LogsPressed()
    {
        var view = new LogsView();
        view.ShowDialog();
    }

    private void FlightPressed()
    {
        _appState.DispatchMode = DispatchMode.Flight;
        var view = new FlightView();
        view.ShowDialog();
    }

    private void CarrierPressed()
    {
        _appState.DispatchMode = DispatchMode.Carrier;
        var view = new CarrierView();
        view.ShowDialog();
    }

    private void PierPressed()
    {
        _appState.DispatchMode = DispatchMode.Pier;
        var view = new PierView();
        view.ShowDialog();
    }

    private void TroublePressed()
    {
        var view = new TroubleView();
        view.ShowDialog();
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}