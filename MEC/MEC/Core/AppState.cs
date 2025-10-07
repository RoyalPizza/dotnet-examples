using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MEC.Core;

public sealed class AppState : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public ObservableCollection<DispatchHistory> DispatchHistories { get; init; } = new();

    [Obsolete("This property is not implemented yet.")]
    public string _user = "";
    [Obsolete("This property is not implemented yet.")]
    public string User
    {
        get => _user;
        set
        {
            if (_user != value)
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }
    }

    private bool _enabled;
    public bool Enabled
    {
        get => _enabled;
        set
        {
            if (_enabled != value)
            {
                _enabled = value;
                OnPropertyChanged(nameof(Enabled));
            }
        }
    }

    private string _pseudo = "";
    public string Pseudo
    {
        get => _pseudo;
        set
        {
            if (_pseudo != value)
            {
                _pseudo = value;
                _iata = ""; // when pseudo changes, this must change with it
                _destination = 0; // when pseudo changes, this must change with it
                OnPropertyChanged(nameof(Pseudo));
            }
        }
    }

    private string _iata = "";
    public string IATA
    {
        get => _iata;
        set
        {
            if (_iata != value)
            {
                _iata = value;
                OnPropertyChanged(nameof(IATA));
            }
        }
    }

    private int _destination = 0;
    public int Destination
    {
        get => _destination;
        set
        {
            if (_destination != value)
            {
                _destination = value;
                OnPropertyChanged(nameof(Destination));
            }
        }
    }

    [Obsolete("This property is not implemented yet.")]
    private DispatchMode _dispatchMode;
    [Obsolete("This property is not implemented yet.")]
    public DispatchMode DispatchMode
    {
        get => _dispatchMode;
        set
        {
            if (_dispatchMode != value)
            {
                _dispatchMode = value;
                OnPropertyChanged(nameof(DispatchMode));
            }
        }
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}