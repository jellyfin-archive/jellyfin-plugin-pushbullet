using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using MediaBrowser.Common.Net;
using MediaBrowser.Model.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pushbullet.Configuration;

namespace Pushbullet.Api
{
    /// <summary>
    /// Pushbullet notifications controller.
    /// </summary>
    [ApiController]
    [Route("Notification/Pushbullet")]
    [Produces(MediaTypeNames.Application.Json)]
    public class PushbulletNotificationsController : ControllerBase
    {
        private readonly IHttpClient _httpClient;
        private readonly IJsonSerializer _jsonSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="PushbulletNotificationsController"/> class.
        /// </summary>
        /// <param name="jsonSerializer">Instance of the <see cref="IJsonSerializer"/> interface.</param>
        /// <param name="httpClient">Instance of the <see cref="IHttpClient"/> interface.</param>
        public PushbulletNotificationsController(IJsonSerializer jsonSerializer, IHttpClient httpClient)
        {
            _jsonSerializer = jsonSerializer;
            _httpClient = httpClient;
        }

        /// <summary>
        /// Send Pushbullet test notification.
        /// </summary>
        /// <param name="userId">The user id of the Jellyfin user.</param>
        /// <response code="204">Test notification successfully sent.</response>
        /// <returns>A <see cref="NoContentResult"/> indicating success.</returns>
        [HttpPost("Test/{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> PostAsync([FromRoute] string userId)
        {
            var options = Plugin.Instance!.Configuration.GetOptions()
                .FirstOrDefault(i => string.Equals(i.UserId, userId, StringComparison.OrdinalIgnoreCase));

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

            return NoContent();
        }
    }
}