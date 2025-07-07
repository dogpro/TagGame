using System;
using VContainer.Unity;

public class GameController : IStartable, IDisposable
{
    private readonly TagSpawner _tagSpawner;
    private readonly GameStats _gameStats;
    private readonly SceneLoader _sceneLoader;

    public GameController(TagSpawner tagSpawner, GameStats gameStats, SceneLoader sceneLoader)
    {
        _tagSpawner = tagSpawner;
        _gameStats = gameStats;
        _sceneLoader = sceneLoader;
    }

    public void Start()
    {
        _gameStats.StartTracking();
        _tagSpawner.SpawnTags();
    }

    public void Dispose()
    {
        _gameStats.StopTracking();
    }
}
