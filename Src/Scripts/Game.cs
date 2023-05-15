using Chickensoft.GoDotLog;
using Godot;

namespace Basilisk;

public partial class Game: Node
{
    private readonly ILog _log = new GDLog("EntryPoint");
    [Export] private Node2D _worldNode = null!;
    
    public override void _Ready()
    {
        _log.Print("Game started!");
    }
}