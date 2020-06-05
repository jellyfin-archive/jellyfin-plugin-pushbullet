using MediaBrowser.Model.Services;

namespace Pushbullet.Api
{
    /// <summary>
    /// Test notification request.
    /// </summary>
    [Route("/Notification/Pushbullet/Test/{UserId}", "POST", Summary = "Tests Pushbullet")]
    public class TestNotification : IReturnVoid
    {
        /// <summary>
        /// Gets or sets user Id to test.
        /// </summary>
        [ApiMember(
            Name = "UserId",
            Description = "User Id",
            IsRequired = true,
            DataType = "string",
            ParameterType = "path",
            Verb = "GET")]
        public string? UserId { get; set; }
    }
}