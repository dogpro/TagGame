using Cysharp.Threading.Tasks;
using System;
using UniRx;
using UnityEngine;

public class TagPresentor : IDisposable
{
    private readonly TagModel _model;
    private readonly TagView _view;
    private readonly Action<TagPresentor> _onClick;

    private CompositeDisposable _disposable =  new CompositeDisposable();

    public TagModel Model => _model;

    public TagPresentor(TagModel model, TagView view, Action<TagPresentor> onClick)
    {
        _model = model;
        _view = view;
        _onClick = onClick;

        _model.Number
            .Subscribe(_view.SetNumberText)
            .AddTo(_disposable);

        _model.Size
            .Subscribe(_view.SetTagSize)
            .AddTo(_disposable);

        _view.OnMoveButton
            .Subscribe(_ => _onClick?.Invoke(this))
            .AddTo(_disposable);
    }

    public async UniTask AnimateMove(Vector3 position)
    {
        await _view.AnimateMove(position, _model.MoveDuration);
    }

    public void Dispose()
    {
        _disposable?.Dispose();
    }
}
