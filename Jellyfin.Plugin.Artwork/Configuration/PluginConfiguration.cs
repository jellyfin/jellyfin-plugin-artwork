using System;
using Jellyfin.Plugin.Artwork.Models;
using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.Artwork.Configuration
{
    /// <summary>
    /// Artwork plugin configuration.
    /// </summary>
    public class PluginConfiguration : BasePluginConfiguration
    {
        /// <summary>
        /// Gets or sets the list of artwork repos.
        /// </summary>
        public ArtworkRepo[] ArtworkRepos { get; set; } = Array.Empty<ArtworkRepo>();
    }
}