using System;
using System.Collections.Generic;
using Jellyfin.Plugin.Artwork.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.Artwork
{
    /// <summary>
    /// Plugin entrypoint.
    /// </summary>
    public class ArtworkPlugin : BasePlugin<PluginConfiguration>, IHasWebPages
    {
        private readonly Guid _id = new ("7871D3B1-F1B9-4318-9C27-F35998FFBBCC");

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtworkPlugin"/> class.
        /// </summary>
        /// <param name="applicationPaths">Instance of the <see cref="IApplicationPaths"/> interface.</param>
        /// <param name="xmlSerializer">Instance of the <see cref="IXmlSerializer"/> interface.</param>
        public ArtworkPlugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
            : base(applicationPaths, xmlSerializer)
        {
            Instance = this;
        }

        /// <summary>
        /// Gets current plugin instance.
        /// </summary>
        public static ArtworkPlugin? Instance { get; private set; }

        /// <inheritdoc />
        public override Guid Id => _id;

        /// <inheritdoc />
        public override string Name => "Artwork";

        /// <inheritdoc />
        public override string Description => "Get artwork from repo";

        /// <inheritdoc />
        public IEnumerable<PluginPageInfo> GetPages()
        {
            var prefix = GetType().Namespace;
            yield return new PluginPageInfo
            {
                Name = Name,
                EmbeddedResourcePath = prefix + ".Configuration.Web.config.html",
            };

            yield return new PluginPageInfo
            {
                Name = $"{Name}.js",
                EmbeddedResourcePath = prefix + ".Configuration.Web.config.js"
            };
        }
    }
}