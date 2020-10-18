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
    public GameEvent GameFinished;
    public Animator CrossSceneAnimator;
    public bool ShowLoading;
    #endregion

    #region Private Fields
    private int _loadingSceneIndex;
    #endregion

    #region Public Methods
    public void LoadScene(int sceneIndex)
    {
        _loadingSceneIndex = sceneIndex;
        CrossSceneAnimator.SetTrigger("StartLoading");
    }

    public void StartLoadScene()
    {
        StartCoroutine(LoadYourAsyncScene(Scenes[_loadingSceneIndex].SceneName));
    }
    #endregion

    #region Private Methods

    public void Awake()
    {
        if (ShowLoading)
        CrossSceneAnimator.SetTrigger("FinishLoading");
    }

    public void Start()
    {
        GameFinished.AddListener(this);
    }

    public void OnDestroy()
    {
        GameFinished.AddListener(this);
    }

    private IEnumerator LoadYourAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        CrossSceneAnimator.SetTrigger("StartLoading");
    }

    public void OnEventRaised()
    {
        LoadScene(CurrentScene.Value);
    }
    #endregion
}
