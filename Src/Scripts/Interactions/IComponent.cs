using Godot;

namespace Basilisk.Interactions;

public interface IComponent
{
    public string Type { get; }
    public void Init(Node2D parent);
}