using Basilisk.Autoloads;
using Basilisk.Models;
using Chickensoft.GoDotNet;
using Godot;
using MonoCustomResourceRegistry;

namespace Basilisk.Interactions;

[RegisteredType(nameof(CorruptedExaminable), baseType: nameof(Area2D))]
public partial class CorruptedExaminable: BaseExaminable
{
    private InsanityManager InsanityManager => this.Autoload<InsanityManager>();
    
    public override void OnExamined(Node viewport, InputEvent @event, int shapeIndx)
    {
        if (PlayerToolState.CurrentTool != PlayerTool.MagnifyingGlass) return;
        if (!IsTopMost) return;
		
        if (@event.IsActionPressed(MOUSE_DOWN))
        {
            Log.Print($"Sending examined concept to short term memory: {Conclusion}");
            Log.Print($"Concept {(string.IsNullOrEmpty(ClueId) ? IS_NOT_CLUE_TEXT: string.Format(IS_PART_OF_CLUE_TEXT, ClueId))}");
            MemorisedConceptModel concept = new()
            {
                ConceptUniqueId = ConceptUniqueId,
                Conclusion = Conclusion,
                ClueId = ClueId
            };
            ShortTermMemoryManager.Add(concept);
            InsanityManager.AddInsanity();
        }
    }
}