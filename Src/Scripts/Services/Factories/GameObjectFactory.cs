using Basilisk.Interactions;
using Basilisk.Models;
using Godot;
using MonoCustomResourceRegistry;

namespace Basilisk.Services.Factories;

[RegisteredType(nameof(GameObjectFactory))]
public partial class GameObjectFactory: Node
{
    [Export] private PackedScene _gameObjectScene = null!;

    public void CreateGameObject(GameObject definition, IGameDataService dataService, Node parent)
    {
        var gameObj = _gameObjectScene.Instantiate<Node2D>();
        var sprite = gameObj.GetNode<Sprite2D>("Sprite");
        if (definition.Attributes?.Graphics != null && sprite != null)
        {
            sprite.Texture = ResourceLoader.Load<Texture2D>(dataService.GraphicsPath.PathJoin(definition.Attributes.Graphics));
        }

        foreach (var component in definition.Components)
        {
            var componentNode = ComponentFactory.CreateComponent(component.Type);
            gameObj.AddChild(componentNode);
            if (componentNode is IComponent componentInstance)
            {
                componentInstance.Init(gameObj);
            }
        }
        
        parent.AddChild(gameObj);
    }
}