using System.Windows.Input;

namespace Autobot.Config;

public class SlideCancelConfiguration
{
    public Key SlideKey { get; set; } = Key.C;
    public Key JumpKey { get; set; } = Key.Space;
    public bool Enabled { get; set; } = false;
}