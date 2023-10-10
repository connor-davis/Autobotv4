using System.Windows;
using System.Windows.Controls;

namespace Autobot.Components.Dialogs;

public partial class SettingsDialog : UserControl
{
    public SettingsDialog()
    {
        InitializeComponent();
        Visibility = Visibility.Collapsed;
        
        SetValue(Panel.ZIndexProperty, 50);
    }

    private void CloseBtn_OnClick(object sender, RoutedEventArgs e)
    {
        Visibility = Visibility.Collapsed;
    }

    private void MustBeep_OnClick(object sender, RoutedEventArgs e)
    {
    }
}