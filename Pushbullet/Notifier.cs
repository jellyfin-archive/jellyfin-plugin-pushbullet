using System.Collections.Generic;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Notifications;
using Microsoft.Extensions.Logging;
using MediaBrowser.Model.Serialization;
using Pushbullet.Configuration;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pushbullet
{
    public class Notifier : INotificationService
    {
        private readonly ILogger _logger;
        private readonly IHttpClient _httpClient;
        private readonly IJsonSerializer _jsonSerializer;

        public Notifier(ILogger logger, IHttpClient httpClient, IJsonSerializer jsonSerializer)
        {
               _logger = logger;
               _httpClient = httpClient;
               _jsonSerializer = jsonSerializer;
        }

        public bool IsEnabledForUser(User user)
        {
            var options = GetOptions(user);

            return options != null && IsValid(options) && options.Enabled;
        }

        private static PushbulletOptions GetOptions(BaseItem user)
        {
            return Plugin.Instance.Configuration.Options
                .FirstOrDefault(i => string.Equals(i.UserId, user.Id.ToString("N"), StringComparison.OrdinalIgnoreCase));
        }

        public string Name => Plugin.Instance.Name;

        public async Task SendNotification(UserNotification request, CancellationToken cancellationToken)
        {
            var options = GetOptions(request.User);

            var parameters = new Dictionary<string, string>
            {
                {"channel_tag", options.Channel},
                {"type", "note"},
                {"title", request.Name},
                {"body", request.Description}
            };

            _logger.LogDebug("Pushbullet to Token : {0} - {1} - {2}", options.Token, options.DeviceId, request.Description);
            
            var requestOptions = new HttpRequestOptions
            {
                Url = PluginConfiguration.Url,
                RequestContent = _jsonSerializer.SerializeToString(parameters),
                RequestContentType = "application/json",
                LogErrorResponseBody = true,
                RequestHeaders = {["Access-Token"] = options.Token}
            };
            
            await _httpClient.Post(requestOptions).ConfigureAwait(false);
        }

        private static bool IsValid(PushbulletOptions options)
        {
            return !string.IsNullOrEmpty(options.Token);
        }
    }
}
