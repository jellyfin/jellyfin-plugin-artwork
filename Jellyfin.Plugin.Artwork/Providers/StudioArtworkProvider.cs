using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;

namespace Jellyfin.Plugin.Artwork.Providers
{
    /// <summary>
    /// Studio artwork provider.
    /// </summary>
    public class StudioArtworkProvider : IRemoteImageProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRepositoryCache _repositoryCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudioArtworkProvider"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Instance of the <see cref="IHttpClientFactory"/> interface.</param>
        /// <param name="repositoryCache">Instance of the <see cref="IRepositoryCache"/> interface.</param>
        public StudioArtworkProvider(
            IHttpClientFactory httpClientFactory,
            IRepositoryCache repositoryCache)
        {
            _httpClientFactory = httpClientFactory;
            _repositoryCache = repositoryCache;
        }

        /// <inheritdoc />
        public string Name
            => "Studio Artwork Provider";

        /// <inheritdoc />
        public bool Supports(BaseItem item)
            => item is Studio;

        /// <inheritdoc />
        public IEnumerable<ImageType> GetSupportedImages(BaseItem item)
            => ArtworkProviderHelper.GetSupportedImages();

        /// <inheritdoc />
        public Task<IEnumerable<RemoteImageInfo>> GetImages(BaseItem item, CancellationToken cancellationToken)
            => _repositoryCache.GetImageInfos(item.GetType(), item);

        /// <inheritdoc />
        public Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken)
            => _httpClientFactory
                .CreateClient(NamedClient.Default)
                .GetAsync(url, cancellationToken);
    }
}