using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class DataManager : MonoBehaviour, IGameEventListener
{
    public VaribleCollection PlayerData;

    public void Awake()
    {
        DontDestroyOnLoad(this);
        foreach (var data in PlayerData)
        {
            data.AddListener(this);
            SaveVaribles(data);
        }
    }

    public void OnDestroy()
    {
        
    }

    private void LoadVaribles()
    {

    }

    public void SaveVaribles(BaseVariable variable)
    {
        var jsonString = JsonUtility.ToJson(variable.BaseValue);
        Debug.Log(jsonString);
    }

    public void OnEventRaised()
    {
        //throw new System.NotImplementedException();
    }
}
