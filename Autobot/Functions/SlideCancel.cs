using System;
using System.Threading;
using Autobot.Config;
using Autobot.Utils;
using SharpHook;
using SharpHook.Native;

namespace Autobot.Functions;

public static class SlideCancel
{
    private static readonly EventSimulator Simulator = new();
    public static bool isRunning = false;
    
    public static void Run(SlideCancelConfiguration configuration)
    {
        isRunning = true;
        
        if (!configuration.Enabled) return;

        if (configuration.NewSlideCancel)
        {
            isRunning = true;
            
            Thread.Sleep(60);
            KeyDown(KeyMapper.GetSharpHookKeyCode(configuration.JumpKey));
            Thread.Sleep(120);
            KeyUp(KeyMapper.GetSharpHookKeyCode(configuration.JumpKey));

            isRunning = false;
        }
        else
        {
            isRunning = true;
            
            Thread.Sleep(80);
            KeyDown(KeyMapper.GetSharpHookKeyCode(configuration.SlideKey));
            Thread.Sleep(80);
            KeyUp(KeyMapper.GetSharpHookKeyCode(configuration.SlideKey));
            Thread.Sleep(60);
            KeyDown(KeyMapper.GetSharpHookKeyCode(configuration.JumpKey));
            Thread.Sleep(120);
            KeyUp(KeyMapper.GetSharpHookKeyCode(configuration.JumpKey));
            
            isRunning = false;
        }
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