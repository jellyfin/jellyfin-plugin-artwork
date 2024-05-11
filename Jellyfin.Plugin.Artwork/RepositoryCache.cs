using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Jellyfin.Plugin.Artwork.Models;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities.Audio;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.Artwork
{
    /// <summary>
    /// The repository cache.
    /// </summary>
    public class RepositoryCache : IRepositoryCache
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<RepositoryCache> _logger;
        private readonly TimeSpan _cacheExpire = TimeSpan.FromMinutes(5);

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryCache"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Instance of the <see cref="IHttpClientFactory"/> interface.</param>
        /// <param name="memoryCache">Instance of the <see cref="IMemoryCache"/> interface.</param>
        /// <param name="logger">Instance of the <see cref="ILogger{RepositoryCache}"/> interface.</param>
        public RepositoryCache(
            IHttpClientFactory httpClientFactory,
            IMemoryCache memoryCache,
            ILogger<RepositoryCache> logger)
        {
            _httpClientFactory = httpClientFactory;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<RemoteImageInfo>> GetImageInfos(string imageTypeKey, Type itemType, IHasProviderIds providerIds)
        {
            var artworkRepo = ArtworkPlugin.Instance!.Configuration.ArtworkRepos;
            var remoteImageInfos = new List<RemoteImageInfo>();

            foreach (var repo in artworkRepo)
            {
                var fullUrl = repo.Url.TrimEnd('/') + $"/{imageTypeKey}.json";
                var artworkDtos = await GetFromRepo(fullUrl).ConfigureAwait(false);
                var artworkDto = GetMatch(itemType, providerIds, artworkDtos);
                AddImageInfos(
                    repo,
                    imageTypeKey,
                    ref remoteImageInfos,
                    artworkDto);
            }

            return remoteImageInfos;
        }

        private static ArtworkDto? GetMatch(Type itemType, IHasProviderIds providerIds, IReadOnlyList<ArtworkDto> artworkDtos)
        {
            foreach (var artworkDto in artworkDtos)
            {
                if (artworkDto.Providers == null)
                {
                    // No providers, skip.
                    // TODO match on name
                    continue;
                }

                if (providerIds.TryGetProviderId("AniList", out var providerId)
                    && string.Equals(providerId, artworkDto.Providers.Anilist, StringComparison.OrdinalIgnoreCase))
                {
                    return artworkDto;
                }

                if (providerIds.TryGetProviderId(MetadataProvider.Imdb, out providerId)
                    && string.Equals(providerId, artworkDto.Providers.Imdb, StringComparison.OrdinalIgnoreCase))
                {
                    return artworkDto;
                }

                if (providerIds.TryGetProviderId(MetadataProvider.Tmdb, out providerId)
                    && string.Equals(providerId, artworkDto.Providers.Tmdb, StringComparison.OrdinalIgnoreCase))
                {
                    return artworkDto;
                }

                if (providerIds.TryGetProviderId(MetadataProvider.Tvdb, out providerId)
                    && string.Equals(providerId, artworkDto.Providers.Tvdb, StringComparison.OrdinalIgnoreCase))
                {
                    return artworkDto;
                }

                if ((itemType == typeof(Audio) || itemType == typeof(MusicAlbum))
                    && providerIds.TryGetProviderId(MetadataProvider.MusicBrainzReleaseGroup, out providerId)
                    && string.Equals(providerId, artworkDto.Providers.Musicbrainz, StringComparison.OrdinalIgnoreCase))
                {
                    return artworkDto;
                }

                if (itemType == typeof(Audio)
                    && providerIds.TryGetProviderId(MetadataProvider.MusicBrainzAlbumArtist, out providerId)
                    && string.Equals(providerId, artworkDto.Providers.Musicbrainz, StringComparison.OrdinalIgnoreCase))
                {
                    return artworkDto;
                }

                if ((itemType == typeof(MusicAlbum) || itemType == typeof(Audio))
                    && providerIds.TryGetProviderId(MetadataProvider.MusicBrainzAlbum, out providerId)
                    && string.Equals(providerId, artworkDto.Providers.Musicbrainz, StringComparison.OrdinalIgnoreCase))
                {
                    return artworkDto;
                }

                if (itemType == typeof(MusicArtist)
                    && providerIds.TryGetProviderId(MetadataProvider.MusicBrainzArtist, out providerId)
                    && string.Equals(providerId, artworkDto.Providers.Musicbrainz, StringComparison.OrdinalIgnoreCase))
                {
                    return artworkDto;
                }

                if (itemType == typeof(Audio)
                    && providerIds.TryGetProviderId(MetadataProvider.MusicBrainzTrack, out providerId)
                    && string.Equals(providerId, artworkDto.Providers.Musicbrainz, StringComparison.OrdinalIgnoreCase))
                {
                    return artworkDto;
                }
            }

            return null;
        }

        private static void AddImageInfos(
            ArtworkRepo repo,
            string itemKey,
            ref List<RemoteImageInfo> imageInfos,
            ArtworkDto? artworkDto)
        {
            if (artworkDto?.ArtworkImages == null)
            {
                // Repo or images not found.
                return;
            }

            /*
             * 0: machine name
             * 1: image type
             * 2: image extension
             */
            var imageUrlTemplate = repo.Url.TrimEnd('/') + $"/{itemKey}/{{0}}/{{1}}.{{2}}";

            foreach (var image in artworkDto.ArtworkImages.Backdrop)
            {
                imageInfos.Add(new RemoteImageInfo
                {
                    Type = ImageType.Backdrop,
                    Url = string.Format(CultureInfo.InvariantCulture, imageUrlTemplate, artworkDto.MachineName, "backdrop", image)
                });
            }

            foreach (var image in artworkDto.ArtworkImages.Primary)
            {
                imageInfos.Add(new RemoteImageInfo
                {
                    Type = ImageType.Primary,
                    Url = string.Format(CultureInfo.InvariantCulture, imageUrlTemplate, artworkDto.MachineName, "primary", image)
                });
            }

            foreach (var image in artworkDto.ArtworkImages.Thumb)
            {
                imageInfos.Add(new RemoteImageInfo
                {
                    Type = ImageType.Thumb,
                    Url = string.Format(CultureInfo.InvariantCulture, imageUrlTemplate, artworkDto.MachineName, "thumb", image)
                });
            }

            foreach (var image in artworkDto.ArtworkImages.Logo)
            {
                imageInfos.Add(new RemoteImageInfo
                {
                    Type = ImageType.Logo,
                    Url = string.Format(CultureInfo.InvariantCulture, imageUrlTemplate, artworkDto.MachineName, "logo", image)
                });
            }
        }

        private async Task<IReadOnlyList<ArtworkDto>> GetFromRepo(string repositoryUrl)
        {
            if (_memoryCache.TryGetValue(repositoryUrl, out IReadOnlyList<ArtworkDto>? cachedArtwork)
                && cachedArtwork is not null)
            {
                return cachedArtwork;
            }

            try
            {
                var artworkDto = await _httpClientFactory
                    .CreateClient(NamedClient.Default)
                    .GetFromJsonAsync<IReadOnlyList<ArtworkDto>>(repositoryUrl)
                    .ConfigureAwait(false);
                if (artworkDto != null)
                {
                    _memoryCache.Set(repositoryUrl, artworkDto, _cacheExpire);
                    return artworkDto;
                }
            }
            catch (HttpRequestException e)
            {
                _logger.LogWarning(e, "Error downloading repo");
            }
            catch (JsonException e)
            {
                _logger.LogWarning(e, "Error deserializing repo response");
            }

            return Array.Empty<ArtworkDto>();
        }
    }
}
