using MEC.Core;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace MEC.Views;

public class HistoryViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public ObservableCollection<DispatchHistory> History => _appState.DispatchHistories;

    private readonly AppState _appState;

    public HistoryViewModel()
    {
        _appState = AppHost.shared.GetService<AppState>();
        _appState.DispatchHistories.CollectionChanged += DispatchHistories_CollectionChanged;
        PropertyChanged += (s, e) => { }; // Ensure PropertyChanged is never null
    }

    private void DispatchHistories_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(History));
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}