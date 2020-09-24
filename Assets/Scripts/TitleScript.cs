using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScript : MonoBehaviour
{
	#region Public Fields
	public GameEventBase LoadGame;
	#endregion

	#region Private Fields
	#endregion

	IEnumerator Start()
	{
		yield return new WaitForSeconds(1f);
		LoadGame.Raise();
	}
}
