using System.Collections.Generic;
using System.Linq;
using Basilisk.Interactions;
using Basilisk.Models;
using Basilisk.Services.Factories;
using Chickensoft.GoDotLog;
using Godot;
using Tomlyn;

namespace Basilisk.Services.Impl;

public class GameDataService: IGameDataService
{
    private readonly ILog _log = new GDLog(nameof(GameDataService));
    public List<GameObject> GameObjectsDataBase { get; } = new();
    public string GameDataPath => OS.HasFeature("editor") ? ProjectSettings.GlobalizePath("res://GameData") : OS.GetExecutablePath().PathJoin("GameData");
    public string DocumentsPath => GameDataPath.PathJoin("Documents");
    public string GraphicsPath => GameDataPath.PathJoin("Graphics");

    public void BuildGameObjectDatabase()
    {
        _log.Print("Building game object database.");
        var dir = DirAccess.Open(DocumentsPath);
        var definitionFiles = dir.GetFiles();
        _log.Print($"Found {definitionFiles.Length} definition files.");

        foreach (var definition in definitionFiles)
        {
            _log.Print($"Loading definition file: {definition}.");
            var content = FileAccess.GetFileAsString(DocumentsPath.PathJoin(definition));

            if (Toml.TryToModel<GameObjectDto>(content, out var dto, out var diagnostics))
            {
                var gameObject = MapToGameObject(dto);
                GameObjectsDataBase.Add(gameObject);
                _log.Print($"Game object with ID \"{gameObject.Attributes?.UniqueName}\" loaded successfully from \"{definition}\".");
                continue;
            }

            _log.Err($"Failed to load definition file: {definition}");
            var errors = diagnostics.GetEnumerator();
            while (errors.MoveNext())
            {
                _log.Err(errors.Current.ToString());
            }

            errors.Dispose();
        }
    }
    
    private GameObject MapToGameObject(GameObjectDto dto)
    {
        var gameObject = new GameObject
        {
            Attributes = dto.Attributes != null ? new GameObjectAttributes { UniqueName = dto.Attributes.UniqueName, Graphics = dto.Attributes.Graphics } : null,
            Components = dto.Components.Select(MapToComponent).ToList()
        };

        return gameObject;
    }
    
    private IComponent? MapToComponent(ComponentDto dto)
    {
        return ComponentFactory.CreateComponent(dto);
    }
}