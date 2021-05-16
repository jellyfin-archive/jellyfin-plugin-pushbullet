using System;
using System.Collections.Generic;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;
using Pushbullet.Configuration;

namespace Pushbullet
{
    /// <summary>
    /// Plugin with configuration and webpages.
    /// </summary>
    public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages
    {
        private readonly Guid _id = new Guid("de228f12-e43e-4bd9-9fc0-2830819c3b92");

        /// <summary>
        /// Initializes a new instance of the <see cref="Plugin"/> class.
        /// </summary>
        /// <param name="applicationPaths">Instance of the <see cref="IApplicationPaths"/> interface.</param>
        /// <param name="xmlSerializer">Instance of the <see cref="IXmlSerializer"/> interface.</param>
        public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
            : base(applicationPaths, xmlSerializer)
        {
            Instance = this;
        }

        /// <inheritdoc />
        public override string Name => "Pushbullet Notifications";

        /// <inheritdoc />
        public override string Description => "Sends notifications via Pushbullet Service.";

        /// <inheritdoc />
        public override Guid Id => _id;

        /// <summary>
        /// Gets plugin instance.
        /// </summary>
        public static Plugin? Instance { get; private set; }

        /// <inheritdoc />
        public IEnumerable<PluginPageInfo> GetPages()
        {
            return new[]
            {
                new PluginPageInfo
                {
                    Name = "pushbulletnotifications",
                    EmbeddedResourcePath = GetType().Namespace + ".Web.pushbulletnotifications.html",
                },
                new PluginPageInfo
                {
                    Name = "pushbulletnotificationsjs",
                    EmbeddedResourcePath = GetType().Namespace + ".Web.pushbulletnotifications.js"
                }
            };
        }
    }
}
