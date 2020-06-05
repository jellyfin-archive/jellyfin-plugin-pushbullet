using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaBrowser.Common.Net;
using MediaBrowser.Model.Serialization;
using MediaBrowser.Model.Services;
using Pushbullet.Configuration;

namespace Pushbullet.Api
{
    /// <summary>
    /// API endpoints.
    /// </summary>
    public class ServerApiEntryPoints : IService
    {
        private readonly IHttpClient _httpClient;
        private readonly IJsonSerializer _jsonSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerApiEntryPoints"/> class.
        /// </summary>
        /// <param name="jsonSerializer">Instance of the <see cref="IJsonSerializer"/> interface.</param>
        /// <param name="httpClient">Instance of the <see cref="IHttpClient"/> interface.</param>
        public ServerApiEntryPoints(IJsonSerializer jsonSerializer, IHttpClient httpClient)
        {
            _jsonSerializer = jsonSerializer;
            _httpClient = httpClient;
        }

        private static PushbulletOptions GetOptions(string userId)
        {
            return Plugin.Instance!.Configuration.GetOptions()
                .FirstOrDefault(i => string.Equals(i.UserId, userId, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Send test notification.
        /// </summary>
        /// <param name="request">Request to send.</param>
        public void Post(TestNotification request)
        {
            PostAsync(request)
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Send test notification.
        /// </summary>
        /// <param name="request">Request to send.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public async Task PostAsync(TestNotification request)
        {
            var options = GetOptions(request.UserId!);

            var parameters = new Dictionary<string, string>
            {
                { "type", "note" },
                { "title", "Test Notification" },
                { "body", "This is a test notification from Jellyfin" }
            };

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
    }
}