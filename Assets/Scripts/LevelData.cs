using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    public int levelIndex;
    public string difficulty;
    public int enemiesAmount;
}
