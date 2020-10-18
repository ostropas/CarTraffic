using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStatusChecker : MonoBehaviour
{
    public IntVariable CurrentLevel;
    public GameObjectCollection InstantiatedCars;
    public GameObjectCollection ActiveSpawners;
    public GameEvent CrashEvent;
    public BoolGameEvent LevelFinished;

    private void Awake()
    {
        CrashEvent.AddListener(FailGame);
    }

    private void FailGame()
    {
        _isFail = true;

        Debug.Log("Fail");
        LevelFinished.Raise(false);
    }

    private bool _isComplete;
    private bool _isFail;
    public void Update()
    {
        if (_isComplete || _isFail)
            return;

        _isComplete = CheckLevelComplete();

        if (_isComplete)
        {
            CurrentLevel.Value++;
            Debug.Log("Complete");
            LevelFinished.Raise(true);
        }
    }

    private bool CheckLevelComplete()
    {
        return InstantiatedCars.Count == 0 && ActiveSpawners.Count == 0;
    }
}
