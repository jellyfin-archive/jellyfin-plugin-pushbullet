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

        /// <summary>
        /// Gets or sets the configured options.
        /// </summary>
        /// <returns><see cref="IEnumerable{PushbulletOptions}"/>.</returns>
        public PushbulletOptions[] Options { get; set; } = Array.Empty<PushbulletOptions>();
    }
}