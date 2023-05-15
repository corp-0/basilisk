using System;
using Godot;
using MonoCustomResourceRegistry;

namespace Basilisk.Autoloads;

public enum PlayerTool
{
    Hand,
    MagnifyingGlass,
}

[RegisteredType(nameof(PlayerToolState))]
public partial class PlayerToolState: Node
{
    public PlayerTool CurrentTool { get; private set; } = PlayerTool.Hand;
    public event Action<PlayerTool> OnToolChanged = null!;
    
    public void ChangeTool(PlayerTool tool)
    {
        CurrentTool = tool;
        OnToolChanged?.Invoke(tool);
    }
}