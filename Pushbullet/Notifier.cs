using System.Collections.Generic;
using System.Text;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Notifications;
using Microsoft.Extensions.Logging;
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

        public Notifier(ILogger logger, IHttpClient httpClient)
        {
               _logger = logger;
               _httpClient = httpClient;
        }

        public bool IsEnabledForUser(User user)
        {
            var options = GetOptions(user);

            return options != null && IsValid(options) && options.Enabled;
        }

        private PushBulletOptions GetOptions(User user)
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
                    {"type", "note"},
                    {"title", request.Name},
                    {"body", request.Description}
                };

            _logger.LogDebug("PushBullet to Token : {0} - {1} - {2}", options.Token, options.DeviceId, request.Description);
            var _httpRequest = new HttpRequestOptions();
            string authInfo = options.Token;
            authInfo = Convert.ToBase64String(Encoding.UTF8.GetBytes(authInfo));

            _httpRequest.RequestHeaders["Authorization"] = "Basic " + authInfo;

            _httpRequest.Url = "https://api.pushbullet.com/v2/pushes";

            _httpRequest.SetPostData(parameters);

            using (await _httpClient.Post(_httpRequest).ConfigureAwait(false))
            {

            }
        }

        private bool IsValid(PushBulletOptions options)
        {
            return !string.IsNullOrEmpty(options.Token);
        }
    }
}
