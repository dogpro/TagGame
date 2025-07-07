using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private CanvasGroup _canvasGroup;

    private SceneLoader _sceneLoader;

    private void Start()
    {
        _sceneLoader = new SceneLoader();
        AnimateMenu();

        _startButton.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromMilliseconds(300))
            .Subscribe(async _ => await StartGame())
            .AddTo(this);

        _exitButton.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromMilliseconds(300))
            .Subscribe(_ => ExitGame())
            .AddTo(this);
    }

    private void AnimateMenu()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1f, 0.5f).SetEase(Ease.OutQuad);

        _startButton.transform.localScale = Vector3.zero;
        _exitButton.transform.localScale = Vector3.zero;

        _startButton.transform.DOScale(1f, 0.5f)
            .SetDelay(0.3f)
            .SetEase(Ease.OutBack);

        _exitButton.transform.DOScale(1f, 0.5f)
            .SetDelay(0.6f)
            .SetEase(Ease.OutBack);
    }

    private async UniTask StartGame()
    {
        await _canvasGroup.DOFade(0f, 0.3f).ToUniTask();
        await UniTask.Delay(400);

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
}
