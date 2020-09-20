using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchManager : MonoBehaviour
{
	#region Initialization
	void Awake()
	{
		_currentScene = SceneManager.GetActiveScene();		
		DontDestroyOnLoad(gameObject);
	}
	#endregion

	#region Public Fields
	#endregion

	#region Private Fields
	private Scene _currentScene;
	private Scene _nextScene;
	#endregion
	

	#region Public Methods
	public void LoadGame()
    {
		LoadScene("Level1");
    }
	#endregion

	#region Private Methods
	private void SwitchToNewScene()
    {
		_nextScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
		SceneManager.UnloadSceneAsync(_currentScene);
		_currentScene = _nextScene;
    }

	private void LoadScene(string sceneName)
    {
		StartCoroutine(LoadYourAsyncScene(sceneName));
    }

	private IEnumerator LoadYourAsyncScene(string sceneName)
	{
		// The Application loads the Scene in the background as the current Scene runs.
		// This is particularly good for creating loading screens.
		// You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
		// a sceneBuildIndex of 1 as shown in Build Settings.

		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

		// Wait until the asynchronous scene fully loads
		while (!asyncLoad.isDone)
		{
			yield return null;
		}
		SwitchToNewScene();
	}
	#endregion
}
