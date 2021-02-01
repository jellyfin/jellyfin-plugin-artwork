using System;
using System.Collections.Generic;
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
        public async Task<IEnumerable<RemoteImageInfo>> GetImageInfos(Type itemType, IHasProviderIds providerIds)
        {
            var artworkRepo = ArtworkPlugin.Instance!.Configuration.ArtworkRepos;
            var remoteImageInfos = new List<RemoteImageInfo>();

            var itemTypeString = itemType.ToString();
            foreach (var repo in artworkRepo)
            {
                var arrayIndex = Array.FindIndex(repo.ItemType, t => string.Equals(t, itemTypeString, StringComparison.OrdinalIgnoreCase));
                if (arrayIndex == -1)
                {
                    // Repo not configured for item type.
                    continue;
                }

                if (string.IsNullOrEmpty(repo.Url))
                {
                    continue;
                }

                var artworkDtos = await GetFromRepo(repo.Url);
                var artworkDto = GetMatch(itemType, providerIds, artworkDtos);
                if (artworkDto?.ArtworkImages != null)
                {
                    AddImageInfos(ref remoteImageInfos, artworkDto.ArtworkImages);
                }
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

                if (string.Equals(
                    providerIds.ProviderIds["AniList"],
                    artworkDto.Providers.Anilist,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return artworkDto;
                }

                if (string.Equals(
                    providerIds.ProviderIds[MetadataProvider.Imdb.ToString()],
                    artworkDto.Providers.Imdb,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return artworkDto;
                }

                if (string.Equals(
                    providerIds.ProviderIds[MetadataProvider.Tmdb.ToString()],
                    artworkDto.Providers.Tmdb,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return artworkDto;
                }

                if (string.Equals(
                    providerIds.ProviderIds[MetadataProvider.Tvdb.ToString()],
                    artworkDto.Providers.Tvdb,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return artworkDto;
                }

                if ((itemType == typeof(Audio) || itemType == typeof(MusicAlbum))
                    && string.Equals(
                        providerIds.ProviderIds[MetadataProvider.MusicBrainzReleaseGroup.ToString()],
                        artworkDto.Providers.Musicbrainz,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return artworkDto;
                }

                if (itemType == typeof(Audio)
                    && string.Equals(
                        providerIds.ProviderIds[MetadataProvider.MusicBrainzAlbumArtist.ToString()],
                        artworkDto.Providers.Musicbrainz,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return artworkDto;
                }

                if ((itemType == typeof(MusicAlbum) || itemType == typeof(Audio))
                    && string.Equals(
                        providerIds.ProviderIds[MetadataProvider.MusicBrainzAlbum.ToString()],
                        artworkDto.Providers.Musicbrainz,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return artworkDto;
                }

                if (itemType == typeof(MusicArtist)
                    && string.Equals(
                        providerIds.ProviderIds[MetadataProvider.MusicBrainzArtist.ToString()],
                        artworkDto.Providers.Musicbrainz,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return artworkDto;
                }

                if (itemType == typeof(Audio)
                    && string.Equals(
                        providerIds.ProviderIds[MetadataProvider.MusicBrainzTrack.ToString()],
                        artworkDto.Providers.Musicbrainz,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return artworkDto;
                }
            }

            return null;
        }

        private static void AddImageInfos(ref List<RemoteImageInfo> imageInfos, ArtworkImages artworkImages)
        {
            if (!string.IsNullOrEmpty(artworkImages.Backdrop))
            {
                imageInfos.Add(new RemoteImageInfo
                {
                    Type = ImageType.Backdrop,
                    Url = artworkImages.Backdrop
                });
            }

            if (!string.IsNullOrEmpty(artworkImages.Primary))
            {
                imageInfos.Add(new RemoteImageInfo
                {
                    Type = ImageType.Primary,
                    Url = artworkImages.Primary
                });
            }

            if (!string.IsNullOrEmpty(artworkImages.Thumb))
            {
                imageInfos.Add(new RemoteImageInfo
                {
                    Type = ImageType.Thumb,
                    Url = artworkImages.Thumb
                });
            }

            if (!string.IsNullOrEmpty(artworkImages.Logo))
            {
                imageInfos.Add(new RemoteImageInfo
                {
                    Type = ImageType.Logo,
                    Url = artworkImages.Logo
                });
            }
        }

        private async Task<IReadOnlyList<ArtworkDto>> GetFromRepo(string repositoryUrl)
        {
            if (_memoryCache.TryGetValue(repositoryUrl, out IReadOnlyList<ArtworkDto> cachedArtwork))
            {
                return cachedArtwork;
            }

            try
            {
                var artworkDto = await _httpClientFactory
                    .CreateClient(NamedClient.Default)
                    .GetFromJsonAsync<IReadOnlyList<ArtworkDto>>(repositoryUrl);
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