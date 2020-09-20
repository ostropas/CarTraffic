using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class LevelsDataBase : ScriptableObject
{
    public string TitleScenePath;

    public string LoadingScenePath;

    public List<string> LevelsPaths;

    //public Scene GetTitleScene()
    //{
    //    return Resources.Load<Scene>("kek");
    //}
}
