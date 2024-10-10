using Discord;
using Discord.Commands;
using Discord.Interactions;
using Spectaitor.Services.Interfacess;


namespace Spectaitor.Modules
{
    public class CommandModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IDeviceMangerService _service;
        private readonly IDeviceProcessService _deviceProcess;
        private readonly IProcessSpectaitorService _spectaitorService;

        public CommandModule(IDeviceMangerService service,IDeviceProcessService deviceProcess, IProcessSpectaitorService spectaitorService)
        {
            _service = service;
            _deviceProcess = deviceProcess;
            _spectaitorService = spectaitorService;
        }

        [SlashCommand("startspectaitor", "Start Spectaitor ")]
        [Discord.Commands.RequireOwner]
        public async Task StartSpectaitor()
        {
            await _spectaitorService.StartProcesssHandler();
        }
        [Discord.Commands.RequireOwner]
        [SlashCommand("stopspectaitor", "Stop Spectaitor.")]
        public async Task StopSpectaitor()
        {
            await _spectaitorService.StopProcesssHandler();
        }
        [Discord.Commands.RequireOwner]
        [SlashCommand("screenshot", "Takes a screenshot and sends it in the channel.")]
        public async Task TakeScreenshot()
        {
            var result = _service.TakeScreenShot(Context.Guild.GetTextChannel(Context.Channel.Id));
            if (result.IsSucceeded)
            {
                await Context.Channel.SendFileAsync(result.Value, "Here is the screenshot.");
                await RespondAsync("Uploading Image...");
               
            }
            else
            {
                await RespondAsync($"Error: {result.Comment}");
            }
        }

        [Discord.Commands.RequireOwner]
        [SlashCommand("turnoff", "Turns off the PC.")]
        public async Task TurnOffPc()
        {
            _service.TurnOffPc();
            await RespondAsync("PC is turning off...");
        }

        [Discord.Commands.RequireOwner]
        [SlashCommand("restart", "Restarts the PC.")]
        public async Task RestartPc()
        {
            _service.RestartPc();
            await RespondAsync("PC is restarting...");
        }

        [Discord.Commands.RequireOwner]
        [SlashCommand("removefile", "Removes a file at the specified path.")]
        public async Task RemoveFile(string path)
        {
            bool success = _service.RemoveFile(path);
            if (success)
            {
                await RespondAsync($"File at {path} has been removed.");
            }
            else
            {
                await RespondAsync($"Error: Unable to remove the file at {path}.");
            }
        }

        [Discord.Commands.RequireOwner]
        [SlashCommand("killprocess", "Kills a process by its name.")]
        public async Task KillProcess(string processName)
        {
            var success = _deviceProcess.KillProcess(processName);
            if (success.IsSucceeded)
            {
                await RespondAsync($"{success.Comment}");
            }
            else
            {
                await RespondAsync($"Error: Unable to kill the process {success.Comment}.");
            }
        }
        [Discord.Commands.RequireOwner]
        [SlashCommand("killprocessbyid", "Kills a process by its name.")]
        public async Task KillProcessById(int ProcessId)
        {
            bool success = _deviceProcess.KillProcess(ProcessId);
            if (success)
            {
                await RespondAsync($"Process {ProcessId} has been killed.");
            }
            else
            {
                await RespondAsync($"Error: Unable to kill the process {ProcessId}.");
            }
        }
        [Discord.Commands.RequireOwner]
        [SlashCommand("openprocess", "Opens a process by its name.")]
        public async Task OpenProcess(string processName)
        {
            bool success = _deviceProcess.OpenProcess(processName);
            if (success)
            {
                await RespondAsync($"Process {processName} has been opened.");
            }
            else
            {
                await RespondAsync($"Error: Unable to open the process {processName}.");
            }
        }
        [Discord.Commands.RequireOwner]
        [SlashCommand("getallprocess", "Get Lists Of Process")]
        public async Task GetAllProcess()
        {
            var List = _deviceProcess.GetProcesses();
            if (List is null)
            {
                await RespondAsync($"Faild To Get Process.");
                return;
            }
            else
            {
                string title = $"***Success to get {List.Count} Process!***";
                string discription = string.Empty ;
            
                
                foreach (var process in List)
                {
                    discription += $"*Process Id:{process.Id} | Process Name : {process.ProcessName}*\n";
                    if(discription.Count() > 1800 || (process == List.Last()))
                    {
                        EmbedBuilder embed = new EmbedBuilder()
                        {
                            Title = title,
                            Color = Discord.Color.Red,  
                            Description = discription
                        };
                       
                        await Context.Channel.SendMessageAsync(embed:embed.Build());
                        discription = "";
                    }
                   
                }
                await RespondAsync($"end");
            }
        }

        [Discord.Commands.RequireOwner]
        [SlashCommand("uploadfile", "Uploads a file.")]
        public async Task UploadFile(IAttachment attachment, string path)
        {
            var result = _service.UploadFile(attachment,path);
            if (result.IsSucceeded)
            {
                await RespondAsync($"File uploaded successfully: {result.Value}");
            }
            else
            {
                await RespondAsync($"Error: {result.Comment}");
            }
        }
    }
   
}
