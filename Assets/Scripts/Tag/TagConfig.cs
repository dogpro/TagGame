using UnityEngine;

[CreateAssetMenu(fileName = "TagConfig", menuName = "Configs/TagConfig")]
public class TagConfig : ScriptableObject
{
    [SerializeField] public int Number;
    [SerializeField] public int X;
    [SerializeField] public int Y;
    [SerializeField] public float MoveDuration = 0.25f;
    [SerializeField] public float Size;
    [SerializeField] public float Spacing;
}
