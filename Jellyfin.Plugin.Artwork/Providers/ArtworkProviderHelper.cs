using System.Collections.Generic;
using MediaBrowser.Model.Entities;

namespace Jellyfin.Plugin.Artwork.Providers
{
    /// <summary>
    /// The artwork provider helper.
    /// </summary>
    public static class ArtworkProviderHelper
    {
        private static readonly IReadOnlyList<ImageType> SupportedImages = new[]
        {
            ImageType.Primary,
            ImageType.Thumb,
            ImageType.Logo,
            ImageType.Backdrop
        };

        /// <summary>
        /// Gets the supported image types.
        /// </summary>
        /// <returns>The supported image types.</returns>
        public static IEnumerable<ImageType> GetSupportedImages => SupportedImages;
    }
}