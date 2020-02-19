<h1 align="center">Jellyfin Pushbullet Plugin</h1>
<h3 align="center">Part of the <a href="https://jellyfin.org/">Jellyfin Project</a></h3>

## About

This plugin can be used for sending notifications to a range of personal devices via <a href="https://www.pushbullet.com/">Pushbullet</a>.

## Build & Installation Process

1. Clone this repository
2. Ensure you have .NET Core SDK set up and installed
3. Build the plugin with following command:

```
dotnet publish --configuration Release --output bin
```

4. Place the resulting `Jellyfin.Plugin.Pushbullet.dll` file in a folder called `plugins/` inside your Jellyfin data directory.

### Screenshot

<img src=screenshot.png>
