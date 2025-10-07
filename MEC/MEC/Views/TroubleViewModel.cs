using MEC.Core;
using MEC.Sorters;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace MEC.Views;

public class TroubleViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public ICommand CancelPressedCommand { get; }
    public ICommand ConfirmPressedCommand { get; }
    public Window? Window { get; set; }

    private string _description = "Click 'Confirm' if this bag cannot be sorted because it is missing a tag and/or its destination cannot be determined. To dispatch using another method, click 'Cancel'.";
    public string Description
    {
        get => _description;
        set
        {
            if (_description != value)
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
    }

    private readonly ILogger<TroubleViewModel> _logger;
    private readonly SorterService _sorterService;
    private readonly AppState _appState;

    public TroubleViewModel()
    {
        _logger = AppHost.shared.GetLogger<TroubleViewModel>();
        _sorterService = AppHost.shared.GetService<SorterService>();
        _appState = AppHost.shared.GetService<AppState>();

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
        _appState.Destination = _sorterService.GetTroubleDestination();

        Window.DialogResult = true;
        Window.Close();
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}