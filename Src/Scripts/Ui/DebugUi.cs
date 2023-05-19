using Basilisk.Autoloads;
using Chickensoft.GoDotNet;
using Godot;
using MonoCustomResourceRegistry;

namespace Basilisk.Ui;

[RegisteredType(nameof(DebugUi), baseType: nameof(Control))]
public partial class DebugUi: Control
{
    private InsanityManager InsanityManager => this.Autoload<InsanityManager>();
    [Export] private Label Insanity { get; set; } = null!;
    [Export] private Label InsanityPercent { get; set; } = null!;
    
    public override void _Ready()
    {
        InsanityManager.InsanityChanged += OnInsanityChanged;
    }

    private void OnInsanityChanged(int lvl, int percent)
    {
        Insanity.Text = lvl.ToString();
        InsanityPercent.Text = $"{percent}%";
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("toggle_debug_ui"))
            Visible = !Visible;
    }
}