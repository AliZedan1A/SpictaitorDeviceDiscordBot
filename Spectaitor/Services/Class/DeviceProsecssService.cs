using Spectaitor.Models;
using Spectaitor.Services.Interfacess;
using System.Diagnostics;

namespace Spectaitor.Services.Class
{
    public class DeviceProcessService : IDeviceProcessService
    {
        public Process GetProcessById(int processId)
        {
            try
            {
                return Process.GetProcessById(processId);
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"No process with ID {processId} was found.");
                return null;
            }
        }

        public Process GetProcessByName(string processname)
        {
            var processes = Process.GetProcessesByName(processname);
            return processes.FirstOrDefault();
        }

        public List<Process> GetProcesses()
        {
            return Process.GetProcesses().ToList();
        }

        public ReturnModel<bool> KillProcess(string processname)
        {
            try
            {
                Console.WriteLine(processname);
                string content = "***Processes Killed\n";
                var process = Process.GetProcessesByName(processname);
                if (process != null)
                {
                    foreach(var item in process)
                    {
                        content += item + " \n"; 
                        item.Kill();

                    }
                    return new() { IsSucceeded=true,Comment = content };
                }
                Console.WriteLine($"Process {processname} not found.");
                return new() { IsSucceeded = false, Comment = "Process not found" };
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                return new() { IsSucceeded = false, Comment = $"Error killing process {processname}: {ex.Message}" };
            }
        }

        public bool KillProcess(int processId)
        {
            try
            {
                var process = GetProcessById(processId);
                if (process != null)
                {
                    process.Kill();
                    return true;
                }
                Console.WriteLine($"Process with ID {processId} not found.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error killing process with ID {processId}: {ex.Message}");
                return false;
            }
        }

        public bool OpenProcess(string processname)
        {
            try
            {
                Process.Start(processname);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening process {processname}: {ex.Message}");
                return false;
            }
        }

        public bool StartReadProcess()
        {
            throw new NotImplementedException();
        }

    }
}
