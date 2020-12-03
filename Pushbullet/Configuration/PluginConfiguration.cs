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
        /// Get configured options.
        /// </summary>
        /// <returns><see cref="IEnumerable{PushbulletOptions}"/>.</returns>
        public IReadOnlyList<PushbulletOptions> Options { get; set; } = Array.Empty<PushbulletOptions>();
    }
}