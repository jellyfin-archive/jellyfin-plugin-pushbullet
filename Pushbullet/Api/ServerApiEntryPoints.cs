using System;
using System.Collections.Generic;
using System.Linq;
using MediaBrowser.Common.Net;
using MediaBrowser.Model.Serialization;
using Microsoft.Extensions.Logging;
using MediaBrowser.Model.Services;
using Pushbullet.Configuration;
using System.Threading.Tasks;

namespace Pushbullet.Api
{
    [Route("/Notification/Pushbullet/Test/{UserId}", "POST", Summary = "Tests Pushbullet")]
    public class TestNotification : IReturnVoid
    {
        [ApiMember(Name = "UserId", Description = "User Id", IsRequired = true, DataType = "string",
            ParameterType = "path", Verb = "GET")]
        public string UserId { get; set; }
    }

    public class ServerApiEndpoints : IService
    {
        private readonly IHttpClient _httpClient;
        private readonly IJsonSerializer _jsonSerializer;

        public ServerApiEndpoints(IJsonSerializer jsonSerializer, IHttpClient httpClient)
        {
            _jsonSerializer = jsonSerializer;
            _httpClient = httpClient;
        }

        private static PushbulletOptions GetOptions(string userId)
        {
            return Plugin.Instance.Configuration.Options
                .FirstOrDefault(i => string.Equals(i.UserId, userId, StringComparison.OrdinalIgnoreCase));
        }

        public void Post(TestNotification request)
        {
            PostAsync(request)
                .GetAwaiter()
                .GetResult();
        }

        public async Task PostAsync(TestNotification request)
        {
            var options = GetOptions(request.UserId);

            var parameters = new Dictionary<string, string>
            {
                {"type", "note"},
                {"title", "Test Notification"},
                {"body", "This is a test notification from Jellyfin"}
            };

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
    }
}
