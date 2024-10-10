using System.IO;
using Newtonsoft.Json;
using Spectaitor.Services.Interfacess;

namespace Spectaitor
{
    public class TimeData
    {
        public TimeSpan startTime { get; set; }
        public TimeSpan endTime { get; set; }
    }
    public class StartUpSpectaitor
    {
        private readonly string Path = "Config.json";
        private readonly string json = @"{
            ""startTime"": ""04:00:00"",
            ""endTime"": ""09:00:00""
        }";
        private readonly IProcessSpectaitorService _processSpectaitor;

        public StartUpSpectaitor(IProcessSpectaitorService processSpectaitor)
        {
            _processSpectaitor = processSpectaitor;
        Start:
            if(!File.Exists(Path))
            {
                File.Create(Path);
                File.WriteAllText(Path, json);
            }
            string jsonContent = File.ReadAllText(Path);
            TimeData timeData;
            
            try
            {
                timeData = JsonConvert.DeserializeObject<TimeData>(jsonContent);
                if(timeData is null)
                {
                    File.WriteAllText(Path, json);
                    goto Start;

                }
            }
            catch
            {
                File.WriteAllText(Path, json);
                goto Start;
            }

            while (true)
            {
                Thread.Sleep(1000);
                TimeSpan currentTime = DateTime.UtcNow.TimeOfDay;
                if (currentTime >= timeData.startTime && currentTime <= timeData.endTime)
                {
                    _processSpectaitor.StartProcesssHandler();
                }
                else
                {
                    _processSpectaitor.StopProcesssHandler();
                }
                Console.WriteLine(_processSpectaitor.IsRunning);
            }

        }

    }
}
