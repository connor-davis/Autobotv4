using System;
using System.Threading.Tasks;
using Octokit;

namespace Autobot.Utils;

public static class AutoUpdater
{
    public static async Task<UpdaterStatus> CheckForUpdates()
    {
        Console.WriteLine("Creating github client.");
        var gitHubClient = new GitHubClient(new ProductHeaderValue("connor-davis"))
        {
            Credentials = new Credentials("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")
        };
        Console.WriteLine("Fetching releases.");
        var releases = await gitHubClient.Repository.Release.GetAll("connor-davis", "Autobotv4");
        Console.WriteLine($"Fetched {releases.Count} releases.");
        
        var latestGithubVersion = new Version(releases[0].TagName.Replace("v", ""));
        Console.WriteLine($"Latest Github Version: {latestGithubVersion}");
        var latestLocalVersion = new Version(Constants.Version);
        Console.WriteLine($"Latest Local Version: {latestLocalVersion}");
        
        var versionComparison = latestLocalVersion.CompareTo(latestGithubVersion);
        Console.WriteLine($"Version Comparison Result: {versionComparison}");
        Console.WriteLine(versionComparison < 0 ? UpdaterStatus.OutDated : UpdaterStatus.UpToDate);

        return versionComparison < 0 ? UpdaterStatus.OutDated : UpdaterStatus.UpToDate;
    }
}