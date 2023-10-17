using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Autobot.Utils;
using Octokit;
using FileMode = System.IO.FileMode;

namespace Autobot;

public partial class UpdateWindow
{
    private static UpdateWindow? _instance;
    private readonly MainWindow _mainWindow = new();
    private static readonly string? DirectoryPath =
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    
    public UpdateWindow()
    {
        InitializeComponent();

        _instance = this;

        new Task(CheckForUpdates).Start();
    }

    private void CloseWindow()
    {
        Dispatcher.Invoke(Close);
    }

    private void OpenMainWindow()
    {
        Dispatcher.Invoke(() =>
        {
            _mainWindow.Show();
        });
    }

    private async void CheckForUpdates()
    {
        var updateStatus = await AutoUpdater.CheckForUpdates();
        
        if (updateStatus == UpdaterStatus.OutDated)
        {
            Console.WriteLine("Update found. Downloading the update.");
            
            new Task(DownloadUpdater).Start();
        }
        else
        {
            Console.WriteLine("No update found.");

            _instance?.OpenMainWindow();
            _instance?.CloseWindow();
        }
    }
    
    private async void DownloadUpdater()
    {
        if (File.Exists($"{DirectoryPath!}\\AutobotUpdater.exe")) File.Delete($"{DirectoryPath!}\\AutobotUpdater.exe");
        
        var gitHubClient = new GitHubClient(new ProductHeaderValue("connor-davis"));
        var releases = await gitHubClient.Repository.Release.GetAll("connor-davis", "AutobotUpdater");
        var latestGithubVersion = new Version(releases[0].TagName.Replace("v", ""));
        var downloadUrl =
            $"https://github.com/connor-davis/AutobotUpdater/releases/download/v{latestGithubVersion}/AutobotUpdater.exe";

        using var httpClient = new HttpClient();

        try
        {
            await using var fileStream =
                new FileStream($"{DirectoryPath!}\\AutobotUpdater.exe", FileMode.Create, FileAccess.Write, FileShare.None);

            // Send an HTTP GET request to the URL and get the response
            using var response = await httpClient.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead);

            // Check if the response is successful
            response.EnsureSuccessStatusCode();

            // Create a buffer for downloading data in chunks
            var buffer = new byte[8192];
                
            // Create a stream to read the response content
            await using var contentStream = await response.Content.ReadAsStreamAsync();
            int bytesReadThisChunk;

            while ((bytesReadThisChunk = await contentStream.ReadAsync(buffer)) > 0)
            {
                // Write the downloaded data to the file
                await fileStream.WriteAsync(buffer.AsMemory(0, bytesReadThisChunk));
            }

            fileStream.Close();
            
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = $"{DirectoryPath!}\\AutobotUpdater.exe",
                    UseShellExecute = true,
                    CreateNoWindow = true
                };

                var process = new Process { StartInfo = startInfo };
                
                process.Start();
                
                Environment.Exit(0);
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                Console.WriteLine("Error");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}