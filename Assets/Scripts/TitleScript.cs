using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScript : MonoBehaviour
{
	#region Public Fields
	public GameEvent LoadGameEvent;
	#endregion

	#region Private Fields
	#endregion

	IEnumerator Start()
	{
		yield return new WaitForSeconds(1f);
		LoadGameEvent.Raise();
	}
}
