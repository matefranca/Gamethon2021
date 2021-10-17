using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : Singleton<SceneLoader>
{
    [Header("Loading Screen.")]
    [SerializeField]
    private GameObject loadingScreen;
    [SerializeField]
    private Image loadingBar;

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingScreen.SetActive(true);
        loadingBar.fillAmount = 0;
        
        yield return new WaitForSeconds(0.5f);
        AsyncOperation handle = SceneManager.LoadSceneAsync(sceneName);
        while (handle.progress < 1)
        {
            loadingBar.fillAmount = handle.progress;
            yield return new WaitForEndOfFrame();
        }

        loadingScreen.SetActive(false);
    }
}
