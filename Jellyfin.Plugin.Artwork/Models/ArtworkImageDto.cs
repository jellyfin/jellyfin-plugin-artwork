using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.Artwork.Models
{
    /// <summary>
    /// The artwork image dto.
    /// </summary>
    public class ArtworkImageDto
    {
        /// <summary>
        /// Gets or sets the backdrop image url.
        /// </summary>
        [JsonPropertyName("backdrop")]
        public IReadOnlyList<string> Backdrop { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Gets or sets the primary image url.
        /// </summary>
        [JsonPropertyName("primary")]
        public IReadOnlyList<string> Primary { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Gets or sets the thumb image url.
        /// </summary>
        [JsonPropertyName("thumb")]
        public IReadOnlyList<string> Thumb { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Gets or sets the logo image url.
        /// </summary>
        [JsonPropertyName("logo")]
        public IReadOnlyList<string> Logo { get; set; } = Array.Empty<string>();
    }
}