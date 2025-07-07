using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private TagSpawner _tagSpawner;
    [SerializeField] private GameStats _gameStats;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(_tagSpawner);
        builder.RegisterComponent(_gameStats);
        builder.Register<SceneLoader>(Lifetime.Singleton);

        builder.RegisterEntryPoint<GameController>();
    }
}
