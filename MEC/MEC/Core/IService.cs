namespace MEC.Core;

internal interface IService
{
    void Start();
    Task StopAsync();
}