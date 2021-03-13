using System;
using System.Diagnostics.CodeAnalysis;
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
        [SuppressMessage(category: "Performance", checkId: "CA1819", Target = "ArtworkRepos", Justification = "Xml Serializer doesn't support IReadOnlyList")]
        public ArtworkRepo[] ArtworkRepos { get; set; } = Array.Empty<ArtworkRepo>();
    }
}