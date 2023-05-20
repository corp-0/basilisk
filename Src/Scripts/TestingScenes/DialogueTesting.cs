using System.Collections.Generic;
using Basilisk.Autoloads;
using Chickensoft.GoDotNet;
using Godot;
using MonoCustomResourceRegistry;

namespace Basilisk.TestingScenes;

[RegisteredType(nameof(DialogueTesting), baseType: nameof(Node2D))]
public partial class DialogueTesting: Node2D
{
    [Export] private ItemList ItemList { get; set; } = null!;
    private DialogueManager DialogueManager => this.Autoload<DialogueManager>();
    private Dictionary<int, string> _indexIdMap = new();
    public override void _Ready()
    {
        ItemList.Connect("item_selected", new Callable(this, nameof(OnItemSelected)));
        PopulateItems();
    }

    private void PopulateItems()
    {
        var index = 0;
        foreach (var dialogue in DialogueManager.Dialogues)
        {
            _indexIdMap.Add(index, dialogue.UniqueId);
            ItemList.AddItem(dialogue.UniqueId);
            index++;
        }
    }

    public void OnItemSelected(int index)
    {
        DialogueManager.QueueDialogue(_indexIdMap[index]);
        DialogueManager.StartDialogue();
    }
}