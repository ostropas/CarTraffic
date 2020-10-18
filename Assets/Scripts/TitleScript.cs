using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScript : MonoBehaviour
{
	#region Public Fields
	public BoolGameEvent LoadGame;
	#endregion

	#region Private Fields
	#endregion

	private void Start()
	{
		LoadGame.Raise(true);
	}
}
