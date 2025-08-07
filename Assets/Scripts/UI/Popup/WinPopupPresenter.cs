using Cysharp.Threading.Tasks;
using System;
using UniRx;
using UnityEngine;
using VContainer;

public class WinPopupPresenter : IDisposable
{
    private readonly IWinPopup _view;
    private readonly SceneLoader _sceneLoader;
    private readonly GameStatsModel _gameStats;
    private readonly CompositeDisposable _disposables = new CompositeDisposable();
    private readonly GameObject _popupObject;

    [Inject]
    public WinPopupPresenter(IWinPopup view, SceneLoader sceneLoader, GameStatsModel gameStats)
    {
        _view = view;
        _sceneLoader = sceneLoader;
        _gameStats = gameStats;
        _popupObject = ((MonoBehaviour)view).gameObject;

        Start();
    }

    public void Start()
    {
        _view.SetStats(_gameStats.Steps.Value, _gameStats.Time.Value);
        _view.ShowPopup();

        _view.OnClickMenuButton
            .Subscribe(async _ => await BackToMenu())
            .AddTo(_disposables);

        _view.OnClickRestartButton
            .Subscribe(async _ => await RestartGame())
            .AddTo(_disposables);
    }
    
    private async void Hide()
    {
        await _view.HidePopup();
        _gameStats.Reset();
        GameObject.Destroy(_popupObject);
    }

    private async UniTask RestartGame()
    {
        Hide();
        await _sceneLoader.LoadGameSceneAsync();
    }

    private async UniTask BackToMenu()
    {
        Hide();
        await _sceneLoader.LoadMenuSceneAsync();
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}
