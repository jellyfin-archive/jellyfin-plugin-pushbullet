using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Data.Entities;
using MediaBrowser.Common.Json;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Notifications;
using Microsoft.Extensions.Logging;
using Pushbullet.Configuration;

namespace Pushbullet
{
    /// <summary>
    /// Notifier service.
    /// </summary>
    public class Notifier : INotificationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<Notifier> _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="Notifier"/> class.
        /// </summary>
        /// <param name="logger">Instance of the <see cref="ILogger{Notifier}"/> interface.</param>
        /// <param name="httpClientFactory">Instance of the <see cref="IHttpClientFactory"/> interface.</param>
        public Notifier(ILogger<Notifier> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _jsonSerializerOptions = JsonDefaults.GetOptions();
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
            if (options == null)
            {
                return;
            }

            var parameters = new Dictionary<string, string>
            {
                { "type", "note" },
                { "title", request.Name },
                { "body", request.Description }
            };

            if (!string.IsNullOrEmpty(options.Channel))
            {
                parameters.Add("channel_tag", options.Channel);
            }

            _logger.LogDebug(
                "Pushbullet to Token : {0} - {1} - {2}",
                options.Token,
                options.DeviceId,
                request.Description);

            var httpClient = _httpClientFactory.CreateClient(NamedClient.Default);
            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, PluginConfiguration.Url);
            requestMessage.Content = new StringContent(
                JsonSerializer.Serialize(parameters, _jsonSerializerOptions),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);
            requestMessage.Headers.TryAddWithoutValidation("Access-Token", options.Token);
            using var responseMessage = await httpClient.SendAsync(requestMessage).ConfigureAwait(false);
        }

        private static PushbulletOptions? GetOptions(User user)
        {
            return Plugin.Instance?.Configuration.GetOptions()
                .FirstOrDefault(i => Guid.Parse(i.UserId) == user.Id);
        }

        private static bool IsValid(PushbulletOptions options)
        {
            return !string.IsNullOrEmpty(options.Token);
        }
    }
}