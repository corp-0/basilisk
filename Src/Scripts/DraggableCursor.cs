using Basilisk.Interactions;
using Godot;

namespace Basilisk;

public partial class DraggableCursor : Area2D
{
    private const string MOUSE_DOWN = "mouse_down";

    private Draggable dragging;
    
    public override void _Input(InputEvent @event)
    {
        if (dragging != null)
        {
            if (dragging.Selected == false) dragging = null;
        }
        GlobalPosition = GetGlobalMousePosition();
        if (@event.IsActionPressed(MOUSE_DOWN) == false) return;
        var topMost = GetTopMostDraggableBasedOnYPos(GetTree().GetNodesInGroup("draggable"));
        if (topMost == null) return;
        topMost.Selected = true;
        dragging = topMost;
    }

    private Draggable GetTopMostDraggableBasedOnYPos(Godot.Collections.Array<Node> draggables)
    {
        Draggable topMos = null;
        foreach (var target in draggables)
        {
            if (target is not Draggable drag) continue;
            if (GetOverlappingAreas().Contains(drag) == false) continue;
            if (topMos == null) { topMos = drag; }
            if (drag.GlobalPosition.Y < topMos.GlobalPosition.Y) { topMos = drag; }
        }

        return topMos;
    }
}