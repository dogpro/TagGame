using Cysharp.Threading.Tasks;
using System;
using UniRx;

public interface IWinPopup
{
    IObservable<Unit> OnClickMenuButton { get; }
    IObservable<Unit> OnClickRestartButton { get; }

    void ShowPopup();
    UniTask HidePopup();
    void SetStats(int steps, float time);
}
