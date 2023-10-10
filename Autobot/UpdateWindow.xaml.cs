using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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

        if (!File.Exists("AutobotUpdater.exe"))
        {
            new Task(DownloadUpdater).Start();
        }
        else
        {
            new Task(CheckForUpdates).Start();
        }
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

    private void SetUpdateStatusContent(string content)
    {
        Dispatcher.Invoke(() =>
        {
            UpdateStatus.Content = content;
        });
    }

    private static async void CheckForUpdates()
    {
        var updateStatus = await AutoUpdater.CheckForUpdates();
        
        if (updateStatus == UpdaterStatus.OutDated)
        {
            Console.WriteLine("Update found. Downloading the update.");

            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = $"{DirectoryPath!}\\AutobotUpdater.exe",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Process.Start(startInfo);

                Environment.Exit(0);
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                Console.WriteLine("Error");
            }
        }
        else
        {
            Console.WriteLine("No update found.");
            
            _instance?.CloseWindow();
            _instance?.OpenMainWindow();
        }
    }
    
    private static async void DownloadUpdater()
    {
        var gitHubClient = new GitHubClient(new ProductHeaderValue("connor-davis"));
        var releases = await gitHubClient.Repository.Release.GetAll("connor-davis", "AutobotUpdater");
        var latestGithubVersion = new Version(releases[0].TagName.Replace("v", ""));
        var downloadUrl =
            $"https://github.com/connor-davis/AutobotUpdater/releases/download/v{latestGithubVersion}/AutobotUpdater.exe";

        using var httpClient = new HttpClient();

        try
        {
            await using var fileStream =
                new FileStream("AutobotUpdater.exe", FileMode.Create, FileAccess.Write, FileShare.None);

            // Send an HTTP GET request to the URL and get the response
            using var response = await httpClient.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead);

            // Check if the response is successful
            response.EnsureSuccessStatusCode();

            // Get the content length (file size) from the response headers
            var totalBytes = response.Content.Headers.ContentLength.GetValueOrDefault();

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
            
            new Task(CheckForUpdates).Start();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}