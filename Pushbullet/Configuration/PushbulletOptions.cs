namespace Pushbullet.Configuration
{
    /// <summary>
    /// Pushbullet Options container.
    /// </summary>
    public class PushbulletOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether option is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the channel.
        /// Unused.
        /// </summary>
        public string? Channel { get; set; }

        /// <summary>
        /// Gets or sets the user id for this configuration.
        /// </summary>
        public string UserId { get; set; } = string.Empty;
    }
}
