using System;
using System.Windows.Input;

namespace Autobot.Config;

[Serializable]
public class SilentShotConfiguration
{
    public Key LethalKey { get; set; } = Key.E;
    public Key WeaponSwapKey { get; set; } = Key.D1;
    public int LethalKeyDelay { get; set; } = 21;
    public int WeaponSwapKeyDelay { get; set; } = 100;
    public bool Enabled { get; set; } = false;
}