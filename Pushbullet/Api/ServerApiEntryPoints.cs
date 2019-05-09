using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Net;
using Microsoft.Extensions.Logging;
using MediaBrowser.Model.Services;
using Pushbullet.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace Pushbullet.Api
{
    [Route("/Notification/PushBullet/Test/{UserID}", "POST", Summary = "Tests PushBullet")]
    public class TestNotification : IReturnVoid
    {
        [ApiMember(Name = "UserID", Description = "User Id", IsRequired = true, DataType = "string", ParameterType = "path", Verb = "GET")]
        public string UserID { get; set; }
    }

    class ServerApiEndpoints : IService
    {
        private readonly IHttpClient _httpClient;
        private readonly ILogger _logger;

        public ServerApiEndpoints(Ilogger logger, IHttpClient httpClient)
        {
              _logger = logger.GetLogger(GetType().Name);
              _httpClient = httpClient;
        }
        private PushBulletOptions GetOptions(String userID)
        {
            return Plugin.Instance.Configuration.Options
                .FirstOrDefault(i => string.Equals(i.MediaBrowserUserId, userID, StringComparison.OrdinalIgnoreCase));
        }

        public void Post(TestNotification request)
        {
            var task = PostAsync(request);
            Task.WaitAll(task);
        }

        public async Task PostAsync(TestNotification request)
        {
            var options = GetOptions(request.UserID);

            var parameters = new Dictionary<string, string>
            {
                {"type", "note"},
                {"title", "Test Notification" },
                {"body", "This is a test notification from MediaBrowser"}
            };

            var _httpRequest = new HttpRequestOptions();

            //Create Basic HTTP Auth Header...

            string authInfo = options.Token;
            authInfo = Convert.ToBase64String(Encoding.UTF8.GetBytes(authInfo));

            _httpRequest.RequestHeaders["Authorization"] = "Basic " + authInfo;

            _httpRequest.Url = "https://api.pushbullet.com/v2/pushes";

            _httpRequest.SetPostData(parameters);

            using (await _httpClient.Post(_httpRequest).ConfigureAwait(false))
            {

            }
        }
    }
}
