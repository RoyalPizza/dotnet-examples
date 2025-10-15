using MEC.Core;
using MEC.Db;
using MEC.Sorters;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace MEC.Views;

public class PierViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public ObservableCollection<Pier> Piers { get; init; }
    public ICommand CancelPressedCommand { get; }
    public ICommand ConfirmPressedCommand { get; }
    public Window? Window { get; set; }

    private Pier _selectedPier;
    public Pier SelectedPier
    {
        get => _selectedPier;
        set
        {
            _selectedPier = value;
            OnPropertyChanged(nameof(SelectedPier));
        }
    }

    private readonly ILogger<TroubleViewModel> _logger;
    private readonly SorterService _sorterService;
    private readonly AppState _appState;

    public PierViewModel()
    {
        _logger = AppHost.shared.GetLogger<TroubleViewModel>();
        _sorterService = AppHost.shared.GetService<SorterService>();
        _appState = AppHost.shared.GetService<AppState>();

        Piers = new ObservableCollection<Pier>();
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
        _appState.Destination = _sorterService.GetPierDestination(SelectedPier);

        Window.DialogResult = true;
        Window.Close();
    }

    public void Loaded()
    {
        Piers.Clear();
        foreach (var pier in _sorterService.GetPiers())
            Piers.Add(pier);
        OnPropertyChanged(nameof(Piers));

    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}