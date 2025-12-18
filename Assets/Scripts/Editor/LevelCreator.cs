using UnityEngine;
using UnityEditor;
using System.IO;

public class LevelCreator
{
    [MenuItem("Tools/Create 10 Levels")]
    public static void CreateLevels()
    {
        string[] difficulties = { "Easy", "Easy", "Medium", "Medium", "Medium", "Hard", "Hard", "Hard", "Expert", "Expert" };
        int[] enemyCounts = { 5, 8, 12, 15, 18, 22, 25, 30, 35, 40 };

        string folderPath = "Assets/ScriptableObjects/Levels";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        for (int i = 0; i < 10; i++)
        {
            LevelData level = ScriptableObject.CreateInstance<LevelData>();
            level.levelIndex = i + 1;
            level.difficulty = difficulties[i];
            level.enemiesAmount = enemyCounts[i];

            string assetPath = $"{folderPath}/Level_{i + 1}.asset";
            AssetDatabase.CreateAsset(level, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Created 10 level ScriptableObjects!");
    }
}
