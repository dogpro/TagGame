using UnityEngine;

[CreateAssetMenu(fileName = "GameBoardConfig", menuName = "Configs/GameBoardConfig")]
public class GameBoardConfig : ScriptableObject
{
    [SerializeField] public int Size;
}
