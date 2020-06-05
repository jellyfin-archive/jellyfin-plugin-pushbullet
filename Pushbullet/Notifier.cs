using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Notifications;
using MediaBrowser.Model.Serialization;
using Microsoft.Extensions.Logging;
using Pushbullet.Configuration;

namespace Pushbullet
{
    /// <summary>
    /// Notifier service.
    /// </summary>
    public class Notifier : INotificationService
    {
        private readonly IHttpClient _httpClient;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly ILogger<Notifier> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Notifier"/> class.
        /// </summary>
        /// <param name="logger">Instance of the <see cref="ILogger{Notifier}"/> interface.</param>
        /// <param name="httpClient">Instance of the <see cref="IHttpClient"/> interface.</param>
        /// <param name="jsonSerializer">Instance of the <see cref="IJsonSerializer"/> interface.</param>
        public Notifier(ILogger<Notifier> logger, IHttpClient httpClient, IJsonSerializer jsonSerializer)
        {
            _logger = logger;
            _httpClient = httpClient;
            _jsonSerializer = jsonSerializer;
        }

        /// <summary>
        /// Gets plugin name.
        /// </summary>
        public string Name => Plugin.Instance!.Name;

        /// <inheritdoc />
        public bool IsEnabledForUser(User user)
        {
            var options = GetOptions(user);

            return options != null && IsValid(options) && options.Enabled;
        }

        /// <inheritdoc />
        public async Task SendNotification(UserNotification request, CancellationToken cancellationToken)
        {
            var options = GetOptions(request.User);

            var parameters = new Dictionary<string, string>
            {
                { "channel_tag", options.Channel! },
                { "type", "note" },
                { "title", request.Name },
                { "body", request.Description }
            };

            _logger.LogDebug(
                "Pushbullet to Token : {0} - {1} - {2}",
                options.Token,
                options.DeviceId,
                request.Description);

            var requestOptions = new HttpRequestOptions
            {
                Url = PluginConfiguration.Url,
                RequestContent = _jsonSerializer.SerializeToString(parameters),
                RequestContentType = "application/json",
                LogErrorResponseBody = true,
                RequestHeaders = { ["Access-Token"] = options.Token }
            };

            await _httpClient.Post(requestOptions).ConfigureAwait(false);
        }

        private static PushbulletOptions GetOptions(BaseItem user)
        {
            return Plugin.Instance!.Configuration.GetOptions()
                .FirstOrDefault(i =>
                    string.Equals(i.UserId, user.Id.ToString("N", CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase));
        }

        private static bool IsValid(PushbulletOptions options)
        {
            return !string.IsNullOrEmpty(options.Token);
        }
    }
}