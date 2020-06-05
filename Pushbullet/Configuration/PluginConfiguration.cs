using System;
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
        /// Pushbullet API url.
        /// </summary>
        public const string Url = "https://api.pushbullet.com/v2/pushes";
        private readonly PushbulletOptions[] _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginConfiguration"/> class.
        /// </summary>
        public PluginConfiguration()
        {
            _options = Array.Empty<PushbulletOptions>();
        }

        /// <summary>
        /// Get configured options.
        /// </summary>
        /// <returns><see cref="IEnumerable{PushbulletOptions}"/>.</returns>
        public IEnumerable<PushbulletOptions> GetOptions()
            => _options;
    }
}