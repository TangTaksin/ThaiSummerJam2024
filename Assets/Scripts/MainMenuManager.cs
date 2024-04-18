using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public string sceneName;
    [SerializeField] private GameObject fadeScreen;

    private void Start()
    {
        SetFadeScreenAlpha(1f);
        StartCoroutine(FadeScreenAlpha(1f, 0f, 2f));
    }

    public void LoadNextScene()
    {
        StartCoroutine(FadeScreenAndLoadNextScene(0f, 1f, 2f));
    }

    private IEnumerator FadeScreenAndLoadNextScene(float startAlpha, float endAlpha, float duration)
    {
        yield return StartCoroutine(FadeScreenAlpha(startAlpha, endAlpha, duration));
        StartCoroutine(LoadNextSceneCoroutine());
    }

    private IEnumerator LoadNextSceneCoroutine()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = (currentIndex + 1) % SceneManager.sceneCountInBuildSettings;

        AudioManager.Instance.PlayMusic(AudioManager.Instance.bgmClips[nextIndex], 1);

        yield return new WaitForSeconds(1.5f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextIndex);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private IEnumerator FadeScreenAlpha(float startAlpha, float endAlpha, float duration)
    {
        Image fadeImage = fadeScreen.GetComponent<Image>();
        Color startColor = fadeImage.color;
        Color endColor = startColor;
        endColor.a = endAlpha;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(timer / duration);
            fadeImage.color = Color.Lerp(startColor, endColor, normalizedTime);
            yield return null;
        }

        fadeImage.color = endColor;
    }

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

    private void SetFadeScreenAlpha(float alpha)
    {
        Color color = fadeScreen.GetComponent<Image>().color;
        color.a = alpha;
        fadeScreen.GetComponent<Image>().color = color;
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
