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
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the artwork.
        /// </summary>
        [JsonPropertyName("artwork")]
        public ArtworkImages? ArtworkImages { get; set; }

        /// <summary>
        /// Gets or sets the providers.
        /// </summary>
        public Providers? Providers { get; set; }
    }
}