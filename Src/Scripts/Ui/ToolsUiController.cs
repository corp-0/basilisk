using Basilisk.Autoloads;
using Chickensoft.GoDotNet;
using Godot;
using MonoCustomResourceRegistry;

namespace Basilisk.Ui;

[RegisteredType(nameof(ToolsUiController))]
public partial class  ToolsUiController: Node
{
    private PlayerToolState PlayerToolState => this.Autoload<PlayerToolState>();
    [Export] private TextureButton _handButton = null!;
    [Export] private TextureButton _magnifyingGlassButton = null!;
    
    public override void _Ready()
    {
        _handButton.Connect("pressed", new Callable(this, nameof(OnHandButtonPressed)));
        _magnifyingGlassButton.Connect("pressed", new Callable(this, nameof(OnMagnifyingGlassButtonPressed)));
    }
    
    private void OnHandButtonPressed()
    {
        PlayerToolState.ChangeTool(PlayerTool.Hand);
    }
    
    private void OnMagnifyingGlassButtonPressed()
    {
        PlayerToolState.ChangeTool(PlayerTool.MagnifyingGlass);
    }
}