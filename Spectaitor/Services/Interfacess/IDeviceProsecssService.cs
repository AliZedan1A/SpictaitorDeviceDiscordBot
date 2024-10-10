using Spectaitor.Models;
using System.Diagnostics;

namespace Spectaitor.Services.Interfacess
{
    public interface IDeviceProcessService
    {
        List<Process> GetProcesses();
        Process GetProcessById(int processId);
        Process GetProcessByName(string processname);
        ReturnModel<bool> KillProcess(string processname);
        bool KillProcess(int processId);
        bool OpenProcess(string processname);
        bool StartReadProcess();
    }
}
