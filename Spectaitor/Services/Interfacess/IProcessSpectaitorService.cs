

namespace Spectaitor.Services.Interfacess
{
    public interface IProcessSpectaitorService
    {
        Task StartProcesssHandler();
        Task StopProcesssHandler();
        bool IsRunning { get; }
    }
}
