using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TagView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textNumber;
    [SerializeField] private Button _moveButton;
    [SerializeField] private RectTransform _rectTransform;

    public IObservable<Unit> OnMoveButton => _moveButton
        .OnClickAsObservable()
        .ThrottleFirst(TimeSpan.FromMilliseconds(300));

    public void SetNumberText(int number)
    {
        _textNumber.text = number.ToString();
    }

    public void SetTagSize(float size)
    {
        _rectTransform.sizeDelta = new Vector2(size, size);
    }

    public async UniTask AnimateMove(Vector3 targetPosition, float duration)
    {
        await transform.DOLocalMove(targetPosition, duration).ToUniTask();
    }
}
