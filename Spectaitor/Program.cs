using Discord.Interactions;
using Discord.WebSocket;
using Spectaitor.Logger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Discord.Commands;
using Discord;
using Spectaitor.Services.Interfacess;
using Spectaitor.Services.Class;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Spectaitor
{
    

    public class program
    {
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;
        public DiscordSocketClient _client;

        public static Task Main(string[] args) => new program().MainAsync();


        public async Task MainAsync()
        {
            string exePath = "C:\\R\\Spectaitor.exe";

            RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            rk.SetValue("TestSpectaitorApp", exePath);

            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);

            var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .Build();
            using IHost host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>

            services
            .AddSingleton(config)
            .AddSingleton(x => new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.All,
                LogGatewayIntentWarnings = false,
                AlwaysDownloadUsers = true,
                LogLevel = LogSeverity.Debug,
                UseInteractionSnowflakeDate =false
               
            }))
            .AddTransient<ConsoleLogger>()
            .AddSingleton<IDeviceMangerService,DeviceMangerService>()
            .AddSingleton<IDeviceProcessService,DeviceProcessService>()
            .AddSingleton<IProcessSpectaitorService,ProcessSpectaitorService>()
            .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
            .AddSingleton<InteractionHandler>()
            .AddSingleton(x => new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Debug,
                DefaultRunMode = Discord.Commands.RunMode.Async
            }))
            .AddSingleton<PrefixHandler>())
            .Build();

            await RunAsync(host);
        }
        public async Task RunAsync(IHost host)
        {
            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;

            var commands = provider.GetRequiredService<InteractionService>();
            _client = provider.GetRequiredService<DiscordSocketClient>();
            var config = provider.GetRequiredService<IConfigurationRoot>();

            await provider.GetRequiredService<InteractionHandler>().InitializeAsync();

            var prefixCommands = provider.GetRequiredService<PrefixHandler>();
            prefixCommands.AddModule<Spectaitor.Modules.PrefixModule>();
            await prefixCommands.InitializeAsync();


            _client.Log += _ => provider.GetRequiredService<ConsoleLogger>().Log(_);


            commands.Log += _ => provider.GetRequiredService<ConsoleLogger>().Log(_);

            _client.Ready += async () =>
            {

                if (IsDebug())
                    await commands.RegisterCommandsGloballyAsync(false);
                else
                    await commands.RegisterCommandsGloballyAsync(false);
            };


            await _client.LoginAsync(Discord.TokenType.Bot, "token");
            await _client.StartAsync();
            _client.UserJoined += serverjoined;
           
            // StartUpSpectaitor startUpSpectaitor = new StartUpSpectaitor(new ProcessSpectaitorService());
            await Task.Delay(-1);
        }
        public async Task serverjoined(SocketGuildUser user)
        {
            Console.WriteLine(user.Id);
        }
       
      
        static bool IsDebug()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}
