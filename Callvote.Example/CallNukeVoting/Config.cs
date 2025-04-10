using Exiled.API.Interfaces;

namespace CallNukeVoting
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; }
    }
}
