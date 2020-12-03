using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MediaBrowser.Common.Json;
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
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="PushbulletNotificationsController"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Instance of the <see cref="IHttpClientFactory"/> interface.</param>
        public PushbulletNotificationsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _jsonSerializerOptions = JsonDefaults.GetOptions();
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
            var options = Plugin.Instance?.Configuration.Options
                .FirstOrDefault(i => string.Equals(i.UserId, userId, StringComparison.OrdinalIgnoreCase));
            if (options == null)
            {
                return BadRequest("Options are null");
            }

            var parameters = new Dictionary<string, string>
            {
                { "type", "note" },
                { "title", "Test Notification" },
                { "body", "This is a test notification from Jellyfin" }
            };

            var httpClient = _httpClientFactory.CreateClient(NamedClient.Default);
            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, PluginConfiguration.Url);
            requestMessage.Content = new StringContent(
                JsonSerializer.Serialize(parameters, _jsonSerializerOptions),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);
            requestMessage.Headers.TryAddWithoutValidation("Access-Token", options.Token);
            using var responseMessage = await httpClient.SendAsync(requestMessage).ConfigureAwait(false);
            return NoContent();
        }
    }
}