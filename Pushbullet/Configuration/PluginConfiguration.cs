using System.Collections.Generic;
using MediaBrowser.Model.Plugins;

namespace Pushbullet.Configuration
{
    /// <summary>
    /// The plugin configuration.
    /// </summary>
    public class PluginConfiguration : BasePluginConfiguration
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginConfiguration"/> class.
        /// </summary>
        public PluginConfiguration()
        {
            Options = System.Array.Empty<PushbulletOptions>();
        }

        /// <summary>
        /// Pushbullet API url.
        /// </summary>

        public const string Url = "https://api.pushbullet.com/v2/pushes";

        public PushbulletOptions[] Options { get; set; }

    }

    public class PushbulletOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether option is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the device id.
        /// Unused.
        /// </summary>
        public string? DeviceId { get; set; }

        /// <summary>
        /// Gets or sets the channel.
        /// Unused.
        /// </summary>
        public string? Channel { get; set; }

        /// <summary>
        /// Gets or sets the user id for this configuration.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

    }
}
