﻿using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Autobot.Utils;
using Octokit;
using Application = System.Windows.Application;
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

    private async void CheckForUpdates()
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
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                var process = new Process { StartInfo = startInfo };
                
                process.Start();
                
                Dispatcher.Invoke(() =>
                {
                    Application.Current.Shutdown();
                });
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
    
    private async void DownloadUpdater()
    {
        var gitHubClient = new GitHubClient(new ProductHeaderValue("connor-davis"))
        {
            Credentials = new Credentials("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")
        };
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