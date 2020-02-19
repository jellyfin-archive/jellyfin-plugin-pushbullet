using System.Collections.Generic;
using System.Text;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Notifications;
using Microsoft.Extensions.Logging;
using MediaBrowser.Model.Serialization;
using Jellyfin.Plugin.Pushbullet.Configuration;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Jellyfin.Plugin.Pushbullet
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

        private PushbulletOptions GetOptions(User user)
        {
            return Plugin.Instance.Configuration.Options
                .FirstOrDefault(i => string.Equals(i.MediaBrowserUserId, user.Id.ToString("N"), StringComparison.OrdinalIgnoreCase));
        }

        public string Name
        {
            get { return Plugin.Instance.Name; }
        }

        public async Task SendNotification(UserNotification request, CancellationToken cancellationToken)
        {
            var options = GetOptions(request.User);

            var parameters = new Dictionary<string, string>
                {
                   // {"device_iden", options.DeviceId},
                    {"channel_tag", options.Channel},
                    {"type", "note"},
                    {"title", request.Name},
                    {"body", request.Description}
                };

            _logger.LogDebug("Pushbullet to Token : {0} - {1} - {2}", options.Token, options.DeviceId, request.Description);
            
            string authInfo = options.Token;
            authInfo = Convert.ToBase64String(Encoding.UTF8.GetBytes(authInfo));

            var requestOptions = new HttpRequestOptions
                {
                    Url = "https://api.pushbullet.com/v2/pushes",
                    RequestContent = _jsonSerializer.SerializeToString(parameters),
                    BufferContent = false,
                    RequestContentType = "application/json",
                    LogErrorResponseBody = true,
                    DecompressionMethod = CompressionMethod.None,
                    EnableKeepAlive = false
                };
            requestOptions.RequestHeaders["Authorization"] = "Basic " + authInfo;
            await _httpClient.Post(requestOptions).ConfigureAwait(false);
        }

        private bool IsValid(PushbulletOptions options)
        {
            return !string.IsNullOrEmpty(options.Token);
        }
    }
}
