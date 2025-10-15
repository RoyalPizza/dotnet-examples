using MEC.Core;
using MEC.Sorters;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Windows.Input;

namespace MEC.Views;

internal sealed class NumberpadViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public ICommand NumberPressedCommand { get; }
    public ICommand DeletePressedCommand { get; }

    private string _display = "";
    public string Display
    {
        get => _display;
        private set
        {
            if (_display != value)
            {
                _display = value;
                CheckEnteredValue();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Display)));
            }
        }
    }

    private bool _editEnabled = false;
    public bool EditEnabled
    {
        get => _editEnabled;
        private set
        {
            if (_editEnabled != value)
            {
                _editEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditEnabled)));
            }
        }
    }

    private readonly ILogger<NumberpadViewModel> _logger;
    private readonly AppState _appState;
    private readonly SorterService _sorter;

    public NumberpadViewModel()
    {
        _logger = AppHost.shared.GetLogger<NumberpadViewModel>();
        _appState = AppHost.shared.GetService<AppState>();
        _sorter = AppHost.shared.GetService<SorterService>();

        _appState.PropertyChanged += AppState_PropertyChanged;

        NumberPressedCommand = new RelayCommand<string>(OnNumberPressed);
        DeletePressedCommand = new RelayCommand(OnDeletePressed);
    }

    public void Unloaded()
    {
        _appState.PropertyChanged -= AppState_PropertyChanged;
    }

    private void OnNumberPressed(string number)
    {
        Display += number;
    }

    private void OnDeletePressed()
    {
        if (Display.Length > 0)
            Display = Display.Substring(0, Display.Length - 1);
    }

    private void AppState_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AppState.Pseudo))
        {
            if (string.IsNullOrEmpty(_appState.Pseudo) || _appState.Pseudo == "0")
            {
                Display = "";
                EditEnabled = false;
            }
            else
            {
                Display = "";
                EditEnabled = true;
            }
        }
    }

    private void CheckEnteredValue()
    {
        // TODO: For v1, this assumes we are only ever typing IATA Consider more support in v2.

        if (Display.Length != 10)
        {
            EditEnabled = true;
            return;
        }

        EditEnabled = false;
        if (!_sorter.IsValidIATA(Display))
            return;

        _appState.DispatchMode = DispatchMode.IATA;
        _appState.IATA = Display;
        _appState.Destination = _sorter.GetSortDestination(_appState.IATA);
    }
}