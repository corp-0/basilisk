using System;
using Godot;
using MonoCustomResourceRegistry;

namespace Basilisk.Autoloads;

[RegisteredType(nameof(InsanityManager))]
public partial class InsanityManager: Node
{
    private int CurrentInsanity { get; set; }
    [Export] private int DefaultInsanityPenalty { get; set; }
    [Export] private int MaxInsanity { get; set; }
    private int CurrentInsanityPercent => (CurrentInsanity / MaxInsanity) * 100;
    
    //current amount, current amount in percentage
    public event Action<int, int>? InsanityChanged;
    
    public void AddInsanity(int? amount = null)
    {
        CurrentInsanity += amount ?? DefaultInsanityPenalty;
        if (CurrentInsanity > MaxInsanity)
            CurrentInsanity = MaxInsanity;
        InsanityChanged?.Invoke(CurrentInsanity, CurrentInsanity / CurrentInsanityPercent);
    }
    
    public void ResetInsanity()
    {
        CurrentInsanity = 0;
        InsanityChanged?.Invoke(CurrentInsanity, CurrentInsanity / CurrentInsanityPercent);
    }
}