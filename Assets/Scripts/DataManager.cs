using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DataManager : MonoBehaviour, IGameEventListener
{
    public bool LoadData;
    public bool SaveData;

    public VaribleCollection PlayerData;
    private VaribleCollection _baseValues;

    public void Awake()
    {
        if (LoadData)
        {
            _baseValues = ScriptableObject.CreateInstance<VaribleCollection>();

            foreach (var data in PlayerData)
            {
                _baseValues.Add(GameObject.Instantiate(data));

                InitVariable(data);
            }
        }

#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += ModeChanged;
#endif
    }

#if UNITY_EDITOR
    private void ModeChanged(PlayModeStateChange playMode)
    {
        if (playMode == PlayModeStateChange.ExitingPlayMode)
        {
            for (int i = 0; i < PlayerData.Count; i++)
            {
                var data = PlayerData[i];
                data.RemoveAll();
                SaveVarible(data);
                data.BaseValue = _baseValues[i].BaseValue;
            }
            Destroy(_baseValues);
        }
    }
#endif

    private void LoadVarible(BaseVariable variable)
    {
        BinaryFormatter bf = new BinaryFormatter();
        var fileName = Path.Combine(Application.persistentDataPath, $"{variable.name}.bin");

        FileStream dataFile;

        if (File.Exists(fileName))
        {
            dataFile = File.OpenRead(fileName);
            variable.BaseValue = bf.Deserialize(dataFile);
            dataFile.Dispose();
        }
    }

    public void InitVariable(BaseVariable variable)
    {
        if (IsVaribleSaved(variable))
        {
            LoadVarible(variable);
        }
        else
        {
            SaveVarible(variable);
        }

        variable.AddListener(this);
    }

    public bool IsVaribleSaved(BaseVariable variable)
    {
        var fileName = Path.Combine(Application.persistentDataPath, $"{variable.name}.bin");
        return File.Exists(fileName);
    }

    public void SaveVarible(BaseVariable variable)
    {
        BinaryFormatter bf = new BinaryFormatter();
        var fileName = Path.Combine(Application.persistentDataPath, $"{variable.name}.bin");

        FileStream dataFile;


        if (File.Exists(fileName))
        {
            dataFile = File.OpenWrite(fileName);
        }
        else
        {
            dataFile = File.Create(fileName);
        }

        bf.Serialize(dataFile, variable.BaseValue);
        dataFile.Dispose();
    }

    public void OnEventRaised()
    {
        if (SaveData)
            for (int i = 0; i < PlayerData.Count; i++)
            {
                var data = PlayerData[i];
                SaveVarible(data);
            }
    }
}
