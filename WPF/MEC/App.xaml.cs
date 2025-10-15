using MEC.Core;
using System.Windows;

namespace MEC;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        AppHost.shared.Startup();

        var mainWindow = new MainView();
        mainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        AppHost.shared.Shutdown();

        base.OnExit(e);
    }
}