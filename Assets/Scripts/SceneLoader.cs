using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

public class SceneLoader
{
    private readonly IObjectResolver _container;

    public SceneLoader(IObjectResolver container)
    {
        _container = container;
    }

    public SceneLoader() {}

    public async UniTask LoadMenuSceneAsync()
    {

        await Addressables.LoadSceneAsync("MenuScene", LoadSceneMode.Single).ToUniTask();
    }

    public async UniTask LoadGameSceneAsync()
    {
        await Addressables.LoadSceneAsync("GameScene", LoadSceneMode.Single).ToUniTask();
    }

    public async UniTask<WinPopup> ShowWinPopupAsync(Transform parent)
    {
        var handle = Addressables.InstantiateAsync("WinPopup", parent);
        GameObject popup = await handle.Task;
        _container.InjectGameObject(popup);
        popup.GetComponent<WinPopup>().Init();

        return popup.GetComponent<WinPopup>();
    }
}
