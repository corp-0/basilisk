using System.Linq;
using Basilisk.Models;
using Chickensoft.GoDotLog;
using Chickensoft.GoDotNet;
using Godot;
using MonoCustomResourceRegistry;

namespace Basilisk.Autoloads;

[RegisteredType(nameof(ShortTermMemoryManager))]
public partial class ShortTermMemoryManager: Node
{
    [Export] private int _maxMemories = 7;
    private ShortTermMemory _shortTermMemory = null!;
    private TutorialTracker TutorialTracker => this.Autoload<TutorialTracker>();
    private readonly ILog _log = new GDLog(nameof(ShortTermMemoryManager));
    private DialogueManager DialogueManager => this.Autoload<DialogueManager>();
    
    public override void _Ready()
    {
        _log.Print($"Initialising {nameof(ShortTermMemoryManager)}...");
        _shortTermMemory = new ShortTermMemory(_maxMemories);
    }
    
    public void Add(MemorisedConceptModel memorisedConcept)
    {
        if (TutorialTracker.FirstTimeRemembering)
        {
            DialogueManager.QueueDialogue("should_remember");
            DialogueManager.QueueDialogue("system_thought_added");
            DialogueManager.StartDialogue();
        }
        
        if (_shortTermMemory.Memories.Count >= _maxMemories && TutorialTracker.FirstTimeForgetting)
        {
            DialogueManager.QueueDialogue("first_time_forgetting");
            DialogueManager.QueueDialogue("system_first_time_forgetting");
            DialogueManager.StartDialogue();
            TutorialTracker.FirstTimeForgetting = false;
        }
        
        _shortTermMemory.Add(memorisedConcept);
    }
    
    public bool IsInMemory(string conceptUniqueId)
    {
        return _shortTermMemory.Memories.Any(memorisedConcept => memorisedConcept.ConceptUniqueId == conceptUniqueId);
    }
}