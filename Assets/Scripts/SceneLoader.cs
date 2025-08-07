using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class SceneLoader
{

    public async UniTask LoadMenuSceneAsync()
    {

        await Addressables.LoadSceneAsync("MenuScene", LoadSceneMode.Single).ToUniTask();
    }

    public async UniTask LoadGameSceneAsync()
    {
        await Addressables.LoadSceneAsync("GameScene", LoadSceneMode.Single).ToUniTask();
    }
}
