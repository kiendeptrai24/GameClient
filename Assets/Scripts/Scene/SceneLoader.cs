using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum SceneName
{
    GameStart,
    GameHome,
    Loading,
    GamePlay,
    Options,
    Lobby,
    LobbyReady
}

public class SceneLoader : Singleton<SceneLoader>
{

    [Header("Loading Screen")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private UnityEngine.UI.Slider progressBar;

    public void LoadScene(SceneName scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

    public void LoadSceneAsync(SceneName scene)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(scene));
    }

    private IEnumerator LoadSceneAsyncCoroutine(SceneName scene)
    {
        if (loadingScreen != null) loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(scene.ToString());

        while (!operation.isDone)
        {
            if (progressBar != null)
                progressBar.value = Mathf.Clamp01(operation.progress / 0.9f);
            yield return null;
        }

        if (loadingScreen != null) loadingScreen.SetActive(false);
    }
}
