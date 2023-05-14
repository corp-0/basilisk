using System;
using System.Collections.Generic;
using Basilisk.Interactions;

namespace Basilisk.Models;

[Serializable]
public class GameObjectAttributes
{
    public string? UniqueName { get; set; }
    public string? Graphics { get; set; }
}

[Serializable]
public class ComponentDto
{
    public string? Type { get; set; }
}

[Serializable]
public class GameObjectDto
{
    public GameObjectAttributes? Attributes { get; set; }
    public List<ComponentDto> Components { get; set; } = new();
}

public class GameObject
{
    public GameObjectAttributes? Attributes { get; set; }
    public List<IComponent?> Components { get; set; } = new();
}