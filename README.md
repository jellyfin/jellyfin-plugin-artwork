<h1 align="center">Jellyfin Artwork Plugin</h1>
<h3 align="center">Part of the <a href="https://jellyfin.org">Jellyfin Project</a></h3>

<p align="center">
Download studio artwork for Jellyfin from configured artwork repositories.
</p>

## About

The Artwork plugin adds a remote image provider for Jellyfin studio entries.
When Jellyfin looks up artwork for a studio, the plugin checks the configured
artwork repositories and returns matching images.

Supported image types:

- Primary
- Thumb
- Logo
- Backdrop

The plugin is intended to be used with artwork repositories such as the
[Jellyfin Artwork Repository](https://github.com/jellyfin/jellyfin-artwork),
which provides studio images and the metadata needed to match them to Jellyfin
items.

## Configuration

1. Install the plugin.
2. Open the Artwork plugin settings in the Jellyfin dashboard.
3. Add one or more artwork repositories.
4. Save the configuration and refresh studio metadata.

An artwork repository is expected to expose a `studios.json` file and matching
image files under a `studios/` directory. The plugin matches entries by provider
IDs such as IMDb, TMDb, TVDb, AniList, and MusicBrainz where available.

## Build Process

1. Clone or download this repository

2. Ensure you have .NET SDK setup and installed

3. Build plugin with following command.

```sh
dotnet publish --configuration Release --output bin
```

4. Place the resulting file in the `plugins` folder under the program data directory or inside the portable install directory
