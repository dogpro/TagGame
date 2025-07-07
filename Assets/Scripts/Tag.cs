using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UniRx;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class Tag : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textNumber;
    [SerializeField] private Button _button;
    [SerializeField] private RectTransform _rectTransform;

    public ReactiveProperty<int> Number { get; private set; } = new();
    public ReactiveProperty<float> TagSize { get; private set; } = new();
    public ReactiveProperty<int> X { get; private set; } = new();
    public ReactiveProperty<int> Y { get; private set; } = new();

    private TagSpawner _spawner;

    [Inject]
    public void Construct(TagSpawner spawner)
    {
        _spawner = spawner;
    }

    public void Init()
    {
        Number
           .Subscribe(num => _textNumber.text = num.ToString())
           .AddTo(this);

        _rectTransform.sizeDelta = new Vector2(TagSize.Value, TagSize.Value);

        _button.OnClickAsObservable()
        .Subscribe(_ => _spawner.TryMove(this).Forget())
        .AddTo(this);
    }

    public async UniTask AnimateMove(Vector3 targetPosition, float duration)
    {
        await transform.DOLocalMove(targetPosition, duration).ToUniTask();
    }
}
