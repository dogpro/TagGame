using Cysharp.Threading.Tasks;
using System;
using UniRx;
using VContainer;
using VContainer.Unity;

public class MainMenuPresenter : IDisposable, IStartable
{
    private IMainMenu _view;
    private CompositeDisposable _disposables = new();

    [Inject] private SceneLoader _sceneLoader;

    public MainMenuPresenter(IMainMenu view)
    {
        _view = view;
    }

    public void Start()
    {
        _view.ShowMenu();

        _view.OnStartClicked
            .Subscribe(async _ => await StartGame())
            .AddTo(_disposables);

        _view.OnExitClicked
            .Subscribe(_ => ExitGame())
            .AddTo(_disposables);
    }

    private async UniTask StartGame()
    {
        await _view.HideMenu();
        await _sceneLoader.LoadGameSceneAsync();
    }

    private void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Dispose()
    {
        _disposables?.Dispose();
    }
}
