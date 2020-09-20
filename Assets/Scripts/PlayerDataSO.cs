using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataSO : ScriptableObject
{

    public PlayerData PD;
}

[System.Serializable]
public class PlayerData
{
    public int Hp;
    public string kek;
}
