namespace Jellyfin.Plugin.Artwork.Models
{
    /// <summary>
    /// The artwork images.
    /// </summary>
    public class ArtworkImages
    {
        /// <summary>
        /// Gets or sets the backdrop image url.
        /// </summary>
        public string? Backdrop { get; set; }

        /// <summary>
        /// Gets or sets the primary image url.
        /// </summary>
        public string? Primary { get; set; }

        /// <summary>
        /// Gets or sets the thumb image url.
        /// </summary>
        public string? Thumb { get; set; }

        /// <summary>
        /// Gets or sets the logo image url.
        /// </summary>
        public string? Logo { get; set; }
    }
}