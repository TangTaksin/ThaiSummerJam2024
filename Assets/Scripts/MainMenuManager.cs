using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string SceneName;
    private Scene overlayScene;

    public void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = (currentIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextIndex);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
        overlayScene = SceneManager.GetSceneByName(SceneName);
    }

    public void UnloadSceneGame()
    {
        if (overlayScene.IsValid())
        {
            SceneManager.UnloadSceneAsync(overlayScene);
            overlayScene = default;
        }

    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
