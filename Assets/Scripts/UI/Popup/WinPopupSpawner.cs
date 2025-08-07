using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;

public class WinPopupSpawner
{
    private readonly IObjectResolver _container;

    public WinPopupSpawner(IObjectResolver container)
    {
        _container = container;
    }

    public async UniTask SpawnAsync(Transform parent)
    {
        var handle = Addressables.InstantiateAsync("WinPopup", parent);
        var gameObject = await handle.Task;

        _container.InjectGameObject(gameObject);

        var view = gameObject.GetComponent<IWinPopup>();
        var factory = _container.Resolve<WinPopupPresenterFactory>();
        var presenter = factory.Create(view);
    }
}
