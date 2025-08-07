using UnityEngine;

public class GameBoardView : MonoBehaviour
{
    [SerializeField] private Transform _tagParent;
    [SerializeField] private GameObject _tagPrefab;

    public Transform TagParent => _tagParent;
    public Transform PopupParent => this.transform;
}
