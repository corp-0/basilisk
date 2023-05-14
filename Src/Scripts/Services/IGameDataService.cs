using System.Collections.Generic;
using Basilisk.Models;


namespace Basilisk.Services;

public interface IGameDataService
{
    public List<GameObject> GameObjectsDataBase { get; }
    public string GameDataPath { get; }
    public string DocumentsPath { get; }
    public string GraphicsPath { get; }
    public void BuildGameObjectDatabase();
}