using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataManager : MonoBehaviour, IGameEventListener
{
    public VaribleCollection PlayerData;
    private VaribleCollection _baseValues;

    public void Awake()
    {
        DontDestroyOnLoad(this);
        _baseValues = ScriptableObject.CreateInstance<VaribleCollection>();

        foreach (var data in PlayerData)
        {
            data.AddListener(this);
            _baseValues.Add(GameObject.Instantiate(data));

            InitVariable(data);
        }
    }

    public void OnDestroy()
    {
        for (int i = 0; i < PlayerData.Count; i++)
        {
            var data = PlayerData[i];
            data.RemoveListener(this);
            SaveVarible(data);
            data.BaseValue = _baseValues[i].BaseValue;
        }
        Destroy(_baseValues);
    }

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
        } else
        {
            SaveVarible(variable);
        }
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
            File.Delete(fileName);

        dataFile = File.Create(fileName);
        bf.Serialize(dataFile, variable.BaseValue);
        dataFile.Dispose();
    }

    public void OnEventRaised()
    {
        //throw new System.NotImplementedException();
    }
}
