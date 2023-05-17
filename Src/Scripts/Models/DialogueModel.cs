using System;
using System.Collections.Generic;

namespace Basilisk.Models;

public class DialoguesFileModel
{
    public List<DialogueModel> Dialogues { get; init; } = new();
}

[Serializable]
public class DialogueModel
{
    public string UniqueId { get; init; } = null!;
    public string? Speaker { get; set; }
    public string Text { get; init; } = null!;
}