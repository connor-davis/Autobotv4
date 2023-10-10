using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Autobot.Utils;

namespace Autobot;

public partial class UpdateWindow
{
    private static UpdateWindow? _instance;
    private readonly MainWindow _mainWindow = new();
    
    public UpdateWindow()
    {
        InitializeComponent();

        _instance = this;

        Task.Run(async () =>
        {
            await CheckForUpdates();
        });
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

    private void FinishUpdate()
    {
        Dispatcher.Invoke(() =>
        {
            try
            {
                Application.Current.Shutdown();
            }
            catch
            {
                // ignored
            }

            var process = new Process();

            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = false;
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();

            process.StandardInput.WriteLineAsync(@"copy /y .\Update\Autobot.dll Autobot.dll");
            process.StandardInput.FlushAsync();

            process.StandardInput.Close();

            process.WaitForExitAsync();
        });
    }

    private static async Task CheckForUpdates()
    {
        var updateStatus = await AutoUpdater.CheckForUpdates();
        
        if (updateStatus == UpdaterStatus.OutDated)
        {
            Console.WriteLine("Update found. Downloading the update.");
            _instance?.SetUpdateStatusContent(("Update found. Downloading the update."));
            
            try
            {
                await AutoUpdater.DownloadAndInstallUpdate();
                
                _instance?.SetUpdateStatusContent("Update downloaded. Please rerun Autobot.");
                
                Thread.Sleep(1000);
                
                _instance?.FinishUpdate();
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
}