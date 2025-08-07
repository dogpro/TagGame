using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class WinPopupView : MonoBehaviour, IWinPopup
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

    public IObservable<Unit> OnClickMenuButton => _menuButton
        .OnClickAsObservable()
        .ThrottleFirst(TimeSpan.FromMilliseconds(300));

    public IObservable<Unit> OnClickRestartButton => _restartButton
        .OnClickAsObservable()
        .ThrottleFirst(TimeSpan.FromMilliseconds(300));

    public void ShowPopup()
    {
        _canvasGroup.alpha = 0;
        _popupRoot.localScale = Vector3.zero;
        _titleImage.color = new Color(1, 1, 1, 0);

        _canvasGroup.DOFade(1f, 0.3f).SetEase(Ease.OutQuad);
        _popupRoot.DOScale(1f, 0.6f).SetEase(Ease.OutBack);

        _titleImage.DOFade(1f, 0.5f).SetDelay(0.3f);
    }

    public void SetStats(int steps, float time)
    {
        _stepsText.text = $"{steps}";
        _timeText.text = $"{time / 60:00}:{time % 60:00}";
    }

    public async UniTask HidePopup()
    {
        await _canvasGroup.DOFade(0f, 0.2f).ToUniTask();
    }
}
