﻿using System.Windows;
using System.Windows.Controls;
using Autobot.Config;
using Autobot.Utils;
using SharpHook;

namespace Autobot.Components.Dialogs;

public partial class SilentShotDialog
{
    private static bool _isLethalKeyBeingEdited;
    private static bool _isWeaponSwapKeyBeingEdited;
    
    public SilentShotDialog()
    {
        InitializeComponent();
        
        Visibility = Visibility.Collapsed;
        
        SetValue(Panel.ZIndexProperty, 50);
    }

    private void CloseBtn_OnClick(object sender, RoutedEventArgs e)
    {
        Visibility = Visibility.Collapsed;
    }

    private void LethalKey_OnClick(object sender, RoutedEventArgs e)
    {
        _isWeaponSwapKeyBeingEdited = false;
        WeaponSwapKey.IsChecked = false;
        
        if (_isLethalKeyBeingEdited)
        {
            _isLethalKeyBeingEdited = false;
            
            var configuration = MainWindow.GetConfiguration();
            
            if (configuration is not { SilentShotConfiguration: not null }) return;

            LethalKey.Content = configuration.SilentShotConfiguration.LethalKey;
        }
        else
        {
            _isLethalKeyBeingEdited = true;
        }
    }
    
    private void WeaponSwapKey_OnClick(object sender, RoutedEventArgs e)
    {
        _isLethalKeyBeingEdited = false;
        LethalKey.IsChecked = false;
        
        if (_isWeaponSwapKeyBeingEdited)
        {
            _isWeaponSwapKeyBeingEdited = false;
            
            var configuration = MainWindow.GetConfiguration();
            
            if (configuration is not { SilentShotConfiguration: not null }) return;

            WeaponSwapKey.Content = configuration.SilentShotConfiguration.WeaponSwapKey;
        }
        else
        {
            _isWeaponSwapKeyBeingEdited = true;
        }
    }
    
    public void GlobalHookKeyPressed(KeyboardHookEventArgs e)
    {
        if (_isLethalKeyBeingEdited)
        {
            var configuration = MainWindow.GetConfiguration();
            
            if (configuration is not { SilentShotConfiguration: not null }) return;
            
            configuration.SilentShotConfiguration.LethalKey = KeyMapper.GetKeyCode(e.Data.KeyCode);
            SLAPI.WriteToJsonFile(MainWindow.GetInstance()!.ConfigurationPath, configuration);
        } else if (_isWeaponSwapKeyBeingEdited)
        {
            var configuration = MainWindow.GetConfiguration();

            if (configuration is not { SilentShotConfiguration: not null }) return;

            configuration.SilentShotConfiguration.WeaponSwapKey = KeyMapper.GetKeyCode(e.Data.KeyCode);
            SLAPI.WriteToJsonFile(MainWindow.GetInstance()!.ConfigurationPath, configuration);
        }
    }
    
    private void LethalKeyDelayTxtBx_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            var delay = int.Parse(LethalKeyDelayTxtBx.Text);
            
            var configuration = MainWindow.GetConfiguration();

            if (configuration is not { SilentShotConfiguration: not null }) return;

            configuration.SilentShotConfiguration.LethalKeyDelay = delay;
            SLAPI.WriteToJsonFile(MainWindow.GetInstance()!.ConfigurationPath, configuration);
        }
        catch
        {
            // ignored
        }
    }

    private void WeaponSwapDelayTxtBx_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            var delay = int.Parse(WeaponSwapDelayTxtBx.Text);
            
            var configuration = MainWindow.GetConfiguration();

            if (configuration is not { SilentShotConfiguration: not null }) return;

            configuration.SilentShotConfiguration.WeaponSwapKeyDelay = delay;
            SLAPI.WriteToJsonFile(MainWindow.GetInstance()!.ConfigurationPath, configuration);
        }
        catch
        {
            // ignored
        }
    }
}