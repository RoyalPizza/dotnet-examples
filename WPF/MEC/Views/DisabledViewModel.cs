using MEC.Core;
using MEC.Provider;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace MEC.Views;

public class DisabledViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public ICommand EnabledPressedCommand { get; }

    private readonly ILogger<DisabledViewModel> _logger;
    private readonly IProvider _provider;

    public DisabledViewModel()
    {
        _logger = AppHost.shared.GetLogger<DisabledViewModel>();
        _provider = AppHost.shared.GetService<IProvider>();

        EnabledPressedCommand = new AsyncRelayCommand(EnablePressed);
    }

    public async Task EnablePressed()
    {
        try
        {
            await _provider.EnableStationAsync();
        }
        catch (ProviderFailedException ex)
        {
            _ = MessageBox.Show(ex.InnerException?.Message ?? ex.Message, "Enable Station Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}