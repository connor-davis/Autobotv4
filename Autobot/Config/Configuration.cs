using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Autobot.Config;

[Serializable]
public class Configuration : INotifyPropertyChanged
{
    private SilentShotConfiguration _silentShotConfiguration = new()
    {
        LethalKey = Key.E,
        WeaponSwapKey = Key.D1,
        LethalKeyDelay = 21,
        WeaponSwapKeyDelay = 100,
        Enabled = false
    };

    public SilentShotConfiguration SilentShotConfiguration
    {
        get => _silentShotConfiguration;
        set
        {
            if (_silentShotConfiguration == value) return;
            
            _silentShotConfiguration = value;
            
            OnPropertyChanged(nameof(SilentShotConfiguration));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}