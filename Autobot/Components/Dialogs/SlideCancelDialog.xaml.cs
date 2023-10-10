using System.Windows;
using System.Windows.Controls;
using Autobot.Config;
using Autobot.Utils;
using SharpHook;

namespace Autobot.Components.Dialogs;

public partial class SlideCancelDialog
{
    private static bool _isSlideKeyBeingEdited;
    private static bool _isJumpKeyBeingEdited;
    
    public SlideCancelDialog()
    {
        InitializeComponent();

        Visibility = Visibility.Collapsed;
        
        SetValue(Panel.ZIndexProperty, 50);
    }

    private void CloseBtn_OnClick(object sender, RoutedEventArgs e)
    {
        Visibility = Visibility.Collapsed;
    }
    
    public void GlobalHookKeyPressed(KeyboardHookEventArgs e)
    {
        if (_isSlideKeyBeingEdited)
        {
            var configuration = MainWindow.GetConfiguration();
            
            if (configuration is not { SlideCancelConfiguration: not null }) return;
            
            configuration.SlideCancelConfiguration.SlideKey = KeyMapper.GetKeyCode(e.Data.KeyCode);
            
            SLAPI.WriteToJsonFile(MainWindow.GetInstance()!.ConfigurationPath, configuration);
        } else if (_isJumpKeyBeingEdited)
        {
            var configuration = MainWindow.GetConfiguration();

            if (configuration is not { SlideCancelConfiguration: not null }) return;

            configuration.SlideCancelConfiguration.JumpKey = KeyMapper.GetKeyCode(e.Data.KeyCode);
            SLAPI.WriteToJsonFile(MainWindow.GetInstance()!.ConfigurationPath, configuration);
        }
    }
    
    private void SlideKey_OnClick(object sender, RoutedEventArgs e)
    {
        _isJumpKeyBeingEdited = false;
        JumpKey.IsChecked = false;
        
        if (_isSlideKeyBeingEdited)
        {
            _isSlideKeyBeingEdited = false;
            
            var configuration = MainWindow.GetConfiguration();
            
            if (configuration is not { SlideCancelConfiguration: not null }) return;

            SlideKey.Content = configuration.SlideCancelConfiguration.SlideKey;
        }
        else
        {
            _isSlideKeyBeingEdited = true;
        }
    }

    private void JumpKey_OnClick(object sender, RoutedEventArgs e)
    {
        _isSlideKeyBeingEdited = false;
        SlideKey.IsChecked = false;
        
        if (_isJumpKeyBeingEdited)
        {
            _isJumpKeyBeingEdited = false;
            
            var configuration = MainWindow.GetConfiguration();
            
            if (configuration is not { SlideCancelConfiguration: not null }) return;

            JumpKey.Content = configuration.SlideCancelConfiguration.JumpKey;
        }
        else
        {
            _isJumpKeyBeingEdited = true;
        }
    }
}