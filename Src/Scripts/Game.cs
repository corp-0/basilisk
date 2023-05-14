using System;
using Basilisk.Services;
using Basilisk.Services.Factories;
using Basilisk.Services.Impl;
using Chickensoft.GoDotLog;
using Chickensoft.GoDotNet;
using Godot;

namespace Basilisk;

public partial class Game: Node
{
    private readonly ILog _log = new GDLog("EntryPoint");
    private GameObjectFactory GameObjectFactory => this.Autoload<GameObjectFactory>();
    private IGameDataService _gameDataService = null!;
    [Export] private Node2D _worldNode = null!;

    private void SpawnDocuments()
    {
        var objectsParent = GetNode<Marker2D>("PilePositionMark");
        if (objectsParent == null) throw new NullReferenceException();
        foreach (var doc in _gameDataService.GameObjectsDataBase)
        {
            GameObjectFactory.CreateGameObject(doc, _gameDataService, objectsParent);
        }
    }
    
    public override void _Ready()
    {
        _gameDataService = new GameDataService();
        _log.Print($"Loading game data from {_gameDataService.GameDataPath}...");
        _gameDataService.BuildGameObjectDatabase();
        SpawnDocuments();
        _log.Print("Game started!");
    }
}