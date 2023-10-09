using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Autobot.Utils;

public class WindowUtils
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();
    
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
    
    public static string GetFocusedWindowTitle()
    {
        var foregroundWindowHandle = GetForegroundWindow();

        const int maxTitleLength = 256;
        var windowTitle = new StringBuilder(maxTitleLength);

        var length = GetWindowText(foregroundWindowHandle, windowTitle, maxTitleLength);

        return length > 0 ? windowTitle.ToString() : "No focused window found";
    }
}