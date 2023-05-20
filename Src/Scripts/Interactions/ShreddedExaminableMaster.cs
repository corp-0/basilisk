using System.Collections.Generic;
using Basilisk.Autoloads;
using Basilisk.Models;
using Chickensoft.GoDotNet;
using Godot;
using MonoCustomResourceRegistry;

namespace Basilisk.Interactions;

[RegisteredType(nameof(ShreddedExaminableMaster), baseType: nameof(Area2D))]
public partial class ShreddedExaminableMaster: BaseExaminable, IArrangeable
{
    public bool IsInRightPosition => EvaluateRightPosition();
    private readonly List<ShreddedExaminableSlave> _slaves = new();
    private InsanityManager InsanityManager => this.Autoload<InsanityManager>();

    public void RegisterSlave(ShreddedExaminableSlave slave)
    {
        _slaves.Add(slave);
    }
    
    private bool EvaluateRightPosition()
    {
        return _slaves.TrueForAll(slave => slave.IsInRightPosition);
    }
    
    public override void OnExamined(Node viewport, InputEvent @event, int shapeIndx)
    {
        if (PlayerToolState.CurrentTool != PlayerTool.MagnifyingGlass) return;
        if (!IsTopMost) return;
        if (!IsInRightPosition) return;
		
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
            if (IsCorrupted)
            {
                InsanityManager.AddInsanity();
            }
        }
    }
}