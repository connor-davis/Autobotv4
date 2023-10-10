using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Autobot.Config;
using Autobot.Functions;
using Autobot.Utils;
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

        private static readonly string? DirectoryPath =
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public readonly string ConfigurationPath = $"{DirectoryPath}/autobot-config.json";

        private static Configuration Configuration { get; set; } = null!;

        public MainWindow()
        {
            InitializeComponent();

            _instance = this;
            
            SetDataContext(ConfigurationPath);

            Subscribe();
        }

        private void SetDataContext(string configurationPath)
        {
            try
            {
                Configuration = SLAPI.ReadFromJsonFile<Configuration>($"{configurationPath}")!;
                
                DataContext = Configuration;
            }
            catch
            {
                Configuration = new Configuration
                {
                    SilentShotConfiguration = new SilentShotConfiguration
                    {
                        LethalKey = Key.E,
                        WeaponSwapKey = Key.D1,
                        LethalKeyDelay = 21,
                        WeaponSwapKeyDelay = 100,
                        Enabled = false
                    },
                    SlideCancelConfiguration = new SlideCancelConfiguration
                    {
                        SlideKey = Key.C,
                        JumpKey = Key.Space,
                        Enabled = false,
                        NewSlideCancel = true
                    }
                };
                
                SLAPI.WriteToJsonFile(ConfigurationPath, Configuration);
                
                DataContext = Configuration;
            }
        }

        public static MainWindow? GetInstance()
        {
            return _instance;
        }

        public static Configuration GetConfiguration()
        {
            return Configuration;
        }

        private void MainWindow_OnClosed(object? sender, EventArgs e)
        {
            Configuration.SilentShotConfiguration.Enabled = false;

            SLAPI.WriteToJsonFile($"{ConfigurationPath}", Configuration);

            Unsubscribe();
        }

        private void Subscribe()
        {
            _globalHook.KeyPressed.Subscribe(GlobalHookKeyPressed);
            _globalHook.KeyPressed.Subscribe(SilentShotDialog.GlobalHookKeyPressed);
            _globalHook.KeyPressed.Subscribe(SlideCancelDialog.GlobalHookKeyPressed);
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
            SilentShot.Run(Configuration.SilentShotConfiguration);
        }

        private static void RunSlideCancel()
        {
            SlideCancel.Run(Configuration.SlideCancelConfiguration);
        }

        private void UpdateSilentShotToggleBtnContent(string content)
        {
            Dispatcher.Invoke(() => { SilentShotTgleBtn.Content = content; });
        }

        // private void UpdateSlideCancelToggleBtnContent(string content)
        // {
        //     Dispatcher.Invoke(() =>
        //     {
        //         SlideCancelTglBtn.Content = content;
        //     });
        // }

        private static void GlobalHookKeyPressed(KeyboardHookEventArgs e)
        {
            var configuration = GetConfiguration();
            var instance = GetInstance();

            if (configuration is not { SilentShotConfiguration: not null }) return;
            if (instance is null) return;

            if (e.Data.KeyCode == KeyCode.VcF6)
            {
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
                // } else if (e.Data.KeyCode == KeyCode.VcF7)
                // {
                //     if (configuration.SlideCancelConfiguration.Enabled)
                //     {
                //         configuration.SlideCancelConfiguration.Enabled = false;
                //         instance.UpdateSlideCancelToggleBtnContent("Off");
                //         
                //         Console.Beep();
                //         Console.Beep();
                //     }
                //     else
                //     {
                //         configuration.SlideCancelConfiguration.Enabled = true;
                //         instance.UpdateSlideCancelToggleBtnContent("On");
                //         Console.Beep();
                //     }
            }
            else if (e.Data.KeyCode == KeyMapper.GetSharpHookKeyCode(configuration.SlideCancelConfiguration.SlideKey) &&
                     !SlideCancel.IsRunning)
            {
                new Thread(RunSlideCancel).Start();
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

        private void SilentShotTgleBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (Configuration is not { SilentShotConfiguration: not null }) return;

            if (Configuration.SilentShotConfiguration.Enabled)
            {
                Configuration.SilentShotConfiguration.Enabled = false;
                SilentShotTgleBtn.Content = "Off";

                Console.WriteLine($"Silent Shot: {Configuration.SilentShotConfiguration.Enabled}");

                Console.Beep();
                Console.Beep();
            }
            else
            {
                Configuration.SilentShotConfiguration.Enabled = true;
                SilentShotTgleBtn.Content = "On";

                Console.WriteLine($"Silent Shot: {Configuration.SilentShotConfiguration.Enabled}");

                Console.Beep();
            }
        }

        // private void SlideCancelBtn_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        // {
        //     SlideCancelDialog.Visibility = Visibility.Visible;
        // }
        //
        // private void SlideCancelTglBtn_OnClick(object sender, RoutedEventArgs e)
        // {
        //     if (Configuration is not { SlideCancelConfiguration: not null }) return;
        //         
        //     if (Configuration.SlideCancelConfiguration.Enabled)
        //     {
        //         Configuration.SlideCancelConfiguration.Enabled = false;
        //         SlideCancelTglBtn.Content = "Off";
        //
        //         Console.WriteLine($"Slide Cancel: {Configuration.SlideCancelConfiguration.Enabled}");
        //             
        //         Console.Beep();
        //         Console.Beep();
        //     }
        //     else
        //     {
        //         Configuration.SlideCancelConfiguration.Enabled = true;
        //         SlideCancelTglBtn.Content = "On";
        //
        //         Console.WriteLine($"Slide Cancel: {Configuration.SlideCancelConfiguration.Enabled}");
        //         
        //         Console.Beep();
        //     }
        // }
    }
}