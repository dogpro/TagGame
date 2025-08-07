using UnityEngine;
using VContainer.Unity;

public class TimerPresenter : ITickable
{
    private readonly GameStatsModel _stats;
    private bool _isRunning;

    public TimerPresenter(GameStatsModel stats)
    {
        _stats = stats;
    }

    public void Start()
    {
        _isRunning = true;
    }

    public void Stop()
    {
        _isRunning = false;
    }

    public void Tick()
    {
        if (_isRunning)
        {
            _stats.Time.Value += Time.deltaTime;
        }
    }
}
