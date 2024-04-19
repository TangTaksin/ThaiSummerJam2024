using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestloadCombat : MonoBehaviour
{
    public string sceneName;
    public void LoadGame()
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public void UnloadSceneGame()
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        LoadGame();
    }
}
