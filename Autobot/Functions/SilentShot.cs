using System.Threading;
using Autobot.Config;
using Autobot.Utils;
using SharpHook;
using SharpHook.Native;

namespace Autobot.Functions;

public static class SilentShot
{
    private static readonly EventSimulator Simulator = new();
    
    public static void Run(SilentShotConfiguration configuration)
    {
        if (!configuration.Enabled) return;
        
        Thread.Sleep(configuration.LethalKeyDelay);
        KeyDown(KeyMapper.GetSharpHookKeyCode(configuration.LethalKey));
        Thread.Sleep(1);
        KeyUp(KeyMapper.GetSharpHookKeyCode(configuration.LethalKey));
        Thread.Sleep(configuration.WeaponSwapKeyDelay);
        KeyDown(KeyMapper.GetSharpHookKeyCode(configuration.WeaponSwapKey));
        Thread.Sleep(5);
        KeyUp(KeyMapper.GetSharpHookKeyCode(configuration.WeaponSwapKey));
    }

    private static void KeyDown(KeyCode keyCode)
    {
        Simulator.SimulateKeyPress(keyCode);
    }

    private static void KeyUp(KeyCode keyCode)
    {
        Simulator.SimulateKeyRelease(keyCode);
    }
}