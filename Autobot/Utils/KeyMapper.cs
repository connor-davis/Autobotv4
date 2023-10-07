using System;
using System.Collections.Generic;
using System.Windows.Input;
using SharpHook.Native;

namespace Autobot.Utils;

public static class KeyMapper
{
    private static readonly Dictionary<Key, KeyCode> KeyToSharpHookKeyCodeMap = new()
    {
        { Key.A, KeyCode.VcA },
        { Key.B, KeyCode.VcB },
        { Key.C, KeyCode.VcC },
        { Key.D, KeyCode.VcD },
        { Key.E, KeyCode.VcE },
        { Key.F, KeyCode.VcF },
        { Key.G, KeyCode.VcG },
        { Key.H, KeyCode.VcH },
        { Key.I, KeyCode.VcI },
        { Key.J, KeyCode.VcJ },
        { Key.K, KeyCode.VcK },
        { Key.L, KeyCode.VcL },
        { Key.M, KeyCode.VcM },
        { Key.N, KeyCode.VcN },
        { Key.O, KeyCode.VcO },
        { Key.P, KeyCode.VcP },
        { Key.Q, KeyCode.VcQ },
        { Key.R, KeyCode.VcR },
        { Key.S, KeyCode.VcS },
        { Key.T, KeyCode.VcT },
        { Key.U, KeyCode.VcU },
        { Key.V, KeyCode.VcV },
        { Key.W, KeyCode.VcW },
        { Key.X, KeyCode.VcX },
        { Key.Y, KeyCode.VcY },
        { Key.Z, KeyCode.VcZ },
        { Key.D0, KeyCode.Vc0 }, // 0
        { Key.D1, KeyCode.Vc1 }, // 1
        { Key.D2, KeyCode.Vc2 }, // 2
        { Key.D3, KeyCode.Vc3 }, // 3
        { Key.D4, KeyCode.Vc4 }, // 4
        { Key.D5, KeyCode.Vc5 }, // 5
        { Key.D6, KeyCode.Vc6 }, // 6
        { Key.D7, KeyCode.Vc7 }, // 7
        { Key.D8, KeyCode.Vc8 }, // 8
        { Key.D9, KeyCode.Vc9 }, // 9
        { Key.NumPad0, KeyCode.VcNumPad0 },
        { Key.NumPad1, KeyCode.VcNumPad1 },
        { Key.NumPad2, KeyCode.VcNumPad2 },
        { Key.NumPad3, KeyCode.VcNumPad3 },
        { Key.NumPad4, KeyCode.VcNumPad4 },
        { Key.NumPad5, KeyCode.VcNumPad5 },
        { Key.NumPad6, KeyCode.VcNumPad6 },
        { Key.NumPad7, KeyCode.VcNumPad7 },
        { Key.NumPad8, KeyCode.VcNumPad8 },
        { Key.NumPad9, KeyCode.VcNumPad9 },
        { Key.F1, KeyCode.VcF1 },
        { Key.F2, KeyCode.VcF2 },
        { Key.F3, KeyCode.VcF3 },
        { Key.F4, KeyCode.VcF4 },
        { Key.F5, KeyCode.VcF5 },
        { Key.F6, KeyCode.VcF6 },
        { Key.F7, KeyCode.VcF7 },
        { Key.F8, KeyCode.VcF8 },
        { Key.F9, KeyCode.VcF9 },
        { Key.F10, KeyCode.VcF10 },
        { Key.F11, KeyCode.VcF11 },
        { Key.F12, KeyCode.VcF12 },
        { Key.Back, KeyCode.VcBackspace },
        { Key.Tab, KeyCode.VcTab },
        { Key.Enter, KeyCode.VcEnter },
        { Key.Escape, KeyCode.VcEscape },
        { Key.Space, KeyCode.VcSpace },
        { Key.Left, KeyCode.VcLeft },
        { Key.Up, KeyCode.VcUp },
        { Key.Right, KeyCode.VcRight },
        { Key.Down, KeyCode.VcDown },
        { Key.Apps, KeyCode.VcAppBrowser },
        { Key.PrintScreen, KeyCode.VcPrintScreen }, // PrintScreen
        { Key.Insert, KeyCode.VcInsert },
        { Key.Delete, KeyCode.VcDelete },
        { Key.Home, KeyCode.VcHome },
        { Key.End, KeyCode.VcEnd },
        { Key.PageUp, KeyCode.VcPageUp },
        { Key.PageDown, KeyCode.VcPageDown },
        { Key.Add, KeyCode.VcNumPadAdd }, // Numpad +
        { Key.Subtract, KeyCode.VcNumPadSubtract }, // Numpad -
        { Key.Multiply, KeyCode.VcNumPadMultiply }, // Numpad *
        { Key.Divide, KeyCode.VcNumPadDivide }, // Numpad /
        { Key.NumLock, KeyCode.VcNumLock },
        { Key.Scroll, KeyCode.VcScrollDown }, // Scroll Lock
        { Key.CapsLock, KeyCode.VcCapsLock },
        { Key.LeftShift, KeyCode.VcLeftShift }, // Left Shift
        { Key.RightShift, KeyCode.VcRightShift }, // Right Shift
        { Key.LeftCtrl, KeyCode.VcLeftControl }, // Left Control
        { Key.RightCtrl, KeyCode.VcRightControl }, // Right Control
        { Key.LeftAlt, KeyCode.VcLeftAlt }, // Left Alt
        { Key.RightAlt, KeyCode.VcRightAlt }, // Right Alt
        { Key.LWin, KeyCode.VcLeftMeta }, // Left Windows key
        { Key.RWin, KeyCode.VcRightMeta }, // Right Windows key
        // Add more key mappings as needed
    };
    
    private static readonly Dictionary<KeyCode, Key> SharpHookKeyToKeyCodeMap = new()
    {
        { KeyCode.VcA, Key.A },
        { KeyCode.VcB, Key.B },
        { KeyCode.VcC, Key.C },
        { KeyCode.VcD, Key.D },
        { KeyCode.VcE, Key.E },
        { KeyCode.VcF, Key.F },
        { KeyCode.VcG, Key.G },
        { KeyCode.VcH, Key.H },
        { KeyCode.VcI, Key.I },
        { KeyCode.VcJ, Key.J },
        { KeyCode.VcK, Key.K },
        { KeyCode.VcL, Key.L },
        { KeyCode.VcM, Key.M },
        { KeyCode.VcN, Key.N },
        { KeyCode.VcO, Key.O },
        { KeyCode.VcP, Key.P },
        { KeyCode.VcQ, Key.Q },
        { KeyCode.VcR, Key.R },
        { KeyCode.VcS, Key.S },
        { KeyCode.VcT, Key.T },
        { KeyCode.VcU, Key.U },
        { KeyCode.VcV, Key.V },
        { KeyCode.VcW, Key.W },
        { KeyCode.VcX, Key.X },
        { KeyCode.VcY, Key.Y },
        { KeyCode.VcZ, Key.Z },
        { KeyCode.Vc0, Key.D0 },
        { KeyCode.Vc1, Key.D1 },
        { KeyCode.Vc2, Key.D2 },
        { KeyCode.Vc3, Key.D3 },
        { KeyCode.Vc4, Key.D4 },
        { KeyCode.Vc5, Key.D5 },
        { KeyCode.Vc6, Key.D6 },
        { KeyCode.Vc7, Key.D7 },
        { KeyCode.Vc8, Key.D8 },
        { KeyCode.Vc9, Key.D9 },
        { KeyCode.VcNumPad0, Key.NumPad0 },
        { KeyCode.VcNumPad1, Key.NumPad1 },
        { KeyCode.VcNumPad2, Key.NumPad2 },
        { KeyCode.VcNumPad3, Key.NumPad3 },
        { KeyCode.VcNumPad4, Key.NumPad4 },
        { KeyCode.VcNumPad5, Key.NumPad5 },
        { KeyCode.VcNumPad6, Key.NumPad6 },
        { KeyCode.VcNumPad7, Key.NumPad7 },
        { KeyCode.VcNumPad8, Key.NumPad8 },
        { KeyCode.VcNumPad9, Key.NumPad9 },
        { KeyCode.VcF1, Key.F1 },
        { KeyCode.VcF2, Key.F2 },
        { KeyCode.VcF3, Key.F3 },
        { KeyCode.VcF4, Key.F4 },
        { KeyCode.VcF5, Key.F5 },
        { KeyCode.VcF6, Key.F6 },
        { KeyCode.VcF7, Key.F7 },
        { KeyCode.VcF8, Key.F8 },
        { KeyCode.VcF9, Key.F9 },
        { KeyCode.VcF10, Key.F10 },
        { KeyCode.VcF11, Key.F11 },
        { KeyCode.VcF12, Key.F12 },
        { KeyCode.VcBackspace, Key.Back },
        { KeyCode.VcTab, Key.Tab },
        { KeyCode.VcEnter, Key.Enter },
        { KeyCode.VcEscape, Key.Escape },
        { KeyCode.VcSpace, Key.Space },
        { KeyCode.VcLeft, Key.Left },
        { KeyCode.VcUp, Key.Up },
        { KeyCode.VcRight, Key.Right },
        { KeyCode.VcDown, Key.Down },
        { KeyCode.VcAppBrowser, Key.Apps },
        { KeyCode.VcPrintScreen, Key.PrintScreen },
        { KeyCode.VcInsert, Key.Insert },
        { KeyCode.VcDelete, Key.Delete },
        { KeyCode.VcHome, Key.Home },
        { KeyCode.VcEnd, Key.End },
        { KeyCode.VcPageUp, Key.PageUp },
        { KeyCode.VcPageDown, Key.PageDown },
        { KeyCode.VcNumPadAdd, Key.Add },
        { KeyCode.VcNumPadSubtract, Key.Subtract },
        { KeyCode.VcNumPadMultiply, Key.Multiply },
        { KeyCode.VcNumPadDivide, Key.Divide },
        { KeyCode.VcNumLock, Key.NumLock },
        { KeyCode.VcScrollDown, Key.Scroll },
        { KeyCode.VcCapsLock, Key.CapsLock },
        { KeyCode.VcLeftShift, Key.LeftShift },
        { KeyCode.VcRightShift, Key.RightShift },
        { KeyCode.VcLeftControl, Key.LeftCtrl },
        { KeyCode.VcRightControl, Key.RightCtrl },
        { KeyCode.VcLeftAlt, Key.LeftAlt },
        { KeyCode.VcRightAlt, Key.RightAlt },
        { KeyCode.VcLeftMeta, Key.LWin },
        { KeyCode.VcRightMeta, Key.RWin },
        // Add more key mappings as needed
    };

    public static KeyCode GetSharpHookKeyCode(Key key)
    {
        if (KeyToSharpHookKeyCodeMap.TryGetValue(key, out var code))
        {
            return code;
        }

        throw new ArgumentException($"Key '{key}' is not mapped to a SharpHook KeyCode.");
    }

    public static Key GetKeyCode(KeyCode keyCode)
    {
        if (SharpHookKeyToKeyCodeMap.TryGetValue(keyCode, out var code))
        {
            return code;
        }

        throw new ArgumentException($"KeyCode '{keyCode}' is not mapped to a Windows Input Key.");
    }
}