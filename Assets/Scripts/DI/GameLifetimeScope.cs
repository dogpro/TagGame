using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private GameBoardView gameBoardView;
    [SerializeField] private TagConfig _tagConfig;
    [SerializeField] private GameObject tagViewPrefab;
    [SerializeField] private GameStatsView statsView;

    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);

        builder.RegisterInstance(gameBoardView);
        builder.RegisterInstance(_tagConfig);
        builder.RegisterInstance(tagViewPrefab);

        builder.RegisterComponent(statsView);

        builder.Register<WinPopupPresenter>(Lifetime.Transient);
        builder.Register<WinPopupSpawner>(Lifetime.Singleton);
        builder.Register<WinPopupPresenterFactory>(Lifetime.Singleton);

        builder.Register<GameBoardModel>(Lifetime.Singleton);
        builder.Register<TagModel>(Lifetime.Transient);
        builder.Register<ITagFactory, TagFactory>(Lifetime.Singleton);

        builder.Register<GameStatsModel>(Lifetime.Singleton);
        builder.Register<TimerPresenter>(Lifetime.Singleton).As<ITickable>().AsSelf();
        builder.Register<StepsPresenter>(Lifetime.Singleton);

        builder.RegisterEntryPoint<GameBoardPresenter>(Lifetime.Singleton);
    }
}
