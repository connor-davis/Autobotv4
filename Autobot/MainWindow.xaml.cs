using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Autobot.Config;
using Autobot.Functions;
using SharpHook;
using SharpHook.Native;
using SharpHook.Reactive;
using MouseButton = SharpHook.Native.MouseButton;

namespace Autobot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private static MainWindow? _instance;
        private readonly SimpleReactiveGlobalHook _globalHook = new();

        static Configuration Configuration { get; set; } = null!;

        public MainWindow()
        {
            InitializeComponent();

            _instance = this;

            if (!File.Exists("autobot-config.json")) File.Create("autobot-config.json");

            Configuration = SLAPI.ReadFromJsonFile<Configuration>(Directory.GetCurrentDirectory() + "//autobot-config.json") ?? new Configuration
            {
                SilentShotConfiguration = new SilentShotConfiguration
                {
                    LethalKey = Key.E,
                    WeaponSwapKey = Key.D1,
                    LethalKeyDelay = 21,
                    WeaponSwapKeyDelay = 100,
                    FinalWeaponSwapKeyDelay = 35,
                    FinalLethalKeyDelay = 25,
                    Enabled = false
                }
            };

            DataContext = Configuration;
            
            Subscribe();
        }

        public static MainWindow? GetInstance()
        {
            return _instance;
        }

        public static Configuration? GetConfiguration()
        {
            return Configuration;
        }

        private void MainWindow_OnClosed(object? sender, EventArgs e)
        {
            Configuration.SilentShotConfiguration.Enabled = false;
            
            SLAPI.WriteToJsonFile(Directory.GetCurrentDirectory() + "//autobot-config.json", Configuration);
            
            Unsubscribe();
        }

        private void Subscribe()
        {
            _globalHook.KeyPressed.Subscribe(GlobalHookKeyPressed);
            _globalHook.KeyPressed.Subscribe(SilentShotDialog.GlobalHookKeyPressed);
            _globalHook.MousePressed.Subscribe(GlobalHookMousePressed);

            new Thread(_globalHook.Run).Start();
        }

        private static void GlobalHookMousePressed(MouseHookEventArgs e)
        {
            if (e.Data.Button == MouseButton.Button1)
            {
                new Thread(RunSilentShot).Start();
            }
        }

        private static void RunSilentShot()
        {
            SilentShot.Run(Configuration!.SilentShotConfiguration);
        }

        public void UpdateSilentShotToggleBtnContent(string content)
        {
            Dispatcher.Invoke(() =>
            {
                SilentShotTgleBtn.Content = content;
            });
        }

        private static void GlobalHookKeyPressed(KeyboardHookEventArgs e)
        {
            if (e.Data.KeyCode == KeyCode.VcF6)
            {
                var configuration = MainWindow.GetConfiguration();
                var instance = MainWindow.GetInstance();
            
                if (configuration is not { SilentShotConfiguration: not null }) return;
                if (instance is null) return;
                
                if (configuration.SilentShotConfiguration.Enabled)
                {
                    configuration.SilentShotConfiguration.Enabled = false;
                    instance.UpdateSilentShotToggleBtnContent("Off");
                    
                    Console.Beep();
                    Console.Beep();
                }
                else
                {
                    configuration.SilentShotConfiguration.Enabled = true;
                    instance.UpdateSilentShotToggleBtnContent("On");
                    Console.Beep();
                }
            }
        }

        private void Unsubscribe()
        {
            _globalHook.Dispose();
        }

        private void SilentShotBtn_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SilentShotDialog.Visibility = Visibility.Visible;
        }
    }
}