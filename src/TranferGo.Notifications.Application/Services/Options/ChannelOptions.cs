namespace TranferGo.Notifications.Application.Services.Options;

public class ChannelOptions
{
    public bool Enabled { get; set; }
    
    public List<ProviderOptions> Providers { get; set; } = new();
}