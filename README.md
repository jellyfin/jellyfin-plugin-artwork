<h1 align="center">Jellyfin Artwork Plugin</h1>
<h3 align="center">Part of the <a href="https://jellyfin.org">Jellyfin Project</a></h3>

<p align="center">
This plugin is built with .NET to download artwork.
</p>

## Build Process

1. Clone or download this repository

2. Ensure you have .NET SDK setup and installed

3. Build plugin with following command.

```sh
dotnet publish --configuration Release --output bin
```
4. Place the resulting file in the `plugins` folder under the program data directory or inside the portable install directory
