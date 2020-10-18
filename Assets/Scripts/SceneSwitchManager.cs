using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchManager : MonoBehaviour, IGameEventListener<bool>
{
    #region Initialization
    #endregion

    #region Public Fields
    public SceneCollection Scenes;
    public IntVariable CurrentScene;
    public BoolGameEvent GameFinished;
    public Animator CrossSceneAnimator;
    public bool ShowLoading;
    #endregion

    #region Private Fields
    private int _loadingSceneIndex;
    #endregion

    #region Public Methods
    public void LoadScene(int sceneIndex)
    {
        this.Delay(1, () =>
        {
            _loadingSceneIndex = sceneIndex;
            CrossSceneAnimator.SetTrigger("StartLoading");
        });
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
        GameFinished.RemoveListener(this);
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

    public void OnEventRaised(bool complete)
    {
        if (CurrentScene.Value >= Scenes.Count)
        {
            CurrentScene.Value = 1;
        }

        LoadScene(CurrentScene.Value);
    }
    #endregion
}
