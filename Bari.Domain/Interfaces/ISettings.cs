namespace Bari.Domain.Interfaces
{
    public interface ISettings
    {
        string HostName { get; set; }
        int Port { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        string Queue { get; set; }
        int Interval { get; set; }
        string ServiceName { get; set; }
    }
}
