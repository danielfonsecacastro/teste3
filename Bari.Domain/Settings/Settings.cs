using Bari.Domain.Interfaces;

namespace Bari.Domain.Settings
{
    public class Settings : ISettings
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Queue { get; set; }
        public int Interval { get; set; }
        public string ServiceName { get; set; }
    }
}
