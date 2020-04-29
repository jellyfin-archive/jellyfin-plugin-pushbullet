using MediaBrowser.Model.Plugins;

namespace Pushbullet.Configuration
{
    public class PluginConfiguration : BasePluginConfiguration
    {
        public const string Url = "https://api.pushbullet.com/v2/pushes";
        
        public PushbulletOptions[] Options { get; set; }

        public PluginConfiguration()
        {
            Options = new PushbulletOptions[] { };
        }
    }

    public class PushbulletOptions
    {
        public bool Enabled { get; set; }
        public string Token { get; set; }
        public string DeviceId { get; set; }
        public string Channel { get; set; }
        public string UserId { get; set; }
    }

}
