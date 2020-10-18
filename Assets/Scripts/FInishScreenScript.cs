using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FInishScreenScript : MonoBehaviour, ScriptableObjectArchitecture.IGameEventListener<bool>
{
    public UnityEngine.UI.Text FinishText;
    public ScriptableObjectArchitecture.BoolGameEvent FinishGame;

    private void Awake()
    {
        FinishGame.AddListener(this);
    }

    private void OnDestroy()
    {
        FinishGame.RemoveListener(this);
    }

    public void OnEventRaised(bool value)
    {
        FinishText.gameObject.SetActive(true);
        FinishText.text = value ? "Complete!" : "Fail";
    }
}
