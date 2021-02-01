using System;

namespace Jellyfin.Plugin.Artwork.Models
{
    /// <summary>
    /// Artwork repo.
    /// </summary>
    public class ArtworkRepo
    {
        /// <summary>
        /// Gets or sets the repo name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the repo url.
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// Gets or sets the item types.
        /// </summary>
        public string[] ItemType { get; set; } = Array.Empty<string>();
    }
}