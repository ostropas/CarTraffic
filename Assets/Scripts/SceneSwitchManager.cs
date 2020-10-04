using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchManager : MonoBehaviour, IGameEventListener
{
    #region Initialization
    #endregion

    #region Public Fields
    public SceneCollection Scenes;
    public IntVariable CurrentScene;
    #endregion

    #region Private Fields
    #endregion

    #region Public Methods
    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadYourAsyncScene(Scenes[sceneIndex].SceneName));
    }
    #endregion

    #region Private Methods

    public void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void Start()
    {
        CurrentScene.AddListener(this);
    }

    public void OnDestroy()
    {
        CurrentScene.RemoveListener(this);
    }

    private IEnumerator LoadYourAsyncScene(string sceneName)
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void OnEventRaised()
    {
        LoadScene(CurrentScene.Value);
    }
    #endregion
}
