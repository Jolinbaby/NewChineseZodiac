using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : Singleton<TransitionManager>
{
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration;
    private bool isFade;

    public void Transition(string from, string to, bool isMain)
    {
        if (!isFade)
        {
            StartCoroutine(TransitionToScene(from, to, isMain));
        }
    }

    private IEnumerator TransitionToScene(string from, string to, bool isMain)
    {
        yield return Fade(1);
        
        if (isMain)
        {
            Debug.Log("进入切换场景");
            // 如果当前场景是主场景，就叠加
            yield return SceneManager.UnloadSceneAsync(from); 
            yield return SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);
        }
        else
        {
            // 如果非主场景，就卸载
            yield return SceneManager.UnloadSceneAsync(from); //卸载from
        }

        yield return Fade(0);
    }


    // 淡入淡出场景，1是黑，0是白
    private IEnumerator Fade(float targetAlpha)
    {
        isFade = true;
        fadeCanvasGroup.blocksRaycasts = true;

        float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / fadeDuration;
        
        while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
        {
            fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }

        isFade = false;
        fadeCanvasGroup.blocksRaycasts = false;
    }
}
