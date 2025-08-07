using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour, IMainMenu
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private CanvasGroup _canvasGroup;

    public IObservable<Unit> OnStartClicked => _startButton
        .OnClickAsObservable()
        .ThrottleFirst(TimeSpan.FromMilliseconds(300));

    public IObservable<Unit> OnExitClicked => _exitButton
        .OnClickAsObservable()
        .ThrottleFirst(TimeSpan.FromMilliseconds(300));

    public void ShowMenu()
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

    public async UniTask HideMenu()
    {
        await _canvasGroup.DOFade(0f, 0.3f).ToUniTask();
        await UniTask.Delay(400);
    }

}
