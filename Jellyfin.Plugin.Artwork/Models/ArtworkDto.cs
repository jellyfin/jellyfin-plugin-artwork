using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.Artwork.Models
{
    /// <summary>
    /// Artwork container.
    /// </summary>
    public class ArtworkDto
    {
        /// <summary>
        /// Gets or sets the artwork name.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the machine name.
        /// </summary>
        [JsonPropertyName("machine-name")]
        public string MachineName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the artwork.
        /// </summary>
        [JsonPropertyName("artwork")]
        public ArtworkImageDto? ArtworkImages { get; set; }

        /// <summary>
        /// Gets or sets the providers.
        /// </summary>
        [JsonPropertyName("providers")]
        public ArtworkProviderDto? Providers { get; set; }
    }
}