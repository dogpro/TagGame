using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MenuLifetimeScope : LifetimeScope
{
    [SerializeField] private MainMenuUI mainMenuUI;

    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);

        builder.RegisterComponent<IMainMenu>(mainMenuUI);
        builder.RegisterEntryPoint<MainMenuPresenter>(Lifetime.Singleton);
    }
}
