﻿using System.Threading;
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
        if (!WindowUtils.GetFocusedWindowTitle().StartsWith(Constants.CallOfDutyTitle)) return;
        
        Thread.Sleep(configuration.LethalKeyDelay);
        KeyDown(KeyMapper.GetSharpHookKeyCode(configuration.LethalKey));
        Thread.Sleep(configuration.WeaponSwapKeyDelay);
        KeyDown(KeyMapper.GetSharpHookKeyCode(configuration.WeaponSwapKey));
        Thread.Sleep(35);
        KeyUp(KeyMapper.GetSharpHookKeyCode(configuration.WeaponSwapKey));
        Thread.Sleep(25);
        KeyUp(KeyMapper.GetSharpHookKeyCode(configuration.LethalKey));
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