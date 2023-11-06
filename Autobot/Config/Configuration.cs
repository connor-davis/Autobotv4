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

    private SlideCancelConfiguration _slideCancelConfiguration = new()
    {
        SlideKey = Key.C,
        JumpKey = Key.Space,
        Enabled = false
    };

    private bool _mustBeep = true;

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

    public SlideCancelConfiguration SlideCancelConfiguration
    {
        get => _slideCancelConfiguration;
        set
        {
            if (_slideCancelConfiguration == value) return;

            _slideCancelConfiguration = value;
            
            OnPropertyChanged(nameof(SlideCancelConfiguration));
        }
    }

    public bool MustBeep
    {
        get => _mustBeep;
        set
        {
            if (_mustBeep == value) return;

            _mustBeep = value;
            
            OnPropertyChanged(nameof(MustBeep));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}