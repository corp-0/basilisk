using Basilisk.Autoloads;
using Chickensoft.GoDotNet;
using Godot;
using MonoCustomResourceRegistry;

namespace Basilisk.Ui;

[RegisteredType(nameof(DebugUi), baseType: nameof(Control))]
public partial class DebugUi: Control
{
    private InsanityManager InsanityManager => this.Autoload<InsanityManager>();
    private PlayerToolState PlayerToolState => this.Autoload<PlayerToolState>();
    [Export] private Label Insanity { get; set; } = null!;
    [Export] private Label InsanityPercent { get; set; } = null!;
    [Export] private Label CurrentTool { get; set; } = null!;
    [Export] private LineEdit SpawnerInput { get; set; } = null!;
    [Export] private Button SpawnButton { get; set; } = null!;
    
    public override void _Ready()
    {
        InsanityManager.InsanityChanged += OnInsanityChanged;
        PlayerToolState.ToolChanged += OnToolChanged;
        SpawnButton.Connect("pressed", Callable.From(OnSpawnButtonPressed));
    }

    private void OnSpawnButtonPressed()
    {
        var mousePos = GetGlobalMousePosition();
        var toSpawn = SpawnerInput.Text;
        var packedScene = ResourceLoader.Load<PackedScene>("res://Src/Prefabs/Documents/" + toSpawn + ".tscn");
        var instance = packedScene.Instantiate<Node2D>();
        GetTree().CurrentScene.GetNode<Marker2D>("PilePositionMark").AddChild(instance);
        instance.GlobalPosition = mousePos;
    }

    private void OnToolChanged(PlayerTool tool)
    {
        CurrentTool.Text = tool.ToString();
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