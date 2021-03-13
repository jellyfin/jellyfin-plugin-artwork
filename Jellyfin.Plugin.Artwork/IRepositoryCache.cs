using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;

namespace Jellyfin.Plugin.Artwork
{
    /// <summary>
    /// The repository cache interface.
    /// </summary>
    public interface IRepositoryCache
    {
        /// <summary>
        /// Get the remote image info for item.
        /// </summary>
        /// <param name="imageTypeKey">The item repo key.</param>
        /// <param name="itemType">The item type.</param>
        /// <param name="providerIds">The provider ids.</param>
        /// <returns>The list of remote image info.</returns>
        Task<IEnumerable<RemoteImageInfo>> GetImageInfos(string imageTypeKey, Type itemType, IHasProviderIds providerIds);
    }
}