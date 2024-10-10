using Spectaitor.Services.Interfacess;
using System.Diagnostics;


namespace Spectaitor.Services.Class
{
    public class ProcessSpectaitorService : IProcessSpectaitorService
    {
        private bool _isRunning;
        private CancellationTokenSource _cancellationTokenSource;

        public bool IsRunning => _isRunning;

        public async Task StartProcesssHandler()
        {
            if (_isRunning) return;

            _isRunning = true;
            _cancellationTokenSource = new CancellationTokenSource();

            await Task.Run(() => MonitorProcesses(_cancellationTokenSource.Token));
        }

       

        public async Task StopProcesssHandler()
        {
            if (!_isRunning) return;

            _isRunning = false;
            _cancellationTokenSource.Cancel();

            await Task.CompletedTask;
        }

     

        private void MonitorProcesses(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var processes = Process.GetProcesses(); 

                foreach (var process in processes)
                {
                    if (_isRunning && ShouldTerminateProcess(process))
                    {
                        try
                        {
                            process.Kill(); 
                        }
                        catch
                        {

                        }
                    }
                }

                Thread.Sleep(2000); 
            }
        }

        
        private bool ShouldTerminateProcess(Process process)
        {

            string[] criticalProcesses = new string[]
                {
        "System",
        "system",
        "svchost",
        "explorer",
        "lsass",
        "winlogon",
        "csrss",
        "smss",
        "taskhost",
        "services",
        "dwm",
        "audiodg",
        "conhost",
        "spoolsv",
        "SearchIndexer",
        "wininit"
                };

            foreach (string criticalProcess in criticalProcesses)
            {
                if (process.ProcessName.Equals(criticalProcess, StringComparison.OrdinalIgnoreCase))
                {
                    return false; 
                }
            }
            return true;
        }
        
    }
}
