using Godot;
using MonoCustomResourceRegistry;

namespace Basilisk.Autoloads;

[RegisteredType(nameof(TutorialTracker))]
public partial class TutorialTracker: Node
{
    public bool FirstTimeReceivingSanityPenance { get; set; } = true;
    public bool FirstTimeRemembering { get; set; } = true;
    public bool FirstTimeForgetting { get; set; } = true;
    public bool FirstTimeRememberingTwice { get; set; } = true;
}