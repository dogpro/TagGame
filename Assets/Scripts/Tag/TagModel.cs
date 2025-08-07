using UniRx;

public class TagModel
{
    private ReactiveProperty<int> _number;
    private ReactiveProperty<int> _x;
    private ReactiveProperty<int> _y;
    private float _moveDuration;
    private ReactiveProperty<float> _size;
    private float _spacing;

    public ReactiveProperty<int> Number => _number;
    public ReactiveProperty<int> X => _x;
    public ReactiveProperty<int> Y => _y;
    public float MoveDuration => _moveDuration;
    public ReactiveProperty<float> Size => _size;
    public float Spacing => _spacing;

    public TagModel(TagConfig config)
    {
        _number = new ReactiveProperty<int>(config.Number);
        _x = new ReactiveProperty<int>(config.X);
        _y = new ReactiveProperty<int>(config.Y);
        _moveDuration = config.MoveDuration;
        _size = new ReactiveProperty<float>(config.Size);
        _spacing = config.Spacing;
    }

    public float GetCellSize()
    {
        return _size.Value + _spacing;
    }
}
