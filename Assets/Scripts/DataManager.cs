using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour
{
	private string _playerDataFilename = "data.bin";

	#region Initialization
	void Awake()
	{
		_playerDataFilename = Path.Combine(Application.persistentDataPath, _playerDataFilename);

		InitPlayerData();
		DontDestroyOnLoad(gameObject);
	}
	#endregion

	#region Public Fields
	public LevelsDataBase LevelsDataBase;
	public PlayerData PlayerData;
	[SerializeField]
	private PlayerDataSO _defaultPlayerData;
	#endregion

	private void InitPlayerData()
    {
		BinaryFormatter bf = new BinaryFormatter();

        FileStream dataFile;
        if (File.Exists(_playerDataFilename))
        {
			dataFile = File.OpenRead(_playerDataFilename);

			PlayerData = (PlayerData)bf.Deserialize(dataFile);
		} else
        {
			dataFile = File.Create(_playerDataFilename);

			PlayerData = _defaultPlayerData.PD;
			bf.Serialize(dataFile, PlayerData);
		}
    }
}
