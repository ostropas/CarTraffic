using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchManager : MonoBehaviour
{
    #region Initialization
    #endregion

    #region Public Fields
    public SceneCollection Scenes;
    #endregion

    #region Private Fields
    #endregion

    #region Public Methods
    public void LoadScene(IntVariable sceneIndex)
    {
        StartCoroutine(LoadYourAsyncScene(Scenes[sceneIndex].SceneName));
    }
    #endregion

    #region Private Methods

    private IEnumerator LoadYourAsyncScene(string sceneName)
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    #endregion
}
