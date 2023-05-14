using Basilisk.Interactions;
using Basilisk.Models;
using Chickensoft.GoDotLog;
using Godot;

namespace Basilisk.Services.Factories;

public static class ComponentFactory
{
    private static  readonly ILog _log = new GDLog(nameof(ComponentFactory));
    public static IComponent? CreateComponent(ComponentDto dto)
    {
        switch (dto.Type)
        {
            case nameof(Draggable):
                return new Draggable();
            // Repeat for other component types
            default:
                _log.Err($"Unable to create component by name {dto.Type}. Did you make a typo?");
                return null;
        }
    }

    public static Node? CreateComponent(string type)
    {
        var dto = new ComponentDto { Type = type };
        if (CreateComponent(dto) is Node node)
        {
            return node;
        }
        
        _log.Err($"Unable to create component by name {type}. Did you make a typo?");
        return null;
    }
}