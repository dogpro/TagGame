using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

public class WinPopup : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _popupRoot;

    [Space]
    [SerializeField] private Image _titleImage;
    [SerializeField] private GameObject _stepsObj;
    [SerializeField] private TextMeshProUGUI _stepsText;
    [SerializeField] private GameObject _timeObj;
    [SerializeField] private TextMeshProUGUI _timeText;

    [Space]
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _restartButton;

    [Inject] private SceneLoader sceneLoader;

    private GameStats _gameStats;

    [Inject]
    public void Construct(GameStats gameStats)
    {
        _gameStats = gameStats;
    }

    public void Init()
    {
        PlayPopupAnimation();

        _menuButton.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromMilliseconds(300))
            .Subscribe(async _ => await LoadMenu())
            .AddTo(this);

        _restartButton.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromMilliseconds(300))
            .Subscribe(async _ => await RestartGame())
            .AddTo(this);

        _stepsText.text = $"{_gameStats.Steps.Value}";
        _timeText.text = $"{_gameStats.TimeElapsed.Value:mm\\:ss}";
    }

    private void PlayPopupAnimation()
    {
        _canvasGroup.alpha = 0;
        _popupRoot.localScale = Vector3.zero;
        _titleImage.color = new Color(1, 1, 1, 0);

        _canvasGroup.DOFade(1f, 0.3f).SetEase(Ease.OutQuad);
        _popupRoot.DOScale(1f, 0.6f).SetEase(Ease.OutBack);

        _titleImage.DOFade(1f, 0.5f).SetDelay(0.3f);
    }

    private async UniTask LoadMenu()
    {
        await _canvasGroup.DOFade(0f, 0.2f).ToUniTask();
        await sceneLoader.LoadMenuSceneAsync();
    }

    private async UniTask RestartGame()
    {
        await _canvasGroup.DOFade(0f, 0.2f).ToUniTask();

        await sceneLoader.LoadGameSceneAsync();
    }
}
