using UnityEngine;

public class StepsPresenter
{
    private readonly GameStatsModel _stats;

    public StepsPresenter(GameStatsModel stats)
    {
        _stats = stats;
    }

    public void AddStep()
    {
        _stats.Steps.Value++;
    }
}
