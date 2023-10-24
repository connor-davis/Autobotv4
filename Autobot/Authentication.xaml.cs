using System;
using System.Windows;
using System.Windows.Controls;
using Autobot.Utils;

namespace Autobot;

public partial class Authentication
{
    public static api KeyAuth = new(
        name: "Autobot",
        // ReSharper disable once StringLiteralTypo
        ownerid: "P4VflvP4pl",
        secret: "dd12a76806324652b5d77b3c3e9547d2b7ca38d9281fbbb8ce2d14dfe2f8346e",
        version: "1.0"
    );
    
    public Authentication()
    {
        InitializeComponent();
        
        KeyAuth.init();
    }

    private void Authentication_OnClosed(object? sender, EventArgs e)
    {
        
    }

    private void LoginBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (KeyTxtBx.Text.Length <= 0) return;
        
        KeyAuth.license(KeyTxtBx.Text);
            
        if (KeyAuth.response.success)
        {
            var mainWindow = new MainWindow();
        
            mainWindow.Show();
            Close();
        }
        else
            StatusLbl.Content = KeyAuth.response.message;
    }
}