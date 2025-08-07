using UnityEngine;
using VContainer;
using VContainer.Unity;

public class BaseScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);
        builder.Register<SceneLoader>(Lifetime.Singleton);
    }
}
