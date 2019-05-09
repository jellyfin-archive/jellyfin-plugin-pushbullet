using System;
using System.Collections.Generic;
using MediaBrowser.Model.Plugins;

namespace Pushbullet.Configuration
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
        public Boolean Enabled { get; set; }
        public String Token { get; set; }
        public String DeviceId { get; set; }
        public string MediaBrowserUserId { get; set; }
    }

}
