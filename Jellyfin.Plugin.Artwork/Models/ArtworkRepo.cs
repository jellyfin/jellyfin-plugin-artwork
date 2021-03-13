using System.ComponentModel.DataAnnotations;

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
        [Required]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the repo url.
        /// </summary>
        [Required]
        public string Url { get; set; } = null!;
    }
}