using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Autobot.Components;

public partial class Titlebar
{
    public Titlebar()
    {
        InitializeComponent();

        DragArea.Content = $"Autobot v{Constants.Version}";
    }
    
    private void DragArea_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            MainWindow.GetInstance()?.DragMove();
        }
    }

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
    {
        MainWindow.GetInstance()?.Close();
        Environment.Exit(0);
    }

    private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
    {
        MainWindow.GetInstance()!.WindowState = WindowState.Minimized;
    }
}