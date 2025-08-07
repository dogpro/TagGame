using Cysharp.Threading.Tasks;
using System;
using UniRx;

public interface IMainMenu 
{
    IObservable<Unit> OnStartClicked { get; }
    IObservable<Unit> OnExitClicked { get; }

    void ShowMenu();
    UniTask HideMenu();
}
