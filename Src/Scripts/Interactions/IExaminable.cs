using Godot;

namespace Basilisk.Interactions;

public interface IExaminable
{
    void OnExamined(Node viewport, InputEvent @event, int shapeIndx);
}