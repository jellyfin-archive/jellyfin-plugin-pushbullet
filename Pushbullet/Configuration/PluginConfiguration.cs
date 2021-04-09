using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        [SuppressMessage("Microsoft.Maintainability", "CA1819: Properties should not return arrays", Justification = "XML serializer support")]
        public PushbulletOptions[] Options { get; set; } = Array.Empty<PushbulletOptions>();
    }
}
