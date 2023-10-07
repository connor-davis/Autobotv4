using System;
using System.IO;
using System.Threading.Tasks;
using Octokit;

namespace Autobot.Utils;

public static class AutoUpdater
{
    public static async Task<UpdaterStatus> CheckForUpdates()
    {
        Console.WriteLine("Creating github client.");
        var gitHubClient = new GitHubClient(new ProductHeaderValue("connor-davis"));
        Console.WriteLine("Fetching releases.");
        var releases = await gitHubClient.Repository.Release.GetAll("connor-davis", "Autobotv4");
        Console.WriteLine($"Fetched {releases.Count} releases.");
        
        var latestGithubVersion = new Version(releases[0].TagName.Replace("v", ""));
        Console.WriteLine($"Latest Github Version: {latestGithubVersion}");
        var latestLocalVersion = new Version("1.0.0");
        Console.WriteLine($"Latest Local Version: {latestLocalVersion}");
        
        var versionComparison = latestLocalVersion.CompareTo(latestGithubVersion);
        Console.WriteLine($"Version Comparison Result: {versionComparison}");
        Console.WriteLine(versionComparison < 0 ? UpdaterStatus.OutDated : UpdaterStatus.UpToDate);

        return versionComparison < 0 ? UpdaterStatus.OutDated : UpdaterStatus.UpToDate;
    }

    public static async Task DownloadAndInstallUpdate()
    {
        Console.WriteLine("Creating github client.");
        var gitHubClient = new GitHubClient(new ProductHeaderValue("connor-davis"));
        Console.WriteLine("Fetching releases.");
        var releases = await gitHubClient.Repository.Release.GetAll("connor-davis", "Autobotv4");
        Console.WriteLine($"Fetched {releases.Count} releases.");

        var latestGithubVersion = new Version(releases[0].TagName.Replace("v", ""));

        using var client = new System.Net.Http.HttpClient();
        
        var contents = client.GetByteArrayAsync($"https://github.com/connor-davis/Autobotv4/releases/download/v{latestGithubVersion}/Autobot.dll").Result;

        if (!Directory.Exists("Update")) Directory.CreateDirectory("Update");
        
        await File.WriteAllBytesAsync("Update/Autobot.dll", contents);
    }
}