using System;
using UnityEngine;

public interface ITagFactory 
{
    TagPresentor CreateTag(TagConfig config, Transform parent, Vector2 pos, Action<TagPresentor> onClick);
}
