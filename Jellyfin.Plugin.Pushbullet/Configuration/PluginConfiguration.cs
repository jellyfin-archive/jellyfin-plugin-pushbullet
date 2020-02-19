using System;
using System.Collections.Generic;
using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.Pushbullet.Configuration
{
    public class PluginConfiguration : BasePluginConfiguration
    {
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
        public string MediaBrowserUserId { get; set; }
    }

}
