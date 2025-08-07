using System;
using UnityEngine;
using VContainer;

public class TagFactory : ITagFactory
{
    private readonly GameObject _tagPrefab;
    private readonly IObjectResolver _resolver;

    public TagFactory(GameObject prefab, IObjectResolver resolver)
    {
        _tagPrefab = prefab;
        _resolver = resolver;
    }

    public TagPresentor CreateTag(TagConfig config, Transform parent, Vector2 pos, Action<TagPresentor> onClick)
    {
        var view = GameObject.Instantiate(_tagPrefab, parent);
        view.transform.localPosition = new Vector3(pos.x, pos.y, 0);
        _resolver.Inject(view);

        var model = new TagModel(config);

        var presentor = new TagPresentor(model, view.GetComponent<TagView>(), onClick);

        return presentor;
    }
}
