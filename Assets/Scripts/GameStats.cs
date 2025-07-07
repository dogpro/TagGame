using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using VContainer;

public class GameStats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _steps;
    [SerializeField] private TextMeshProUGUI _time;

    public ReactiveProperty<int> Steps { get; private set; } = new();
    public ReactiveProperty<TimeSpan> TimeElapsed { get; private set; } = new();

    private IDisposable _timerDisposable;

    private void Start()
    {
        TimeElapsed
            .Subscribe(time => _time.text = $"{time:mm\\:ss}")
            .AddTo(this);

        Steps
            .Subscribe(step => _steps.text = step.ToString())
            .AddTo(this);
    }

    public void StartTracking()
    {
        _timerDisposable = Observable
            .Interval(TimeSpan.FromSeconds(1))
            .Subscribe(_ => TimeElapsed.Value += TimeSpan.FromSeconds(1))
            .AddTo(this);
    }

    public void StopTracking()
    {
        _timerDisposable?.Dispose();
    }

    public void RegisterStep()
    {
        Steps.Value++;
    }
}
