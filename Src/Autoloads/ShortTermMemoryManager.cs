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
    
    public override void _Ready()
    {
        _shortTermMemory = new ShortTermMemory(_maxMemories);
    }
    
    public void Add(MemorisedConceptModel memorisedConcept)
    {
        if (_shortTermMemory.Memories.Count >= _maxMemories && TutorialTracker.FirstTimeForgetting)
        {
            //TODO: Trigger a dialog here
            _log.Print("First time forgetting a memory. This should trigger a dialog!");
            TutorialTracker.FirstTimeForgetting = false;
        }
        
        _shortTermMemory.Add(memorisedConcept);
    }
    
    public bool IsInMemory(string conceptUniqueId)
    {
        return _shortTermMemory.Memories.Any(memorisedConcept => memorisedConcept.ConceptUniqueId == conceptUniqueId);
    }
}