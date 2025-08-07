using UnityEngine;

public class WinPopupPresenterFactory : MonoBehaviour
{

    private readonly SceneLoader _sceneLoader;
    private readonly GameStatsModel _gameStats;

    public WinPopupPresenterFactory(SceneLoader sceneLoader, GameStatsModel gameStats)
    {
        _sceneLoader = sceneLoader;
        _gameStats = gameStats;
    }

    public WinPopupPresenter Create(IWinPopup view)
    {
        return new WinPopupPresenter(view, _sceneLoader, _gameStats);
    }
}
