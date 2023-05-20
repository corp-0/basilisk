using System;
using Chickensoft.GoDotLog;
using Godot;
using MonoCustomResourceRegistry;

namespace Basilisk.Autoloads;

[RegisteredType(nameof(InsanityManager))]
public partial class InsanityManager: Node
{
    private int CurrentInsanity { get; set; }
    [Export] private int DefaultInsanityPenalty { get; set; }
    [Export] private int MaxInsanity { get; set; }
    private int CurrentInsanityPercent => (int)((float)CurrentInsanity / MaxInsanity * 100);
    private ILog _log = new GDLog(nameof(InsanityManager));
    
    /// <summary>
    /// Broadcasts the current insanity and insanity percent whenever the insanity changes.
    /// </summary>
    public event Action<int, int>? InsanityChanged;

    public override void _Ready()
    {
        _log.Print($"Initialising {nameof(InsanityManager)} with {DefaultInsanityPenalty} default penalty and {MaxInsanity} max insanity...");
    }

    public void AddInsanity(int? amount = null)
    {
        CurrentInsanity += amount ?? DefaultInsanityPenalty;
        if (CurrentInsanity > MaxInsanity)
        {
            CurrentInsanity = MaxInsanity;
        }
        InsanityChanged?.Invoke(CurrentInsanity, CurrentInsanityPercent);
    }
    
    public void ResetInsanity()
    {
        CurrentInsanity = 0;
        InsanityChanged?.Invoke(CurrentInsanity, CurrentInsanityPercent);
    }
}