using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToScene : SingletonMono<FadeToScene>
{
    // CanvasGroup canvasGroup;
    // //渐入渐出时间
    // public float fadeInDuration;
    // public float fadeOutDuration;
    // private void Awake()
    // {
    //     canvasGroup = GetComponent<CanvasGroup>();
    //     DontDestroyOnLoad(gameObject);

    // }
    // //淡入淡出             在场景控制组件中调用
    // public IEnumerator FadeInOut()
    // {
    //     yield return FadeOut(fadeOutDuration);
    //     yield return FadeIn(fadeInDuration);

    // }
    // //淡出
    // public IEnumerator FadeOut(float time)
    // {
    //     while (canvasGroup.alpha < 1)
    //     {
    //         canvasGroup.alpha += Time.deltaTime / time;
    //         yield return null;
    //     }
    // }
    // //淡入
    // public IEnumerator FadeIn(float time)
    // {
    //     while (canvasGroup.alpha != 0)
    //     {
    //         canvasGroup.alpha -= Time.deltaTime / time;
    //         yield return null;
    //     }
    // }
}
