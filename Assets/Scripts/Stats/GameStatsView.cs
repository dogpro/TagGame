using TMPro;
using UnityEngine;
using VContainer;
using UniRx;

public class GameStatsView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI steps;
    [SerializeField] private TextMeshProUGUI time;

    [Inject]
    public void Construct(GameStatsModel stats)
    {
        stats.Time.Subscribe(t =>
        {
            time.text = $"{t / 60:00}:{t % 60:00}";
        }).AddTo(this);

        stats.Steps.Subscribe(m =>
        {
            steps.text = $"{m}";
        }).AddTo(this);
    }
}
