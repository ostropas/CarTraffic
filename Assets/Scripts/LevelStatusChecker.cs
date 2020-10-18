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

    private void Awake()
    {
        CrashEvent.AddListener(FailGame);
    }

    private void FailGame()
    {
        _isFail = true;
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
            Debug.Log("Complete");
            return;
        }

        if (_isFail)
        {
            Debug.Log("Fail");
            return;
        }
    }

    private bool CheckLevelComplete()
    {
        return InstantiatedCars.Count == 0 && ActiveSpawners.Count == 0;
    }
}
