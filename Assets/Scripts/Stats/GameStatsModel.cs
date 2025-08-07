using UniRx;

public class GameStatsModel
{
    public ReactiveProperty<int> Steps { get; } = new();
    public ReactiveProperty<float> Time { get; } = new();

    public void Reset()
    {
        Steps.Value = 0;
        Time.Value = 0;
    }
}
