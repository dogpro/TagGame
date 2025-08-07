using System;
using System.Drawing;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameBoardPresenter : IStartable, IDisposable
{
    private readonly GameBoardModel _model;
    private readonly GameBoardView _view;
    private readonly ITagFactory _tagFactory;
    private readonly TagConfig _tagConfig;
    private readonly TimerPresenter _timer;
    private readonly StepsPresenter _steps;

    private CompositeDisposable _disposable = new CompositeDisposable();
    private TagPresentor[,] _presenters;

    [Inject] private WinPopupSpawner _winPopupSpawner;

    public GameBoardPresenter(TagConfig tagConfig, TimerPresenter timer,
        GameBoardModel model, GameBoardView view, 
        ITagFactory tagFactory, StepsPresenter steps)
    {
        _model = model;
        _view = view;
        _tagFactory = tagFactory;
        _tagConfig = tagConfig;
        _timer = timer;
        _steps = steps;
    }

    public async void Start()
    {
        await _model.GenerateTagsMatrix();
        _model.SpawnPrepare();

        _model.OnTagMoved.Subscribe(async presenter =>
        {
            _steps.AddStep();

            var model = presenter.Model;
            Vector3 target = _model.GetLocalPosition(model.X.Value, model.Y.Value, model.Size.Value);
            await presenter.AnimateMove(target);
        });

        _model.OnWin.Subscribe( async _ => 
        {
            _timer.Stop();
            await _winPopupSpawner.SpawnAsync(_view.PopupParent);
        });

        SpawnTags();
        _timer.Start();
    }

    

    public void SpawnTags()
    {
        int size = _model.Size;
        _presenters = new TagPresentor[size, size];

        for (int y = 0; y < _model.Size; y++)
        {
            for (int x = 0; x < _model.Size; x++)
            {
                if (_model.Matrix[x, y] == null) continue;

                var config = new TagConfig()
                {
                    Number = (int)_model.Matrix[x, y],
                    X = x,
                    Y = y,
                    Size = _tagConfig.Size,
                    Spacing = _tagConfig.Spacing
                };

                var localPos = _model.GetLocalPosition(x, y, config.Size);

                var presenter = _tagFactory.CreateTag(config, _view.TagParent,
                    localPos, OnTagClicked);

                _presenters[x, y] = presenter;
            }
        }
    }

    private void OnTagClicked(TagPresentor presenter)
    {
        _model.TryMove(presenter).Forget();
    }

    public void Dispose()
    {
        _disposable?.Dispose();
    }
}
