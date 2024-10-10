using Discord;

namespace Spectaitor.Logger
{
    public interface ILogger
    {
        public Task Log(LogMessage message);
    }
}
