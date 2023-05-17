using Basilisk.Autoloads;
using Chickensoft.GoDotLog;
using Chickensoft.GoDotNet;
using Godot;

namespace Basilisk;

public partial class Game: Node
{
    private readonly ILog _log = new GDLog("EntryPoint");
    [Export] private Node2D _worldNode = null!;
    private DialogueManager DialogueManager => this.Autoload<DialogueManager>();
    
    public override void _Ready()
    {
        _log.Print("Game started!");
        DialogueManager.QueueDialogue("intro_1");
        DialogueManager.QueueDialogue("intro_2");
        DialogueManager.QueueDialogue("intro_3");
        DialogueManager.QueueDialogue("intro_4");
        DialogueManager.StartDialogue();
    }
}