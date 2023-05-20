using System;
using Basilisk.Autoloads;
using Basilisk.Models;
using Chickensoft.GoDotLog;
using Chickensoft.GoDotNet;
using Godot;
using MonoCustomResourceRegistry;

namespace Basilisk.Interactions;

[RegisteredType(nameof(ShreddedExaminableSlave), baseType: nameof(Area2D))]
public partial class ShreddedExaminableSlave: BaseExaminable, IArrangeable
{
    [Export] private ShreddedExaminableMaster? _master;
    [Export] private Vector2 _distanceFromMaster;
    [Export] private float _tolerance = 0.1f;

    private ILog _log = new GDLog(nameof(ShreddedExaminableSlave));
    
    private InsanityManager InsanityManager => this.Autoload<InsanityManager>();


    public bool IsInRightPosition => EvaluateRightPosition();

    public override void _Ready()
    {
        if (_master == null)
        {
            _log.Err("Master is not set for slave examinable.");
            throw new NullReferenceException();
        }
        
        _master.RegisterSlave(this);
    }
    
    private bool EvaluateRightPosition()
    {
        return Math.Abs(_master!.Position.DistanceTo(Position) - _distanceFromMaster.Length()) < _tolerance;
    }


    public override void OnExamined(Node viewport, InputEvent @event, int shapeIndx)
    {
        if (PlayerToolState.CurrentTool != PlayerTool.MagnifyingGlass) return;
        if (!IsTopMost) return;
        if (!_master!.IsInRightPosition) return;
		
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