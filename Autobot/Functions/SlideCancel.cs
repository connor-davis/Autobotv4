using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Autobot.Config;
using Autobot.Utils;
using SharpHook;
using SharpHook.Native;

namespace Autobot.Functions;

public static class SlideCancel
{
    private static readonly EventSimulator Simulator = new();
    [SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible")] public static bool IsRunning;
    
    public static void Run(SlideCancelConfiguration configuration)
    {
        if (!configuration.Enabled) return;
        if (!WindowUtils.GetFocusedWindowTitle().StartsWith(Constants.CallOfDutyTitle)) return;
        if (IsRunning) return;
        
        IsRunning = true;
                    
        Thread.Sleep(80);
        KeyDown(KeyMapper.GetSharpHookKeyCode(configuration.SlideKey));
        Thread.Sleep(80);
        KeyUp(KeyMapper.GetSharpHookKeyCode(configuration.SlideKey));
        Thread.Sleep(60);
        KeyDown(KeyMapper.GetSharpHookKeyCode(configuration.JumpKey));
        Thread.Sleep(120);
        KeyUp(KeyMapper.GetSharpHookKeyCode(configuration.JumpKey));
        
        IsRunning = false;
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