using System.Collections.Generic;
using Basilisk.Models;

namespace Basilisk.Autoloads;

public class ShortTermMemory
{
    public Queue<MemorisedConceptModel> Memories { get; } = new();
    private readonly int _maxMemories;
    
    public ShortTermMemory(int maxMemories)
    {
        _maxMemories = maxMemories;
    }
    
    public void Add(MemorisedConceptModel memorisedConcept)
    {
        if (Memories.Count >= _maxMemories)
        {
            Memories.Dequeue();
        }
        Memories.Enqueue(memorisedConcept);
    }
}