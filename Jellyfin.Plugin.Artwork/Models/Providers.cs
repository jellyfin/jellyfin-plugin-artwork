namespace Jellyfin.Plugin.Artwork.Models
{
    /// <summary>
    /// The providers.
    /// </summary>
    public class Providers
    {
        /// <summary>
        /// Gets or sets the anilist identifier.
        /// </summary>
        public string? Anilist { get; set; }

        /// <summary>
        /// Gets or sets the imdb identifier.
        /// </summary>
        public string? Imdb { get; set; }

        /// <summary>
        /// Gets or sets the musicbrainz identifier.
        /// </summary>
        public string? Musicbrainz { get; set; }

        /// <summary>
        /// Gets or sets the tmdb identifier.
        /// </summary>
        public string? Tmdb { get; set; }

        /// <summary>
        /// Gets or sets the tvdb identifier.
        /// </summary>
        public string? Tvdb { get; set; }
    }
}